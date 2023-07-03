using Drivers.Api.Configurations;
using Drivers.Api.Models;
using Drivers.Api.Repositories;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data;

namespace Drivers.Api.Services;

public class DriverService : IDriverService
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

    [ImportingConstructor]
    public DriverService(IDriverRepository driverRepository, IMongoCollection<Driver> driversCollection)
    {
        _driverRepository = driverRepository;
        _driversCollection = driversCollection;
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
        bool isDuplicate = await _driverRepository.CheckForDuplicateDriverAsync(driver);

        return isDuplicate;
    }
    public async Task<bool> RemoveAsync(string id)
    {
        var driver = await _driverRepository.GetByIdAsync(id);
        if (driver == null)
        {
            return false;
            //throw new ApplicationException("Driver not found.");
        }

        return await _driverRepository.RemoveAsync(id);
    }

    public async Task<bool> UpdateDriverAsync(Driver driver)
    {
        var existingDriver = await _driverRepository.GetByIdAsync(driver.Id);
        if (existingDriver == null)
        {
            return false;
        }

        // Note that working in this way means driver could be replaced in a car number
        // on a team without updating the driver ID
        existingDriver.Name = driver.Name;
        existingDriver.Number = driver.Number;
        existingDriver.Team = driver.Team;

        await _driverRepository.UpdateDriverAsync(existingDriver);

        return true;
    }

}