using Warehouse.Data;
using Warehouse.Data.Entities;
using Warehouse.Data.Services;
using Serilog;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;
using Warehouse.Data.Records;
using System.Collections.Concurrent;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Diagnostics;
using Azure;

var builder = WebApplication.CreateBuilder(args);

var conn_string = "Server=localhost,1433;Database=WarehouseProject;User Id=sa;Password=LibraryPass1!;TrustServerCertificate=true";

builder.Services.AddDbContext<WarehouseDBContext>(options => options.UseSqlServer(conn_string),
        ServiceLifetime.Scoped, ServiceLifetime.Singleton);

builder.Services.AddDbContextFactory<WarehouseDBContext>(options => options.UseSqlServer(conn_string));

Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()  
            .WriteTo.File("logs/fulfilment-log-.log", rollingInterval: RollingInterval.Day)
            .CreateLogger();

builder.Host.UseSerilog(); 

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddScoped<IWarehouseFactory, WarehouseFactory>();
builder.Services.AddScoped<IBinInventoryService, BinInventoryService>();


var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapPost("/Reset-Transactions", async (WarehouseDBContext db, ILogger<Program> logger) =>
{
    logger.LogWarning("Deleting operation records");
    int deletedRecords =  0;
    deletedRecords += await db.MaterialMovements.ExecuteDeleteAsync();
    deletedRecords += await db.Movements.ExecuteDeleteAsync();
    deletedRecords += await db.LocatedMaterials.ExecuteDeleteAsync();
    deletedRecords += await db.MaterialsByShipments.ExecuteDeleteAsync();
    deletedRecords += await db.Shipments.ExecuteDeleteAsync();

    logger.LogInformation("{deletedRecors records deleted}", deletedRecords);
    return Results.Ok(new {message = $"{deletedRecords} records deleted"});
});

app.MapGet("/list/materials", async (WarehouseDBContext db) =>
{
    Log.Information("Getting materials information");
    return await db.Materials.ToListAsync();
});
app.MapGet("/list/users", async (WarehouseDBContext db) =>
{
    Log.Information("Getting users information");
    return await db.Users.ToListAsync();
});
app.MapGet("/list/locations", async (WarehouseDBContext db) =>
{
    Log.Information("Getting locations information");
    return await db.Locations
        .Select(l => new 
        {
            l.Location_Id,
            l.Location_Name,
            Bins = l.Bins.Select(b => new {b.Bin_Id, b.Bin_Name, b.RealBin})
        })
        .ToListAsync();
    
});
app.MapGet("/list/Movements", async (WarehouseDBContext db) =>
{
    Log.Information("Getting Movements information");
    return await db.Movements
        .Select(m => new 
        {
            m.Movement_Id,
            m.User_Id,
            m.LastBinLocation_Id,
            m.NewBinLocation_Id,
            m.Movement_Time,
            MaterialMovements = m.MaterialMovements.Select(mm => new {mm.Material_Id, mm.Quantity})
        })
        .ToListAsync();
    
});
app.MapGet("/list/BinMaterials/{bin}", async (WarehouseDBContext db, int bin) =>
{
    Log.Information("Getting bin {bin} information", bin);
    return await db.LocatedMaterials
        .Where(l => l.Bin_Id == bin)
        .Select(l => new 
        {
            l.Material_Id,
            l.Quantity        
        }).ToListAsync();
    
});

app.MapGet("/list/Shipments", async (WarehouseDBContext db) =>
{
    Log.Information("Getting shipment information");
    return await db.Shipments
        .Select(s => new 
        {
            s.Shipment_Id,
            s.Sale_Price,
            s.Shipment_Date,
            s.Customer_Id,
            MaterialsByShipments = s.MaterialsByShipments.Select(mm => new {mm.Material_Id, mm.Quantity})
        })
        .ToListAsync();
});

