using MongoDB.Driver;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using WebApplicationDemoS4;
using WebApplicationDemoS4.Entities;
using WebApplicationDemoS4.Data;


var builder = WebApplication.CreateBuilder(args);


// Read the MongoDB connection string from appsettings.json
//var mongoDbConnectionString = builder.Configuration.GetConnectionString("MongoDb");

// register MongoClient as a singleton
//builder.Services.AddSingleton<IMongoClient>(new MongoClient(mongoDbConnectionString));

// Get Product collection through database
//builder.Services.Configure<Product>(builder.Configuration.GetSection("MongoDBSettings"));

// Get Category collection through database
//builder.Services.Configure<Category>(builder.Configuration.GetSection("MongoDBSettings"));

// Register MongoContext for interacting with the db
builder.Services.AddSingleton<MongoContext>();

// Register SeedService to seed initial data
builder.Services.AddScoped<SeedService>();

// Add services to the container
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


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
    //var seedService = scope.ServiceProvider.GetRequiredService<SeedService>();
    //seedService.SeedData().Wait();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
