using Drivers.Api.Configurations;
using Drivers.Api.Models;
using Drivers.Api.Repositories;
using Drivers.Api.Services;
using FluentAssertions;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Moq;
using System;
using System.Linq.Expressions;
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

            var mockRepository = new Mock<IDriverRepository>();
            mockRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(expectedDrivers);

            var mockCollection = new Mock<IMongoCollection<Driver>>();

            var driverService = new DriverService(mockRepository.Object, mockCollection.Object);

            // Act
            var result = await driverService.GetAsync();

            // Assert
            result.Should().BeEquivalentTo(expectedDrivers);
        }

        [Fact]
        public async Task GetAsync_WhenNoDriversExist_ShouldReturnEmptyList()
        {
            // Arrange
            var mockRepository = new Mock<IDriverRepository>();
            mockRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<Driver>());

            var mockCollection = new Mock<IMongoCollection<Driver>>();

            var driverService = new DriverService(mockRepository.Object, mockCollection.Object);

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
            var mockCollection = new Mock<IMongoCollection<Driver>>();
            var driverService = new DriverService(mockRepository.Object, mockCollection.Object);


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
            var mockCollection = new Mock<IMongoCollection<Driver>>();
            var driverService = new DriverService(mockRepository.Object, mockCollection.Object);


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
            var mockCollection = new Mock<IMongoCollection<Driver>>();
            var driverService = new DriverService(mockRepository.Object, mockCollection.Object);

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
            var mockCollection = new Mock<IMongoCollection<Driver>>();
            var driverService = new DriverService(mockRepository.Object, mockCollection.Object);


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

            var mockRepository = new Mock<IDriverRepository>();
            var mockCollection = new Mock<IMongoCollection<Driver>>();
            mockRepository.Setup(r => r.SearchByNameAsync(searchName)).ReturnsAsync(expectedDrivers);

            var driverService = new DriverService(mockRepository.Object, mockCollection.Object);

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
            var mockRepository = new Mock<IDriverRepository>();
            var mockCollection = new Mock<IMongoCollection<Driver>>();
            mockRepository.Setup(r => r.SearchByNameAsync(searchName)).ReturnsAsync(new List<Driver>());

            var driverService = new DriverService(mockRepository.Object, mockCollection.Object);

            // Act
            var result = await driverService.SearchByNameAsync(searchName);

            // Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public async Task CheckForDuplicateDriverAsync_WhenDuplicateDriverExists_ShouldReturnTrue()
        {
            // Arrange
            var mockRepository = new Mock<IDriverRepository>();
            var mockCollection = new Mock<IMongoCollection<Driver>>();
            var driverService = new DriverService(mockRepository.Object, mockCollection.Object);

            var driver = new Driver { Name = "John Doe", Team = "Team A" };

            mockRepository
                .Setup(r => r.CheckForDuplicateDriverAsync(driver))
                .ReturnsAsync(true);

            // Act
            var result = await driverService.CheckForDuplicateDriverAsync(driver);

            // Assert
            result.Should().BeTrue();
            mockRepository.Verify(r => r.CheckForDuplicateDriverAsync(driver), Times.Once);
        }

        [Fact]
        public async Task CheckForDuplicateDriverAsync_WhenNoDuplicateDriverExists_ShouldReturnFalse()
        {
            // Arrange
            var mockRepository = new Mock<IDriverRepository>();
            var mockCollection = new Mock<IMongoCollection<Driver>>();
            var driverService = new DriverService(mockRepository.Object, mockCollection.Object);

            var driver = new Driver { Name = "John Doe", Team = "Team A" };

            // Set up the mock repository method to return false
            mockRepository
                .Setup(r => r.CheckForDuplicateDriverAsync(driver))
                .ReturnsAsync(false);

            // Act
            var result = await driverService.CheckForDuplicateDriverAsync(driver);

            // Assert
            result.Should().BeFalse();
            mockRepository.Verify(r => r.CheckForDuplicateDriverAsync(driver), Times.Once);
        }

        [Fact]
        public async Task RemoveAsync_WhenDriverExists_ShouldReturnTrue()
        {
            // Arrange
            var mockRepository = new Mock<IDriverRepository>();
            var mockCollection = new Mock<IMongoCollection<Driver>>();
            var driverService = new DriverService(mockRepository.Object, mockCollection.Object);

            var driverId = "123";

            mockRepository
                .Setup(r => r.GetByIdAsync(driverId))
                .ReturnsAsync(new Driver { Id = driverId });

            mockRepository
                .Setup(r => r.RemoveAsync(driverId))
                .ReturnsAsync(true);

            // Act
            var result = await driverService.RemoveAsync(driverId);

            // Assert
            result.Should().BeTrue();
            mockRepository.Verify(r => r.RemoveAsync(driverId), Times.Once);
        }

        [Fact]
        public async Task RemoveAsync_WhenDriverDoesNotExist_ShouldReturnFalse()
        {
            // Arrange
            var nonExistingDriverId = "non-existing-driver-id";

            var mockRepository = new Mock<IDriverRepository>();
            var mockCollection = new Mock<IMongoCollection<Driver>>();

            mockRepository.Setup(repo => repo.RemoveAsync(nonExistingDriverId))
                .ReturnsAsync(false);

            var driverService = new DriverService(mockRepository.Object, mockCollection.Object);

            // Act
            var result = await driverService.RemoveAsync(nonExistingDriverId);

            // Assert
            result.Should().BeFalse();
        }


        [Fact]
        public async Task UpdateDriverAsync_Should_Update_Driver_And_Return_True()
        {
            // Arrange
            var existingDriverId = "existing-driver-id";
            var driverToUpdate = new Driver
            {
                Id = existingDriverId,
                Name = "John Doe",
                Number = 42,
                Team = "Team A"
            };

            var existingDriver = new Driver
            {
                Id = existingDriverId,
                Name = "Jane Smith",
                Number = 27,
                Team = "Team B"
            };

            var mockRepository = new Mock<IDriverRepository>();
            var mockCollection = new Mock<IMongoCollection<Driver>>();

            mockRepository.Setup(repo => repo.GetByIdAsync(existingDriverId))
                .ReturnsAsync(existingDriver);
            mockRepository.Setup(repo => repo.CheckForDuplicateDriverAsync(driverToUpdate))
                .ReturnsAsync(false);

            var driverService = new DriverService(mockRepository.Object, mockCollection.Object);

            // Act
            var result = await driverService.UpdateDriverAsync(driverToUpdate);

            // Assert
            result.Should().BeTrue();
            existingDriver.Name.Should().Be(driverToUpdate.Name);
            existingDriver.Number.Should().Be(driverToUpdate.Number);
            existingDriver.Team.Should().Be(driverToUpdate.Team);
            mockRepository.Verify(repo => repo.UpdateDriverAsync(existingDriver), Times.Once);
        }

        [Fact]
        public async Task UpdateDriverAsync_Should_Return_False_If_Driver_Not_Found()
        {
            // Arrange
            var nonExistingDriverId = "non-existing-driver-id";
            var driverToUpdate = new Driver
            {
                Id = nonExistingDriverId,
                Name = "John Doe",
                Number = 42,
                Team = "Team A"
            };

            var mockRepository = new Mock<IDriverRepository>();
            var mockCollection = new Mock<IMongoCollection<Driver>>();
            mockRepository.Setup(repo => repo.GetByIdAsync(nonExistingDriverId))
                .ReturnsAsync((Driver)null);

            var driverService = new DriverService(mockRepository.Object, mockCollection.Object);

            // Act
            var result = await driverService.UpdateDriverAsync(driverToUpdate);

            // Assert
            result.Should().BeFalse();
            mockRepository.Verify(repo => repo.UpdateDriverAsync(It.IsAny<Driver>()), Times.Never);
        }

    }
}
