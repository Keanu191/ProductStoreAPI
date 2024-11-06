using MongoDB.Driver;
using WebApplicationDemoS4.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using WebApplicationDemoS4;


var builder = WebApplication.CreateBuilder(args);


// Read the MongoDB connection string
var mongoDbConnectionString = builder.Configuration.GetConnectionString("MongoDb");
builder.Services.AddSingleton<IMongoClient>(new MongoClient(mongoDbConnectionString));


// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register MongoContext.cs/SeedService.cs
builder.Services.AddSingleton<MongoContext>();
builder.Services.AddScoped<SeedService>();


// Call MongoDB with both collections
builder.Services.Configure<Category>(
    builder.Configuration.GetSection("MongoDBDatabase"));

builder.Services.Configure<Product>(
    builder.Configuration.GetSection("MongoDBDatabase"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Seed data when the app starts
using (var scope = app.Services.CreateScope())
{
    var seedService = scope.ServiceProvider.GetRequiredService<SeedService>();
    seedService.SeedData().Wait();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
