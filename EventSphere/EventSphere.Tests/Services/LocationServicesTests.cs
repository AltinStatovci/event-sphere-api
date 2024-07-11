using Moq;
using Xunit;
using EventSphere.Business.Services.Interfaces;
using EventSphere.Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventSphere.Tests.Services
{
    public class LocationServicesTests
    {
        private readonly Mock<ILocationService> _mockLocationService;

        public LocationServicesTests()
        {
            _mockLocationService = new Mock<ILocationService>();

            _mockLocationService.Setup(s => s.AddLocation(It.IsAny<Location>())).ReturnsAsync((Location loc) => loc);
            _mockLocationService.Setup(s => s.DeleteLocation(It.IsAny<int>())).Returns(Task.CompletedTask);
            _mockLocationService.Setup(s => s.GetAllLocations()).ReturnsAsync(It.IsAny<IEnumerable<Location>>());
            _mockLocationService.Setup(s => s.GetLocationsByCityAsync(It.IsAny<string>())).ReturnsAsync(It.IsAny<IEnumerable<Location>>());
            _mockLocationService.Setup(s => s.GetLocationsByCountryAsync(It.IsAny<string>())).ReturnsAsync(It.IsAny<IEnumerable<Location>>());
            _mockLocationService.Setup(s => s.GetLocationById(It.IsAny<int>())).ReturnsAsync((Location loc) => loc);
            _mockLocationService.Setup(s => s.UpdateLocation(It.IsAny<Location>()));
        }

        [Fact]
        public async Task AddLocation_ShouldReturnLocation()
        {
            // Arrange
            var location = new Location { Id = 3, City = "New City", Country = "New Country" };

            // Act
            var result = await _mockLocationService.Object.AddLocation(location);

            // Assert
            Assert.Equal(location, result);
        }

        [Fact]
        public async Task DeleteLocation_ShouldCallDeleteLocationAsyncWithCorrectId()
        {
            // Arrange
            int locationIdToDelete = 1;

            // Act
            await _mockLocationService.Object.DeleteLocation(locationIdToDelete);

            // Assert 
            _mockLocationService.Verify(s => s.DeleteLocation(locationIdToDelete), Times.Once);
        }

        [Fact]
        public async Task GetAllLocations_ShouldReturnAllLocations()
        {
            // Arrange
            var expectedLocations = new List<Location>
            {
                new Location { Id = 1, City = "Location1", Country = "Location11" },
                new Location { Id = 2, City = "Location2", Country = "Location22" }
            };

            _mockLocationService.Setup(s => s.GetAllLocations()).ReturnsAsync(expectedLocations);

            // Act
            var result = await _mockLocationService.Object.GetAllLocations();

            // Assert
            Assert.Equal(expectedLocations, result);
            _mockLocationService.Verify(s => s.GetAllLocations(), Times.Once);
        }

        [Theory]
        [InlineData("Location1")]
        [InlineData("Location2")]
        [InlineData("Location3")]
        public async Task GetLocationsByCity_ShouldReturnLocationsByCity(string city)
        {
            // Arrange
            var allLocations = new List<Location>
            {
                new Location { Id = 1, City = "Location1", Country = "Location11" },
                new Location { Id = 2, City = "Location1", Country = "Location22" },
                new Location { Id = 3, City = "Location2", Country = "Location22" }
            };
            var expectedLocations = allLocations.Where(l => l.City == city).ToList();

            _mockLocationService.Setup(s => s.GetLocationsByCityAsync(city))
                .ReturnsAsync(expectedLocations);

            // Act
            var result = await _mockLocationService.Object.GetLocationsByCityAsync(city);

            // Assert
            Assert.Equal(expectedLocations, result);
            _mockLocationService.Verify(s => s.GetLocationsByCityAsync(city), Times.Once);
        }

        [Theory]
        [InlineData("Location11")]
        [InlineData("Location22")]
        [InlineData("Location33")]
        public async Task GetLocationsByCountry_ShouldReturnLocationsByCountry(string country)
        {
            // Arrange
            var allLocations = new List<Location>
            {
                new Location { Id = 1, City = "Location1", Country = "Location11" },
                new Location { Id = 2, City = "Location1", Country = "Location22" },
                new Location { Id = 3, City = "Location2", Country = "Location22" }
            };
            var expectedLocations = allLocations.Where(l => l.Country == country).ToList();

            _mockLocationService.Setup(s => s.GetLocationsByCountryAsync(country))
                .ReturnsAsync(expectedLocations);

            // Act
            var result = await _mockLocationService.Object.GetLocationsByCountryAsync(country);

            // Assert
            Assert.Equal(expectedLocations, result);
            _mockLocationService.Verify(s => s.GetLocationsByCountryAsync(country), Times.Once);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async Task GetLocationById_ShouldReturnLocationsById(int id)
        {
            //Arrange
            var allLocations = new List<Location>
            {
                new Location { Id = 1, City = "Location1", Country = "Location11" },
                new Location { Id = 2, City = "Location1", Country = "Location22" },
                new Location { Id = 3, City = "Location2", Country = "Location22" }
            };
            var expectedLocation = allLocations.First(l => l.Id == id);

            _mockLocationService.Setup(s => s.GetLocationById(id))
                .ReturnsAsync(expectedLocation);
            //Act
            var result = await _mockLocationService.Object.GetLocationById(id);

            //Assert
            Assert.Equal(expectedLocation, result);
            _mockLocationService.Verify(s => s.GetLocationById(id), Times.Once);
        }

        [Theory]
        [InlineData(1, "Location1", "Country1")]
        [InlineData(2, "Location2", "Country2")]
        [InlineData(3, "Location3", "Country3")]
        public async Task UpdateLocation_ShouldUpdateLocation(int id, string city, string country)
        {
            // Arrange
            var locationToUpdate = new Location { Id = id, City = city, Country = country };

            _mockLocationService.Setup(s => s.UpdateLocation(It.IsAny<Location>())).Returns(Task.CompletedTask);

            // Act
            await _mockLocationService.Object.UpdateLocation(locationToUpdate);

            // Assert
            _mockLocationService.Verify(s => s.UpdateLocation(locationToUpdate), Times.Once);
        }

    }
}
