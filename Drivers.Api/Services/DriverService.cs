using Drivers.Api.Configurations;
using Drivers.Api.Models;
using Drivers.Api.Repositories;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;

namespace Drivers.Api.Services;

public class DriverService
{
    private readonly IDriverRepository _driverRepository;
    private readonly IMongoCollection<Driver> _driversCollection;

    //public DriverService(IOptions<DatabaseSettings> databaseSettings, IMongoCollection<Driver> driversCollection)
    //{
    //    // Initialize my mongodb client
    //    var mongoClient = new MongoClient(databaseSettings.Value.ConnectionString);

    //    // Connect to the mongodb database
    //    var mongoDb = mongoClient.GetDatabase(databaseSettings.Value.DatabaseName);

    //    // Connect db Collection to Driver
    //    //_driversCollection = mongoDb.GetCollection<Driver>(databaseSettings.Value.CollectionName);
    //    _driversCollection = driversCollection;
    //}

    public DriverService(IDriverRepository driverRepository)
    {
        _driverRepository = driverRepository;
    }


    public async Task<List<Driver>> GetAsync() => await _driverRepository.GetAllAsync();

    public async Task<Driver> GetByIdAsync(string id)
    {
        return await _driverRepository.GetByIdAsync(id);
    }

    public async Task<List<Driver>> SearchByNameAsync(string name)
    {
        return await _driverRepository.SearchByNameAsync(name);
    }

    public async Task AddAsync(Driver driver)
    {
        bool isDuplicate = await _driverRepository.CheckForDuplicateDriverAsync(driver);

        if (isDuplicate)
        {
            throw new ApplicationException("Duplicate driver found. Please provide a unique name or team name.");
        }

        await _driverRepository.AddAsync(driver);
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