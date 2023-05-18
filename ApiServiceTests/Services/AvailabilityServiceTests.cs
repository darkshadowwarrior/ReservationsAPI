using CarPark.Api.Models;

namespace ApiServiceTests.Services
{
    public class AvailabilityServiceTests
    {
        [Fact]
        public void GivenADateRange_ReturnsAvailableParkingSpaces()
        {
            var from = new DateTime(2023, 1, 1);
            var to = new DateTime(2023, 12, 25);
            var expectedSpaces = new List<ParkingSpace>();

            var service = new AvailabilityService();

            var spacesAvailable = service.GetAvailableParkingSpacesByDateRange(from, to);

            Assert.Equal(spacesAvailable, expectedSpaces);
        }
    }
}
