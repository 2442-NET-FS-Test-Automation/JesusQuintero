using Microsoft.EntityFrameworkCore;
using Library.Data.Entities;

namespace Library.Data;

// All the code that does the actual SQL generation, creating a connection to my database,
// doing CRUD, updating the DB based on changes to my models - ALL OF THAT lives in class
// called DBContect. I don't want tomodify that class. It comes in from EF Core itself. What I do
// is create a file with a class that INHERITS from it.
public class LibraryDBContext : DbContext
{
    // This class needs a constructor, and it needs to take a certain argument
    // We ourselves will never call this constructor. ASP.NET's DI Container will do it for us
    public LibraryDBContext(DbContextOptions<LibraryDBContext> options) : base(options){ }

    // We need to tell our DBContext what C# classes we are tracking as Entities
    // Reminder - these Entities become our tables
    public DbSet<Product> Products => Set<Product>();
    public DbSet<InventoryItem> Inventory => Set<InventoryItem>();
}