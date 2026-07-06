// This class will hold the business logic/db retry logic fot fulfilling transactions

using System.Data;
using Library.Data;
using Library.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Serilog;

namespace Library.Api.Fulfillment;

// ASP.NET's builder (DI container) NEEDS us to provide 2 things when we register a service
// An interface and a concrete implementation. These can both go in the same file.

public interface IFulfillmentService
{
    public Task<FulfillmentResult> FulfillOneAsync(int orderId, CancellationToken ct);
    public Task<BurstResult> FulfillBurstAsync(IEnumerable<int> orderIds, CancellationToken ct);
}

// Im going to stick everything about order fulfillment in this file
// Requests are either Fulfilled or Barckordered - no other results possible
public enum FulfillmentResult { Fulldilled, Backordered}

// Also going to make a record for the result of a Burst (many orders at the same time)
// recors are lightweight custom types that allow for comparison with ==
public record BurstResult (int Fulfilled, int Backordered);

public class FulfillmentService : IFulfillmentService
{
    // ASP.NET manages the creation (and destruction) of all our dependencies across our app
    // If we need a DBContext or DBContectFactory or Logger or any other dependency
    // we DO NOT instantiate one here, we ask for one via the Constructor
    private readonly IDbContextFactory<LibraryDBContext> _factory; // holds my factory
    
    // The factory in the constructor arguments list comes from the ASP.NET DI Container
    public FulfillmentService(IDbContextFactory<LibraryDBContext> factory)
    {
        _factory = factory;
    }

    // This method is going to handle filfillment - its gonna be a bit long. Wich is why we didn't
    // just write all of this in Program.cs
    public async Task<FulfillmentResult> FulfillOneAsync(int orderId, CancellationToken ct)
    {
        // First - we need a db contect
        await using var db = await _factory.CreateDbContextAsync(ct);

        // Lets grab our order from the database
        // Flow for this - a customer places an order. It hits the order table - we are now fulfilling that order
        var order = await db.Orders.Include(o => o.Lines).FirstAsync(o => o.Id == orderId, ct); // LINQ with async

        // Lets create that dictionary with the productId key and the OrderId value
        // yay for LINQ/Collections namespace
        var requested = order.Lines.ToDictionary(l => l.ProductId, l => l.OrderId);

        // Creating a flag for "can I continue fulfilling this order"
        bool canFufill = true;

        foreach(OrderLine line in order.Lines)
        {
            // First - grab the current inventory from the db for that product
            InventoryItem inv = await db.Inventory.FirstAsync(i => i.ProductId == line.ProductId, ct);

            // Nect - check if we can meet the order
            if(inv.CurrentStock < line.Quantity)
            {
                canFufill = false;
                break;
            }

            inv.CurrentStock -= line.Quantity; // This write to the inventoryItem table is guarded by RowVersion
        }

        // Assuming we broke out of the foreach and cannot fulfill the order
        if (!canFufill) // checking for canFulfull == false
        {
            // We can't fulfill this order, its now Backordered
            db.fulfillmentEvents.Add(new FulfillmentEvent { OrderId = orderId, Type = "Backorder"});

            await db.SaveChangesAsync(ct);

            // Log the transaction, using Serilog structured logging syntax
            Log.Warning("Backordered {OrderId}: insufficient stock", orderId);

            return FulfillmentResult.Backordered;
        }

        // If we make it, we CAN fulfill that order
        order.Status = Status.Fulfilled;
        order.CompletedUtc = DateTime.UtcNow;
        db.fulfillmentEvents.Add(new FulfillmentEvent {OrderId = orderId, Type = "Fulfilled"});

        // Adding our retry save method
        if(!await SaveWithRetryAsync(db, requested, ct)) // if we enter this id - we lost enoungh times
        { // that stock dropped this order was backordered
            db.ChangeTracker.Clear(); // clear change tracker
            Order staleOrder = await db.Orders.FirstAsync(o => o.Id == orderId, ct); // grab stale order from db
            staleOrder.Status = Status.Backordered; // set its status to backordered
            Log.Warning("Backordered order {OrderId} after concurrency retry", orderId);
            return FulfillmentResult.Backordered;
        }

        await db.SaveChangesAsync(ct);
        Log.Information("Fulfilled order: {OrderId}, {LineCount} lines", orderId, order.Lines.Count);
        return FulfillmentResult.Fulldilled;
    }

    // Lets break the logic for saving with retry (via RowVersion) into its own method
    // just to help keep things straight
    private static async Task<bool> SaveWithRetryAsync(
        LibraryDBContext db, IReadOnlyDictionary<int, int> requestedByProductId, CancellationToken ct)
    {
        // This is tha RowVersion Change Tracker entry retry from yesterday
        // Lets set max retries to 3 - by wrapping everything in a loop
        for(int attemp = 0; ; attemp++)
        {
            // Our loop as written never exists - its does increment attemp for us
            // If we retry and fail x amount of times - we will throw an exception manually
            try
            {
                // The DBContext inside this method came from FulfillOneAsync - if it has changes
                // staged to it - we can save them here. Its the same object.
                await db.SaveChangesAsync(ct);
                return true;
            }
            // We can tell our try catch how many times to handle this exception for us
            // After 3 attemps - we won't enter the catch. It bubbles up to whatever this method
            // was called 
            catch (DbUpdateConcurrencyException ex) when (attemp < 3)
            {
                // Retry Logic - remember that Change Tracker stuff?
                // entry is an EF Core Change Tracker entry
                foreach( var entry in ex.Entries)
                {
                    var current = await entry.GetDatabaseValuesAsync(); // grab the current database values

                    // If some other user deleted the entry out from under us.. we can't save
                    // return false
                    if(current is null) return false;

                    // Set the Original Values bucket on the entry to what they currently are
                    entry.OriginalValues.SetValues(current);

                    if(entry.Entity is InventoryItem inv)
                    {
                        // Grab the current total for that item's stock
                        int freshValue = current.GetValue<int>(nameof(InventoryItem.CurrentStock));
                        //Dictionary lookup against the dict we passed in
                        int desireAmount = requestedByProductId[inv.ProductId];

                        // Re-check on the fresh stock - don't blinfly trust it
                        if (freshValue < desireAmount) return false;

                        inv.CurrentStock = freshValue - desireAmount;
                    }
                }
            }
        }
    }

    public async Task<BurstResult> FulfillBurstAsync(IEnumerable<int> orderIds, CancellationToken ct)
    {
        // we are just going to piggyback off of FulfillOneAsync - no need to rewrite logic we can just call it again
        var tasks = orderIds.Select(id => FulfillOneAsync(id, ct)); // each call will get its own dbContext

        // await here until all tasks in the collection are complete
        var results = await Task.WhenAll(tasks);

        return new BurstResult(
            Fulfilled: results.Count(r => r == FulfillmentResult.Fulldilled),
            Backordered: results.Count(r => r == FulfillmentResult.Backordered)
        );
    }
}
