using Drivers.Api.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Xml.Linq;

namespace Drivers.Api.Repositories;

// Create a repository interface

// Implement the repository class that interacts with the database
public class DriverRepository : IDriverRepository
{
    private readonly IMongoCollection<Driver> _driversCollection;

    public DriverRepository(IMongoCollection<Driver> driversCollection)
    {
        _driversCollection = driversCollection;
    }

    public async Task<List<Driver>> GetAllAsync()
    {
        return await _driversCollection.Find(_ => true).ToListAsync();
    }

    public async Task<Driver> GetByIdAsync(string id)
    {
        return await _driversCollection.Find(driver => driver.Id == id).FirstOrDefaultAsync();
    }

    public async Task<bool> CheckForDuplicateDriverAsync(Driver driver)
    {
        // Logic to check for duplicate driver based on name or team
        bool isDuplicate = await _driversCollection
            .Find(d => d.Name == driver.Name || d.Team == driver.Team)
            .AnyAsync();

        return isDuplicate;
    }

    public async Task AddAsync(Driver driver)
    {
        await _driversCollection.InsertOneAsync(driver);
    }

    public async Task<List<Driver>> SearchByNameAsync(string name)
    {
        var filter = Builders<Driver>.Filter.Regex(driver => driver.Name, new BsonRegularExpression(name, "i"));
        return await _driversCollection.Find(filter).ToListAsync();
    }


}