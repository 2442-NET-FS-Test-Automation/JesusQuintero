using Microsoft.EntityFrameworkCore;
using Library.Data;
using Library.Data.Entities;
using Serilog;
using Library.Api.Fulfillment;

// This is my API program.cs
// No main. We can think of it as 2 sections
// Registering things with the builder
// And then configuring things on the app
// Ad at the very bottom that app object that represents our entire API call its run method

// Builder area
var builder = WebApplication.CreateBuilder(args);

// The first thing that we need is to give our builder a connection string to our database
// var conn_string = "Data Source=localhost,1433;User ID=sa;Password=LibraryPass1!;Pooling=False;Connect Timeout=30;Encrypt=True;Trust Server Certificate=True;Authentication=SqlPassword;Application Name=vscode-mssql;Application Intent=ReadWrite;Command Timeout=30";
var conn_string = "Server=localhost,1433;Database=LibraryMinimalDb;User Id=sa;Password=LibraryPass1!;TrustServerCertificate=true";

Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()  // Write to console, and write to file - starting a new file each day.
            .WriteTo.File("logs/fulfilment-log-.log", rollingInterval: RollingInterval.Day)
            .CreateLogger();

builder.Host.UseSerilog(); // Telling the builder to use Serilog

// Tell the builder to use our LibraryDBContext with the connection sttring above
// By registering our DBContext class (or even classes, technically you use one per Database)
// we hand off the managging of creating and destroying these DBContext objects to ASP.NET's
// dependency injection container Like spring beans if you're familiar.

// ASP.NET has few different scope types
// Transient - a new instance is created every time it's requested.
// Scoped - a new instance per HTTP request
// Singleton - A single instance for the entire runtime of the app
builder.Services.AddDbContext<LibraryDBContext>(options => options.UseSqlServer(conn_string),
        ServiceLifetime.Scoped, ServiceLifetime.Singleton); // Scoped is the default, but we can be explicit when needed

// We know we will more than one LibraryBDContext in one or more of these methods. Bu we don't know how many
// before runtime. So we can use a DBContextFactory to create as many as we need at runtime.
builder.Services.AddDbContextFactory<LibraryDBContext>(options => options.UseSqlServer(conn_string));

// Registered our custom service with the builder
builder.Services.AddScoped<IFulfillmentService, FulfillmentService>();

// Swagger stuff added to builder
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// App area
var app = builder.Build();

// Swager stuff added to app
app.UseSwagger();
app.UseSwaggerUI();


// Endpoint area
app.MapGet("/", () => "Hello World!");

// Get all items from the inventory
app.MapGet("/Inventory", async (LibraryDBContext db) => {
    return await db.Inventory.ToListAsync();
});

// Lets use LINQ - Language Integrated Query
// LINQ is a library that just lets us query collections
// The logic actually flows from SQL DQL - You can use method OR sql query syntax
// You can even save the queries themselves as C# objects if you want to
app.MapGet("/Inventory/by-value", (LibraryDBContext db) =>
{
    return db.Inventory.Include(i => i.Product)
                        .GroupBy(i => i.CurrentStock >= 5 ? "Well-Stocked" : "Low") // group by just like in sql
                        .Select(g => new { tier = g.Key, count = g.Count(), units = g.Sum(i => i.CurrentStock)})
                        .ToList();
});


// Any endpoints that start with "/peek/*" are diagnostic/demo
// We are going to use them to expose things like EF Core change tracking and other
// underlying behaviors for learning. A real app would have no reason to expose HTTP endpoints
// to outside users to make this stuff observable.

app.MapGet("/peek/tracking", (LibraryDBContext db) =>
{
    // Lets see the underlying EF Core change tracker
    var unchanged = db.Products.First(); // grab the first object. Read but not modified => Unchanged
    var modified = db.Products.Skip(1).First(); // queried... still Unchanged as of here

    modified.Price += 1; // statse => Modified

    // When we create a new object and call the dbset's.Add() method it's state is
    // "Added" - this has not actually hit the database yet. But it's tracked to be added
    db.Products.Add(new Product {Sku = "BK-TMP", Name = "Tmp", Price = 1m} );

    // This bit of code is the non-production demo bit
    // We are accessing the LibraryDBContext object's change tracker to pull info
    // At most you's debug with this
    var states = db.ChangeTracker.Entries()
                    .Select(e => new { entity = e.Entity.GetType().Name, state = e.State.ToString()})
                    .ToList();

    // Crearing the change tracker manually
    db.ChangeTracker.Clear();


    return states;
});


