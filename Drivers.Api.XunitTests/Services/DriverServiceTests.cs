using Drivers.Api.Configurations;
using Drivers.Api.Models;
using Drivers.Api.Repositories;
using Drivers.Api.Services;
using FluentAssertions;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Drivers.Api.XunitTests.Services
{
    public class DriverServiceTests
    {
        [Fact]
        public async Task GetAsync_WhenDriversExist_ShouldReturnListOfDrivers()
        {
            // Arrange
            var expectedDrivers = new List<Driver>
            {
                new Driver { Id = "1", Name = "John Doe", Team = "Team A" },
                new Driver { Id = "2", Name = "Jane Smith", Team = "Team B" },
                new Driver { Id = "3", Name = "Mike Johnson", Team = "Team A" }
            };

            var repositoryMock = new Mock<IDriverRepository>();
            repositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(expectedDrivers);

            var driverService = new DriverService(repositoryMock.Object);

            // Act
            var result = await driverService.GetAsync();

            // Assert
            result.Should().BeEquivalentTo(expectedDrivers);
        }

        [Fact]
        public async Task GetAsync_WhenNoDriversExist_ShouldReturnEmptyList()
        {
            // Arrange
            var repositoryMock = new Mock<IDriverRepository>();
            repositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<Driver>());

            var driverService = new DriverService(repositoryMock.Object);

            // Act
            var result = await driverService.GetAsync();

            // Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public async Task GetByIdAsync_WhenDriverExists_ShouldReturnDriver()
        {
            // Arrange
            var mockRepository = new Mock<IDriverRepository>();
            var driverService = new DriverService(mockRepository.Object);

            var driverId = "1";
            var expectedDriver = new Driver { Id = driverId, Name = "John Doe", Team = "Team A" };

            mockRepository
                .Setup(r => r.GetByIdAsync(driverId))
                .ReturnsAsync(expectedDriver);

            // Act
            var result = await driverService.GetByIdAsync(driverId);

            // Assert
            result.Should().BeEquivalentTo(expectedDriver);
        }

        [Fact]
        public async Task GetByIdAsync_WhenDriverDoesNotExist_ShouldReturnNull()
        {
            // Arrange
            var mockRepository = new Mock<IDriverRepository>();

            string driverId = "-1";

            var drivers = new List<Driver>
            {
                new Driver { Id = "1", Name = "John Doe", Team = "Team A" },
                new Driver { Id = "2", Name = "Jane Smith", Team = "Team B" },
                // Add more driver objects as needed
            };
            mockRepository
                .Setup(r => r.GetByIdAsync(It.IsAny<string>()))
                .ReturnsAsync((string id) => drivers.FirstOrDefault(d => d.Id == id));
            var driverService = new DriverService(mockRepository.Object);
            var invalidId = "invalid_id";

            // Act
            var result = await driverService.GetByIdAsync(driverId);

            // Assert
            result.Should().BeNull();
            mockRepository.Verify(r => r.GetByIdAsync(driverId), Times.Once);
        }


        [Fact]
        public async Task AddAsync_WhenNoDuplicateDriver_ShouldInsertDriver()
        {
            // Arrange
            var mockRepository = new Mock<IDriverRepository>();
            var driverService = new DriverService(mockRepository.Object);

            var driver = new Driver { Name = "John Doe", Team = "Team A" };

            mockRepository
                .Setup(r => r.CheckForDuplicateDriverAsync(driver))
                .ReturnsAsync(false);

            // Act
            Func<Task> act = async () => await driverService.AddAsync(driver);

            // Assert
            await act.Should().NotThrowAsync<ApplicationException>();
            mockRepository.Verify(r => r.CheckForDuplicateDriverAsync(driver), Times.Once);
            mockRepository.Verify(r => r.AddAsync(driver), Times.Once);
        }

        [Fact]
        public async Task AddAsync_WhenDuplicateDriver_ShouldThrowException()
        {
            // Arrange
            var mockRepository = new Mock<IDriverRepository>();
            var driverService = new DriverService(mockRepository.Object);

            var driver = new Driver { Name = "John Doe", Team = "Team A" };

            mockRepository
                .Setup(r => r.CheckForDuplicateDriverAsync(driver))
                .ReturnsAsync(true);

            // Act
            Func<Task> act = async () => await driverService.AddAsync(driver);

            // Assert
            await act.Should().ThrowAsync<ApplicationException>()
                .WithMessage("Duplicate driver found. Please provide a unique name or team name.");
            mockRepository.Verify(r => r.CheckForDuplicateDriverAsync(driver), Times.Once);
            mockRepository.Verify(r => r.AddAsync(driver), Times.Never);
        }

        [Fact]
        public async Task SearchByNameAsync_WhenDriversExist_ShouldReturnMatchingDrivers()
        {
            // Arrange
            var searchName = "John";
            var expectedDrivers = new List<Driver>
            {
                new Driver { Id = "1", Name = "John Doe", Team = "Team A" },
                new Driver { Id = "2", Name = "John Smith", Team = "Team B" },
                new Driver { Id = "3", Name = "Mike Johnson", Team = "Team A" }
            };

            var repositoryMock = new Mock<IDriverRepository>();
            repositoryMock.Setup(r => r.SearchByNameAsync(searchName)).ReturnsAsync(expectedDrivers);

            var driverService = new DriverService(repositoryMock.Object);

            // Act
            var result = await driverService.SearchByNameAsync(searchName);

            // Assert
            result.Should().BeEquivalentTo(expectedDrivers);
        }

        [Fact]
        public async Task SearchByNameAsync_WhenNoMatchingDriversExist_ShouldReturnEmptyList()
        {
            // Arrange
            var searchName = "John";
            var repositoryMock = new Mock<IDriverRepository>();
            repositoryMock.Setup(r => r.SearchByNameAsync(searchName)).ReturnsAsync(new List<Driver>());

            var driverService = new DriverService(repositoryMock.Object);

            // Act
            var result = await driverService.SearchByNameAsync(searchName);

            // Assert
            result.Should().BeEmpty();
        }
    }
}
