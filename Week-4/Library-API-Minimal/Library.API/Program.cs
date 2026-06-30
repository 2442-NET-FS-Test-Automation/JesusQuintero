using Microsoft.EntityFrameworkCore;
using Library.Data;

// This is my API program.cs
// No main. We can think of it as 2 sections
// Registering things with the builder
// And then configuring things on the app
// Ad at the very bottom that app object that represents our entire API call its run method

// Builder area
var builder = WebApplication.CreateBuilder(args);

// App area
var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();
