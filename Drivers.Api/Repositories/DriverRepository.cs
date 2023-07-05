using Drivers.Api.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Serilog;
using System.Xml.Linq;
using ILogger = Serilog.ILogger;

namespace Drivers.Api.Repositories;

// Create a repository interface

// Implement the repository class that interacts with the database
public class DriverRepository : IDriverRepository
{
    private readonly IMongoCollection<Driver> _driversCollection;
    private readonly ILogger _logger;

    public DriverRepository(IMongoCollection<Driver> driversCollection, ILogger logger)
    {
        _driversCollection = driversCollection;
        _logger = logger;
    }

    public async Task<List<Driver>> GetAllAsync()
    {
        try
        {
            return await _driversCollection.Find(_ => true).ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Failed to get all drivers");
            throw;
        }
    }

    public async Task<Driver> GetByIdAsync(string id)
    {
        try
        {
            return await _driversCollection.Find(driver => driver.Id == id).FirstOrDefaultAsync();
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Failed to get driver by ID: {DriverId}", id);
            throw;
        }
    }

    public async Task<bool> CheckForDuplicateDriverAsync(Driver driver)
    {
        try
        {
            bool isDuplicate = await _driversCollection
                .Find(d => d.Name == driver.Name || d.Team == driver.Team)
                .AnyAsync();

            return isDuplicate;
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Failed to check for duplicate driver");
            throw;
        }
    }

    public async Task AddAsync(Driver driver)
    {
        try
        {
            await _driversCollection.InsertOneAsync(driver);
            _logger.Information("Driver added successfully: {@Driver}", driver);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Failed to add driver: {@Driver}", driver);
            throw;
        }
    }

    public async Task<List<Driver>> SearchByNameAsync(string name)
    {
        try
        {
            var filter = Builders<Driver>.Filter.Regex(driver => driver.Name, new BsonRegularExpression(name, "i"));
            return await _driversCollection.Find(filter).ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Failed to search drivers by name: {Name}", name);
            throw;
        }
    }

    public async Task<bool> RemoveAsync(string id)
    {
        try
        {
            var filter = Builders<Driver>.Filter.Eq(driver => driver.Id, id);
            var result = await _driversCollection.DeleteOneAsync(filter);
            return result.DeletedCount > 0;
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Failed to remove driver by ID: {DriverId}", id);
            throw;
        }
    }

    public async Task<bool> UpdateDriverAsync(Driver driver)
    {
        try
        {
            var filter = Builders<Driver>.Filter.Eq(d => d.Id, driver.Id);
            var update = Builders<Driver>.Update
                .Set(d => d.Name, driver.Name)
                .Set(d => d.Number, driver.Number)
                .Set(d => d.Team, driver.Team);

            var updateResult = await _driversCollection.UpdateOneAsync(filter, update);

            if (updateResult.ModifiedCount == 0)
            {
                _logger.Warning("Failed to update the driver: {@Driver}", driver);
                return false;
            }

            _logger.Information("Driver updated successfully: {@Driver}", driver);
            return true;
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Failed to update driver: {@Driver}", driver);
            throw;
        }
    }



}