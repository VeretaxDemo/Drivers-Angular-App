using Drivers.Api.Models;

namespace Drivers.Api.Repositories;

public interface IDriverRepository
{
    Task<bool> CheckForDuplicateDriverAsync(Driver driver);
    Task AddAsync(Driver driver);
    Task<Driver> GetByIdAsync(string id);
    Task<List<Driver>> GetAllAsync();
}