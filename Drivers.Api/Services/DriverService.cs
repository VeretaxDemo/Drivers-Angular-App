using Drivers.Api.Configurations;
using Drivers.Api.Models;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
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

    public async Task<Driver> GetByIdAsync(string id)
    {
        return await _driversCollection.Find(driver => driver.Id == id).FirstOrDefaultAsync();
    }

    public async Task<List<Driver>> SearchByNameAsync(string name)
    {
        var filter = Builders<Driver>.Filter.Regex(driver => driver.Name, new BsonRegularExpression(name, "i"));
        return await _driversCollection.Find(filter).ToListAsync();
    }

    public async Task AddAsync(Driver driver)
    {
        await _driversCollection.InsertOneAsync(driver);
    }

}