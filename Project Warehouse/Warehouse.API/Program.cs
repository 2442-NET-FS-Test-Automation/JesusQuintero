using Warehouse.Data;
using Warehouse.Data.Entities;
using Serilog;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var conn_string = "Server=localhost,1433;Database=WarehouseProject;User Id=sa;Password=LibraryPass1!;TrustServerCertificate=true";

Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()  
            .WriteTo.File("logs/fulfilment-log-.log", rollingInterval: RollingInterval.Day)
            .CreateLogger();

builder.Host.UseSerilog(); 

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();




var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();
