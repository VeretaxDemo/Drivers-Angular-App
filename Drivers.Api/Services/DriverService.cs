using Drivers.Api.Configurations;
using Drivers.Api.Models;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Drivers.Api.Services;

public class DriverService
{
    private readonly IMongoCollection<Driver> _driversCollection;

    public DriverService(IOptions<DatabaseSettings> databaseSettings, IMongoCollection<Driver> driversCollection)
    {
        // Initialize my mongodb client
        var mongoClient = new MongoClient(databaseSettings.Value.ConnectionString);

        // Connect to the mongodb database
        var mongoDb = mongoClient.GetDatabase(databaseSettings.Value.DatabaseName);

        // Connect db Collection to Driver
        //_driversCollection = mongoDb.GetCollection<Driver>(databaseSettings.Value.CollectionName);
        _driversCollection = driversCollection;
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
        bool isDuplicate = await CheckForDuplicateDriverAsync(driver);

        if (isDuplicate)
        {
            throw new ApplicationException("Duplicate driver found. Please provide a unique name or team name.");
        }

        await _driversCollection.InsertOneAsync(driver);
    }

    public async Task<bool> CheckForDuplicateDriverAsync(Driver driver)
    {
        // Logic to check for duplicate driver based on name or team
        bool isDuplicate = await _driversCollection
            .Find(d => d.Name == driver.Name || d.Team == driver.Team)
            .AnyAsync();

        return isDuplicate;
    }

}