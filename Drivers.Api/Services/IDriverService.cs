﻿using Drivers.Api.Models;

namespace Drivers.Api.Services;

public interface IDriverService
{
    Task<List<Driver>> GetAsync();
    Task<Driver> GetByIdAsync(string id);
    Task<List<Driver>> SearchByNameAsync(string name);
    Task AddAsync(Driver driver);
    Task<bool> CheckForDuplicateDriverAsync(Driver driver);
    Task<bool> RemoveAsync(string id);
    Task<bool> UpdateDriverAsync(Driver driver);
}