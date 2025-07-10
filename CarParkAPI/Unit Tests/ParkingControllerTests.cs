using Xunit;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CarParkAPI.Controllers;
using CarParkAPI.Models;
using CarParkAPI.Services;

namespace CarParkAPI.Unit_Tests
{
    public class ParkingControllerTests
    {
        [Fact]
        public async Task GetParkingSpaces_ReturnsListOfParkingSpaces()
        {
            // Arrange
            var mockService = new Mock<IParkingService>();
            var mockCostService = new Mock<ICostService>();

            var sampleData = new List<ParkingSpace>
            {
                new ParkingSpace { Id = 1 },
                new ParkingSpace { Id = 2}
            };

            mockService.Setup(s => s.GetAllAsync())
                       .ReturnsAsync(new ActionResult<IEnumerable<ParkingSpace>>(sampleData));

            var controller = new ParkingController(mockService.Object, mockCostService.Object);

            // Act
            var result = await controller.GetParkingSpaces();

            // Assert
            var okResult = Assert.IsType<ActionResult<IEnumerable<ParkingSpace>>>(result);
            var returnValue = Assert.IsAssignableFrom<IEnumerable<ParkingSpace>>(okResult.Value);

            Assert.NotNull(returnValue);
            Assert.Collection(returnValue,
                item => Assert.Equal(1, item.Id),
                item => Assert.Equal(2, item.Id)
            );
        }
    }
}