// Lets manually go out of our way to create a conflict - obviusly, don't do this in a real app
app.MapGet("peek/conflict", (IServiceScopeFactory scopes) =>
{
    // Manually asking for scopes. Normally each endpoint method call gets its own scope tracked
    // by ASP.NET under the hood during runtime. We can, for various reasons good and bad do this manually
    using var scopeA = scopes.CreateScope();
    using var scopeB = scopes.CreateScope();

    // Now, remember that a dbContext is generated per scope, so we have to do that too
    var firstDB = scopeA.ServiceProvider.GetRequiredService<LibraryDBContext>();
    var secondDB = scopeB.ServiceProvider.GetRequiredService<LibraryDBContext>();


    // Each dbContext reads from the same database BUT they track changes independently
    // Remember we gave aaInventory entitirs a RowVersion - not just a property named RowVersion
    // but an actual OnModelCreation FluentAPI config for a RowVersion
    // Both of these start with the same RowVersion value
    var firstInventory = firstDB.Inventory.First(i => i.Id == 1);
    var secondInventory = secondDB.Inventory.First(i => i.Id == 1);

    // Lets modify one AND save its changes, while just modifiying the other
    firstInventory.CurrentStock --; // decrement => Modified
    firstDB.SaveChanges(); // save changes is whath persists any created, deleted or modified objects
    // that row in the DB now has a RowVersion of 2
    
    // Calling SaveChanges() above modifies the RowVersion  value

    // This object. that should represent the exact same row in the DB now has a stale RowVersion
    // before EF tries to persist any changes, it will check the RowVersion. It won't match
    // and an wxception will be thrown

    secondInventory.CurrentStock --; // RowVersion still 1 - doesnt matck DB

    try
    {
        secondDB.SaveChanges(); // thisshould fail as RowVersions don;t match
    }
    catch( DbUpdateConcurrencyException ex)
    {
        // In this case we want EF to retry the UPDATE 
        // Asking for thw actual ChangeTracker entry that threw the exception
        // this is EF Core specific
        var entry = ex.Entries.Single();

        // For the entry that threw the exception - grab it's current values from the DB
        // not the object, jist the values 
        var current = entry.GetDatabaseValues();

        // Every entry in the chanfe tracker tracks two sets of values.
        // OriginalValues = the values of the object when it was loaded from the db
        // CurrentValues = the new modified values we changed on the object in our app
        // Here we manually set the OriginalValues to the values from the DB we JUST grabbed
        entry.OriginalValues.SetValues(current!);

        // Using the entry to grab the actual item - going somewhat backward
        ((InventoryItem) entry.Entity).CurrentStock = 
            current!.GetValue<int>(nameof(InventoryItem.CurrentStock)) - 1;

        secondDB.SaveChanges();

    }

    // I can sent back specific codes via methos like .Ok() with messages inside
    // others include Problem(), NotFound(), etc
    return Results.Ok("Conflict caught, reloaded and retried");
});

// Endpoint to reset the stock of the items in my catalog - useful for testing and demo
// might need to hit this endpoint while we work
app.MapPost("/inventory/rest", (LibraryDBContext db, ILogger<Program> logger) =>
{
    // We just ask for an ILogger loke we do our DBContext
    // then use it as normal
    logger.LogInformation("Started seeing database");

    // What I want to do is reset the items that I know I stuck into the db.
    foreach(InventoryItem inv in db.Inventory) // for each item in my db Inventory table... do something
    {
        // I only want to do something if the primary key is 1, 2 or 3
        switch (inv.Id)
        {
            case 1:
            inv.CurrentStock = 5;
            break;
            case 2:
            inv.CurrentStock = 3;
            break;
            case 3:
            inv.CurrentStock = 8;
            break;
            default:
            break; 
        }
    }

    db.SaveChanges(); // persisting to DB
    logger.LogInformation("Stock reset");
    return Results.Ok("STOCK RESET");
});

// Fulfillment stuff for orders goes down here
// Im going to take in info from the front end (swagger for now)
// I have a few options
// I can take in from the uri/quiery string
// I can also take in parameters from the body

// Quick method to fulfill one order
app.MapPost("/orders", async (OrderPaylod orderRequest, IDbContextFactory<LibraryDBContext> factory,
            CancellationToken ct, IFulfillmentService fsvc) =>
{
    // Remember we create an order in our db
    // And then try to create a Succesful fulfilment record against the db
    await using var db = await factory.CreateDbContextAsync(ct); // ask for db contrct to place order

    var newOrder = new Order
    {
        CustomerId = orderRequest.CustomerId,
        Priority = Priority.Normal,
        // Using the orderRequest from the HTTP request to create my order
        Lines = {new OrderLine { ProductId = orderRequest.ProductId, Quantity = orderRequest.Quantity}}
    };

    db.Orders.Add(newOrder); // Add new order
    await db.SaveChangesAsync(ct); // save that order to db

    // Now that we've added the order - we try to fulfill it
    var result = await fsvc.FulfillOneAsync(newOrder.Id, ct); // newOrder os now in the db, we can ask for it
    return Results.Ok(new {orderId = newOrder.Id, result = result.ToString()});


});



// My file always ends with app.Run() - minimal API or Controller API
app.Run();

Log.CloseAndFlush();
public record OrderPaylod(int ProductId, int Quantity, int CustomerId);
