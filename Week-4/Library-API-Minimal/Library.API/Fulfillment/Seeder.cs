using Library.Data;
using Library.Data.Entities;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.EntityFrameworkCore;

// In "production" our orders would come from users. These API's run locally
// so we could either - create a post for a single order and run a shell script or something
// or we could create a seeding enpoint from here to generate some orders for us

public interface ISeeder
{
    IReadOnlyList<int> SeedOrders(int n, bool expedited);

}

public class Seeder : ISeeder
{
    // Going ahead and hardcoding some item SKUs (barcode numbers essentially in a list)
    private static readonly string[] Skus = {"BK-001", "BK-002", "BK-003"};
    private readonly IDbContextFactory<LibraryDBContext> _factory;

    public Seeder(IDbContextFactory<LibraryDBContext> factory)
    {
        _factory = factory;
    }
    public IReadOnlyList<int> SeedOrders(int n, bool expedited)
    {
        // Ask for a dbContext
        using var db = _factory.CreateDbContext();

        // Create a dictionary based on our product table (the IDs in the DB) and the SKUs
        var pid = db.Products.ToDictionary(p => p.Sku, p => p.Id); // Sku key, productId value

        // New list of ids
        var ids = new List<int>(n);

        // Based on n (number of orders the user want to seed)
        // lets use a for loop to create those orders programmatically

        for(int i = 0; i < n; i++)
        {
            var order = new Order
            {
                CustomerId = Random.Shared.Next(1, 3), // Random number - bounded
                Priority = expedited ? Priority.Expedited : Priority.Normal,
                Lines = { new OrderLine { ProductId = pid[Skus[i % Skus.Length]], Quantity = 1}}

            };

            db.Orders.Add(order);  // Add - stage changes if EF Core change tracker
            db.SaveChanges(); // persist the changes
            ids.Add(order.Id); // add the created order's ID to the id list
        }

        return ids;
    }
}