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


        // Setting RowVersion's property
        b.Entity<LocatedMaterials>().Property(i => i.RowVersion).IsRowVersion();
        b.Entity<MaterialMovements>().Property(i => i.RowVersion).IsRowVersion();
        b.Entity<Materials>().Property(i => i.RowVersion).IsRowVersion();
        b.Entity<MaterialsByShipments>().Property(i => i.RowVersion).IsRowVersion();
        b.Entity<Movements>().Property(i => i.RowVersion).IsRowVersion();
        b.Entity<Shipments>().Property(i => i.RowVersion).IsRowVersion();


        // Setting unique values
        b.Entity<Users>().HasIndex(c => c.User_Email).IsUnique();
        b.Entity<Locations>().HasIndex(c => c.Location_Name).IsUnique();
        b.Entity<Materials>().HasIndex(c => c.Material_Name).IsUnique();
        b.Entity<Models>().HasIndex(c => c.Model_Name).IsUnique();
        b.Entity<Vendors>().HasIndex(c => c.Vendor_Email).IsUnique();
        b.Entity<Vendors>().HasIndex(c => c.Vendor_Name).IsUnique();
        b.Entity<Customers>().HasIndex(c => c.Customer_Email).IsUnique();

        
        // Seeding data
        b.Entity<Users>().HasData(
            new Users {User_Id = 1, 
                User_Fullname = "Jesus Eduardo Quintero", 
                User_Adress = "Cuauhtemoc #72", 
                User_Email = "jesus.quintero7478@alumnos.udg.mx",
                User_Password = "JesusEduardo"},
            new Users {User_Id = 2, 
                User_Fullname = "Jorge Flores Kuan", 
                User_Adress = "Ramon Corona #53", 
                User_Email = "jorge.flores0258@alumnos.udg.mx",
                User_Password = "JorgeFlores"}
        );

        b.Entity<Locations>().HasData(
            new Locations {Location_Id = 1, Location_Name = "0100"},
            new Locations {Location_Id = 2, Location_Name = "0101"},
            new Locations {Location_Id = 3, Location_Name = "0102"}
        );

        b.Entity<Bins>().HasData(
            new Bins {Bin_Id=1, Location_Id=1, Bin_Name="100-01A", RealBin = true},
            new Bins {Bin_Id=2, Location_Id=1, Bin_Name="100-01B", RealBin = true},
            new Bins {Bin_Id=3, Location_Id=1, Bin_Name="100-01C", RealBin = true},
            new Bins {Bin_Id=4, Location_Id=1, Bin_Name="100-01D", RealBin = true},
            new Bins {Bin_Id=5, Location_Id=1, Bin_Name="100-01E", RealBin = true},
            new Bins {Bin_Id=6, Location_Id=1, Bin_Name="100-02A", RealBin = true},
            new Bins {Bin_Id=7, Location_Id=1, Bin_Name="100-02B", RealBin = true},
            new Bins {Bin_Id=8, Location_Id=1, Bin_Name="100-02C", RealBin = true},
            new Bins {Bin_Id=9, Location_Id=1, Bin_Name="100-02D", RealBin = true},
            new Bins {Bin_Id=10, Location_Id=1, Bin_Name="100-02E", RealBin = true},
            new Bins {Bin_Id=11, Location_Id=2, RealBin = false},
            new Bins {Bin_Id=12, Location_Id=3, RealBin = false}
        );

        b.Entity<Vendors>().HasData(
            new Vendors {Vendor_Id = 1, Vendor_Name = "Production Line", Vendor_Email = "BossMail@example.com"},
            new Vendors {Vendor_Id = 2, Vendor_Name = "Universal Circuits", Vendor_Email = "UniversalCircuits@example.com"},
            new Vendors {Vendor_Id = 3, Vendor_Name = "Universal Plastics", Vendor_Email = "UniversalPlastics@example.com"},
            new Vendors {Vendor_Id = 4, Vendor_Name = "Universal Steels", Vendor_Email = "UniversalSteels@example.com"}
        );

        b.Entity<Materials>().HasData(
            new Materials {Material_Id = 1,
                Material_Name = "Circuit A",
                Material_Description = "Circuit type A for router",
                Vendor_Id = 2},
            new Materials {Material_Id = 2,
                Material_Name = "Casing A",
                Material_Description = "Casing for Router",
                Vendor_Id = 3},
            new Materials {Material_Id = 3,
                Material_Name = "Screw A",
                Material_Description = "Screw for router",
                Vendor_Id = 4},
            new Materials { Material_Id = 4,
                Material_Name = "Circuit B",
                Material_Description = "Circuit type B for fast router",
                Vendor_Id = 2},
            new Materials { Material_Id = 5,
                Material_Name = "Router",
                Material_Description = "Manufactered Router ready for shipping",
                Vendor_Id = 1},
            new Materials { Material_Id = 6,
                Material_Name = "Fast Router",
                Material_Description = "Manufactered Fast Router ready for shipping",
                Vendor_Id = 1}
        );

        b.Entity<Models>().HasData(
            new Models {Model_Id = 1, Model_Name = "Router", New_Material_Id = 5},
            new Models {Model_Id = 2, Model_Name = "Fast Router", New_Material_Id = 6}
        );

        b.Entity<MaterialsByModels>().HasData(
            new MaterialsByModels {MaterialByModel_Id = 1, Model_Id = 1, Material_Id = 1, Quantity = 1},
            new MaterialsByModels {MaterialByModel_Id = 2, Model_Id = 1, Material_Id = 2, Quantity = 1},
            new MaterialsByModels {MaterialByModel_Id = 3, Model_Id = 1, Material_Id = 3, Quantity = 4},
            new MaterialsByModels {MaterialByModel_Id = 4, Model_Id = 2, Material_Id = 4, Quantity = 1},
            new MaterialsByModels {MaterialByModel_Id = 5, Model_Id = 2, Material_Id = 2, Quantity = 1},
            new MaterialsByModels {MaterialByModel_Id = 6, Model_Id = 2, Material_Id = 3, Quantity = 4}
        );

        b.Entity<Customers>().HasData(
            new Customers {Customer_Id = 1, Customer_Name = "Router Universals", Customer_Email = "RouterUniversal@example.com"}
        );
        
        
    }


}