app.MapPost("/Add-Stock", async (WarehouseDBContext db, EntryMaterialDto materials, IWarehouseFactory factory) =>
{
    Movements newEntry = factory.MakeMovement(MovementTypes.Entry, materials.materials, materials.userId, toBin:materials.toBin);

    db.Movements.Add(newEntry);

    foreach(MaterialMovementDto mat in materials.materials)
    {
        LocatedMaterials? loc = await db.LocatedMaterials.FirstOrDefaultAsync(l => l.Bin_Id == materials.toBin && l.Material_Id == mat.idMaterial);
        if (loc == null)
        {
            loc = new LocatedMaterials { Bin_Id =  materials.toBin, Material_Id = mat.idMaterial, Quantity = mat.quantity};
            db.LocatedMaterials.Add(loc);
        }
        else
        {
            loc.Quantity += mat.quantity;
        }
    }
    Log.Information("Entry processed succesfully");
    await db.SaveChangesAsync();

    return Results.Created($"/movements/{newEntry.Movement_Id}", new {newEntry.Movement_Id});
});

app.MapPost("/Move-Stock", async (WarehouseDBContext db, MoveMaterialDto materials, IWarehouseFactory factory, IBinInventoryService invService) =>
{

    try
    {
        await invService.ReduceBinInventory(db,materials.fromBin, materials.materials);
        await invService.AddBinInventory(db,materials.toBin,materials.materials);

        Movements newEntry = factory.MakeMovement(MovementTypes.Movement, materials.materials, materials.userId, materials.fromBin ,materials.toBin);

        db.Movements.Add(newEntry);
        Log.Information("Movement processed succesfully");
        await db.SaveChangesAsync();
        
        return Results.Ok();
    }
    catch(InsufficientStockException ex)
    {
        return Results.BadRequest(new {message = ex.Message});
    }

});

app.MapPost("/Make-Shipment", async (WarehouseDBContext db, ShipMaterialDto materials, IWarehouseFactory factory, 
                                    IBinInventoryService invService, decimal price, int customerId) =>
{

    try
    {
        await invService.ReduceBinInventory(db,materials.fromBin, materials.materials);

        Movements newEntry = factory.MakeMovement(MovementTypes.Shipment, materials.materials, materials.userId, materials.fromBin);
        db.Movements.Add(newEntry);
        await factory.MakeShipment(db, materials.materials, customerId, price);
        Log.Information("Shipment processed succesfully");
        await db.SaveChangesAsync();
        
        return Results.Ok();
    }
    catch(AggregateException ex) when (ex.InnerExceptions.Any(e => e is InsufficientStockException))
    {
        return Results.BadRequest(new {message = ex.Message});
    }

});

app.MapPost("/Burst/Movements-Priority", async (WarehouseDBContext db, IWarehouseFactory factory, 
                                    IBinInventoryService invService, int numberOfMovements) =>
{
    PriorityQueue<GenericMaterialMovementDto, int> myMovements = await factory.GenerateBurst(numberOfMovements);
    Stopwatch sw = new Stopwatch();
    sw.Start();
    BurstResult br = await factory.RunSecuentialBurst(db, factory, invService, myMovements);
    sw.Stop();
    await db.SaveChangesAsync();
    Log.Information("Succesfull Entry: {sEntry}", br.compEntries);
    Log.Information("Succesfull Movement: {sMov}", br.compMovements);
    Log.Information("Succesfull Shipment: {sShip}", br.compShipments);
    Log.Information("Failed Movemnt: {fMov}", br.failMovements);
    Log.Information("Failed Shipment: {fShip}", br.failShipments);
    Log.Information("Execution time from burst {time}", sw.ElapsedMilliseconds);
    string oKMessage = $"Succesfull Operations: Entry-{br.compEntries}, Movement-{br.compMovements}, Shipment-{br.compShipments}";
    oKMessage += $"\nFailed Operations:  Movement-{br.failMovements}, Shipment-{br.failShipments}";
    oKMessage += $"\nExecution time from burst {sw.ElapsedMilliseconds}";

    return Results.Ok(oKMessage);
});


app.Run();

