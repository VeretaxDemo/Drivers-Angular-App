﻿using Drivers.Api.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

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

    public async Task<bool> RemoveAsync(string id)
    {
        var filter = Builders<Driver>.Filter.Eq(driver => driver.Id, id);
        var result = await _driversCollection.DeleteOneAsync(filter);
        return result.DeletedCount > 0;
    }

    public async Task<bool> UpdateDriverAsync(Driver driver)
    {
        var filter = Builders<Driver>.Filter.Eq(d => d.Id, driver.Id);
        var update = Builders<Driver>.Update
            .Set(d => d.Name, driver.Name)
            .Set(d => d.Number, driver.Number)
            .Set(d => d.Team, driver.Team);

        var updateResult = await _driversCollection.UpdateOneAsync(filter, update);

        if (updateResult.ModifiedCount == 0)
        {
            throw new Exception("Failed to update the driver.");
        }

        return true;
    }



}