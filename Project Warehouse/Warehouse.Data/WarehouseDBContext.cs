using Microsoft.EntityFrameworkCore;
using Warehouse.Data.Entities;

namespace Warehouse.Data;

public class WarehouseDBContext : DbContext
{
    public WarehouseDBContext(DbContextOptions<WarehouseDBContext> options) : base(options){ }

    public DbSet<Bins> Bin => Set<Bins>();
    public DbSet<Customers> Customer => Set<Customers>();
    public DbSet<LocatedMaterials> LocatedMaterial => Set<LocatedMaterials>();
    public DbSet<Locations> Location => Set<Locations>();
    public DbSet<MaterialMovements> MaterialMovement => Set<MaterialMovements>();
    public DbSet<Materials> Material => Set<Materials>();
    public DbSet<MaterialsByModels> MaterialsByModel => Set<MaterialsByModels>();
    public DbSet<MaterialsByShipments> MaterialsByShipment => Set<MaterialsByShipments>();
    public DbSet<Models> Models => Set<Models>();
    public DbSet<Movements> Movement => Set<Movements>();
    public DbSet<Shipments> Shipment => Set<Shipments>();
    public DbSet<Users> Users => Set<Users>();
    public DbSet<Vendors> v => Set<Vendors>();



    protected override void OnModelCreating(ModelBuilder b)
    {
        
        b.Entity<Bins>(e =>
        {
            e.HasOne(p=>p.Location)
                    .WithOne(i => i.Product)
                    .HasForeignKey<InventoryItem>(i=>i.ProductId);
        });
    }


}