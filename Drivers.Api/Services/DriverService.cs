using Drivers.Api.Configurations;
using Drivers.Api.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Drivers.Api.Services;

public class DriverService
{
    private readonly IMongoCollection<Driver> _driversCollection;

    public DriverService(IOptions<DatabaseSettings> databaseSettings)
    {
        // Initialize my mongodb client
        var mongoClient = new MongoClient(databaseSettings.Value.ConnectionString);

        // Connect to the mongodb database
        var mongoDb = mongoClient.GetDatabase(databaseSettings.Value.DatabaseName);

        // Connect db Collection to Driver
        _driversCollection = mongoDb.GetCollection<Driver>(databaseSettings.Value.CollectionName);
    }

    public async Task<List<Driver>> GetAsync() => await _driversCollection.Find(_ => true).ToListAsync();
}