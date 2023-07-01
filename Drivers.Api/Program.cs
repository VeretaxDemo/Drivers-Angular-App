using Drivers.Api.Configurations;
using Drivers.Api.Models;
using Drivers.Api.Repositories;
using Drivers.Api.Services;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<DatabaseSettings>(builder.Configuration.GetSection("MongoDatabase"));

// Add MongoDB client and database
builder.Services.AddSingleton<IMongoClient>(provider =>
{
    var databaseSettings = provider.GetService<IOptions<DatabaseSettings>>()?.Value;
    return new MongoClient(databaseSettings.ConnectionString);
});

builder.Services.AddSingleton<IMongoCollection<Driver>>(provider =>
{
    var mongoClient = provider.GetService<IMongoClient>();
    var databaseSettings = provider.GetService<IOptions<DatabaseSettings>>()?.Value;
    var database = mongoClient.GetDatabase(databaseSettings.DatabaseName);
    return database.GetCollection<Driver>(databaseSettings.CollectionName);
});

builder.Services.AddSingleton<IDriverRepository, DriverRepository>();
builder.Services.AddSingleton<IDriverService, DriverService>();

builder.Services.AddControllers();

// Add CORS rule
builder.Services.AddCors(options => options.AddPolicy("AngularClient", policy =>
{
    policy.WithOrigins("https://localhost:4200")
        .AllowAnyMethod()
        .AllowAnyHeader();
    policy.WithOrigins("http://localhost:4200")
        .AllowAnyMethod()
        .AllowAnyHeader();
}));



// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
});


builder.Services.AddSingleton<DriverService>();

var app = builder.Build();

//app.UseCors(options =>
//{
//    options.AllowAnyOrigin()
//        .AllowAnyMethod()
//        .AllowAnyHeader();
//});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API v1"));
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseCors("AngularClient");

app.UseAuthorization();

app.MapControllers();

app.Run();
