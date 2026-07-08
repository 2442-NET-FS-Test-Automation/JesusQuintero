using Library.ControllerApi.Filters;
using Library.ControllerApi.Mapping;
using Library.ControllerApi.Middleware;
using Library.ControllerApi.Services;
using Library.Data;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);



// Adding conection string
var conn_string = "Server=localhost,1433;Database=LibraryMinimalDb;User Id=sa;Password=LibraryPass1!;TrustServerCertificate=true";

builder.Services.AddDbContextFactory<LibraryDBContext>(o => o.UseSqlServer(conn_string));

// Registering oir custom Repo and Service Layer methods like we did before
builder.Services.AddScoped<IInventoryRepository, InventoryRepository>(); // Could later swap for inventpryMongoRepo
builder.Services.AddScoped<IInventoryService, InventoryService>();

// Adding out mapping profile for AutoMapper
builder.Services.AddAutoMapper(cfg => cfg.AddMaps(typeof(MappingProfile).Assembly));

builder.Services.AddControllers(o => o.Filters.Add<TimingFilter>());

Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()  // Write to console, and write to file - starting a new file each day.
            .WriteTo.File("logs/fulfilment-log-.log", rollingInterval: RollingInterval.Day)
            .CreateLogger();

builder.Host.UseSerilog(); // Telling the builder to use Serilog


// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Adding swagger back
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

// Swagger stuff added to app
app.UseSwagger();
app.UseSwaggerUI();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// This is a simple diagnostic middleware. All it will do is time our request for us and log that.
//It takes in ctx (HttpContext -> everuthing about the request AND the response)
// next - represents a call to the subsequent middleware
app.Use(async (ctx, next) =>
{
    var sw = System.Diagnostics.Stopwatch.StartNew();

    await next();

    sw.Stop();
    Log.Information("{Method} {Path} -> {StatusCode} in {Elapsed} ms",
        ctx.Request.Method, ctx.Request.Path, ctx.Response.StatusCode, sw.ElapsedMilliseconds);
});

app.Use(async (ctx, next) =>
{
    if (ctx.Request.Headers.ContainsKey("X-Manteinance"))
    {
        ctx.Response.StatusCode = StatusCodes.Status503ServiceUnavailable;
        await ctx.Response.WriteAsync("Down for maintenance");
        return; // don't call next() - never hits controllers
    }

    await next(ctx);
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
