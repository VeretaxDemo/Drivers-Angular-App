using Drivers.Api.Models;
using Drivers.Api.Services;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace Drivers.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DriversController : ControllerBase
{
    private readonly ILogger<WeatherForecastController> _logger;
    private readonly DriverService _driverService;
    public DriversController(ILogger<WeatherForecastController> logger, DriverService driverService)
    {
        _logger = logger;
        _driverService = driverService;
    }



    public static bool TestMongoDBConnection(string connectionString)
    {
        try
        {
            var mongoClient = new MongoClient(connectionString);
            mongoClient.ListDatabases(); // Try accessing the databases to test the connection
            return true;
        }
        catch (Exception ex)
        {
            // Handle or log the exception
            return false;
        }
    }


[HttpGet]
    public async Task<IActionResult> GetDrivers()
    {
        string connectionString = "mongodb://localhost:27717";
        bool isConnected = TestMongoDBConnection(connectionString);

        if (isConnected)
        {
            Console.WriteLine("Connected to MongoDB.");
        }
        else
        {
            Console.WriteLine("Failed to connect to MongoDB.");
        }

        var drivers = await _driverService.GetAsync();

        return Ok(drivers);
    }
}
