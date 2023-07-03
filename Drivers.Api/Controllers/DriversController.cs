using Drivers.Api.Models;
using Drivers.Api.Services;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace Drivers.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DriversController : ControllerBase
{
    private readonly ILogger<DriversController> _logger;
    private readonly IDriverService _driverService;
    public DriversController(ILogger<DriversController> logger, IDriverService driverService)
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

    [HttpGet("{id}")]
    public async Task<ActionResult<Driver>> GetById(string id)
    {
        var driver = await _driverService.GetByIdAsync(id);
        if (driver == null)
        {
            return NotFound();
        }
        return driver;
    }

    [HttpGet("search")]
    public async Task<ActionResult<List<Driver>>> SearchByName([FromQuery] string name)
    {
        var drivers = await _driverService.SearchByNameAsync(name);
        if (drivers == null || drivers.Count == 0)
        {
            return NotFound();
        }
        return drivers;
    }

    [HttpPost]
    public async Task<ActionResult<Driver>> AddDriver(Driver driver)
    {
        try
        {
            bool isDuplicate = await _driverService.CheckForDuplicateDriverAsync(driver);
            if (isDuplicate)
            {
                return BadRequest("Duplicate driver found.");
            }
            await _driverService.AddAsync(driver);
            return CreatedAtAction(nameof(GetById), new { id = driver.Id }, driver);
        }
        catch (ApplicationException ex)
        {
            // Handle the ApplicationException and return a suitable response
            return StatusCode(500, "An error occurred while adding the driver.");
            // note we could do this by passing the internal exception message
            // however, it might leak internals of the api. return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> RemoveDriver(string id)
    {
        var result = await _driverService.RemoveAsync(id);
        if (result)
        {
            return NoContent(); // Return 204 No Content if the driver was successfully removed
        }
        else
        {
            return NotFound($"Unable to remove driver with id: {id}. Driver not found"); // Return 404 Not Found if the driver was not found
        }
    }

}
