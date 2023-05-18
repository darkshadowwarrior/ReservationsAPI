using CarPark.Api.Services;

namespace ApiServiceTests.Services
{
    public class ParkingManagerTests
    {
        [Fact]
        public void GivenADateRange_ReturnsTrueIfSpacesAvailable()
        {
            var from = new DateTime(2023, 1, 1);
            var to = new DateTime(2023, 1, 10);

            var service = new ParkingManager();

            var spacesAvailable = service.IsParkingAvailable(from, to);

            Assert.False(spacesAvailable);
        }
    }
}