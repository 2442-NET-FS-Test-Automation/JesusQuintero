using Warehouse.Data;
using Warehouse.Data.Entities;
using Serilog;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;

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




var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapPost("/Reset-Transactions", async (WarehouseDBContext db, ILogger<Program> logger) =>
{
    logger.LogInformation("Starting to delete records");
    int deletedRecords =  0;
    deletedRecords += await db.Movements.ExecuteDeleteAsync();
    deletedRecords += await db.LocatedMaterials.ExecuteDeleteAsync();
    deletedRecords += await db.Shipments.ExecuteDeleteAsync();

    logger.LogInformation("{deletedRecors records deleted}", deletedRecords);
    return Results.Ok(new {message = $"{deletedRecords} records deleted"});
});

app.MapGet("/list/materials", async (WarehouseDBContext db) =>
{
    return await db.Materials.ToListAsync();
});
app.MapGet("/list/users", async (WarehouseDBContext db) =>
{
    return await db.Users.ToListAsync();
});
app.MapGet("/list/locations", async (WarehouseDBContext db) =>
{
    return await db.Locations
        .Select(l => new 
        {
            l.Location_Id,
            l.Location_Name,
            Bins = l.Bins.Select(b => new {b.Bin_Id, b.Bin_Name, b.RealBin})
        })
        .ToListAsync();
    
});

app.MapPost("/Add-Stock", async (WarehouseDBContext db, int idMaterial, string bin_Name, int quantity) =>
{
    
});

app.Run();
