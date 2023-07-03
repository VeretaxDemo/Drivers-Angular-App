using Drivers.Api.Models;

namespace Drivers.Api.Repositories;

public interface IDriverRepository
{
    Task<List<Driver>> GetAllAsync();
    Task<Driver> GetByIdAsync(string id);
    Task<bool> CheckForDuplicateDriverAsync(Driver driver);
    Task AddAsync(Driver driver);
    Task<List<Driver>> SearchByNameAsync(string name);
    Task<bool> RemoveAsync(string id);
    Task<bool> UpdateDriverAsync(Driver driver);
}