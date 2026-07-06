using Microsoft.EntityFrameworkCore;
using Warehouse.Data.Entities;

namespace Warehouse.Data;

public class WarehouseDBContext : DbContext
{
    public WarehouseDBContext(DbContextOptions<WarehouseDBContext> options) : base(options){ }

    public DbSet<Bins> Bins => Set<Bins>();
    public DbSet<Customers> Customers => Set<Customers>();
    public DbSet<LocatedMaterials> LocatedMaterials => Set<LocatedMaterials>();
    public DbSet<Locations> Locations => Set<Locations>();
    public DbSet<MaterialMovements> MaterialMovements => Set<MaterialMovements>();
    public DbSet<Materials> Materials => Set<Materials>();
    public DbSet<MaterialsByModels> MaterialsByModels => Set<MaterialsByModels>();
    public DbSet<MaterialsByShipments> MaterialsByShipments => Set<MaterialsByShipments>();
    public DbSet<Models> Models => Set<Models>();
    public DbSet<Movements> Movements => Set<Movements>();
    public DbSet<Shipments> Shipments => Set<Shipments>();
    public DbSet<Users> Users => Set<Users>();
    public DbSet<Vendors> Vendors => Set<Vendors>();



    protected override void OnModelCreating(ModelBuilder b)
    {
        // Bins relations
        b.Entity<Bins>().HasOne(b => b.Location)
                        .WithMany(l => l.Bins)
                        .HasForeignKey(b => b.Location_Id);

        
        // Movements relations
        b.Entity<Movements>().HasOne(m => m.User)
                             .WithMany(u => u.Movements)
                             .HasForeignKey(m => m.User_Id);
        
        b.Entity<Movements>().HasOne(m => m.LastBinLocation)
                             .WithMany()
                             .HasForeignKey(m => m.LastBinLocation_Id);
        
        b.Entity<Movements>().HasOne(m => m.NewBinLocation)
                             .WithMany()
                             .HasForeignKey(m => m.NewBinLocation_Id);


        //MaterialMovements relations
        b.Entity<MaterialMovements>().HasOne(mm => mm.Movement)
                                     .WithMany(m => m.MaterialMovements)
                                     .HasForeignKey(mm => mm.Movement_Id);
                                     
        b.Entity<MaterialMovements>().HasOne(mm => mm.Material)
                                     .WithMany(m => m.MaterialMovements)
                                     .HasForeignKey(mm => mm.Material_Id);

        
        // LocatedMaterials relations
        b.Entity<LocatedMaterials>()
            .HasOne(lm => lm.Material)
            .WithMany(m => m.LocatedMaterials)
            .HasForeignKey(lm => lm.Material_Id);
        
        b.Entity<LocatedMaterials>()
            .HasOne(lm => lm.Bin)
            .WithMany(b => b.LocatedMaterials)
            .HasForeignKey(lm => lm.Bin_Id);

        
        // Materials relations
        b.Entity<Materials>()
            .HasOne(m => m.vendor)
            .WithMany(v => v.Materials)
            .HasForeignKey(m => m.Vendor_Id);

        
        // Models relations
        b.Entity<Models>()
            .HasOne(m => m.New_Material)
            .WithMany(mat => mat.Models)
            .HasForeignKey(m => m.New_Material_Id);
        

        // MaterialsByModel relations
        b.Entity<MaterialsByModels>()
            .HasOne(mm => mm.Material)
            .WithMany(m => m.MaterialsByModels)
            .HasForeignKey(mm => mm.Material_Id);
        
        b.Entity<MaterialsByModels>()
            .HasOne(mm => mm.model)
            .WithMany(m => m.materialsByModels)
            .HasForeignKey(mm => mm.Model_Id);
        

        // MaterialsByShimpent relations
        b.Entity<MaterialsByShipments>()
            .HasOne(ms => ms.Material)
            .WithMany(m => m.MaterialsByShipments)
            .HasForeignKey(ms => ms.Material_Id);
        
        b.Entity<MaterialsByShipments>()
            .HasOne(ms => ms.Shipment)
            .WithMany(s => s.MaterialsByShipments)
            .HasForeignKey(ms => ms.Shipment_Id);
        

        // Shipments relations
        b.Entity<Shipments>()
            .HasOne(s => s.Customer)
            .WithMany(c => c.Shipments)
            .HasForeignKey(s => s.Customer_Id);


        // Setting RowVersion's
        
                        
        
        
        
    }


}