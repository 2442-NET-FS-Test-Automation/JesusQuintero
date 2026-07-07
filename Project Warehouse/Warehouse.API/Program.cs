using Warehouse.Data;
using Warehouse.Data.Entities;
using Serilog;
using Microsoft.EntityFrameworkCore;

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

app.MapGet("/list/materials", async (WarehouseDBContext db) =>
{
    await db.Materials.ToListAsync();
});
app.MapGet("/list/users", async (WarehouseDBContext db) =>
{
    await db.Users.ToListAsync();
});
app.MapGet("/list/locations", async (WarehouseDBContext db) =>
{
    await db.Locations.Include(l => l.Bins).ToListAsync();
});

app.Run();
