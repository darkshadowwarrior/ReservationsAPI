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

            var manager = new ParkingManager();

            var spacesAvailable = manager.IsParkingAvailable(from, to);

            Assert.False(spacesAvailable);
        }

        [Fact]
        public void GivenAWeekDayDateRange_GetParkingPriceForDateRange_ReturnsPriceForParking()
        {
            var from = new DateTime(2023, 1, 1);
            var to = new DateTime(2023, 1, 7);

            var manager = new ParkingManager();

            var actualPrice = manager.GetParkingPriceForDateRange(from, to);

            decimal expectedPrice = 80.0M;
            Assert.Equal(expectedPrice, actualPrice);
        }

        [Fact]
        public void GivenADateRangeOverAWeekend_GetParkingPriceForDateRange_ReturnsPriceForParking()
        {
            var from = new DateTime(2023, 1, 1);
            var to = new DateTime(2023, 1, 10);

            var manager = new ParkingManager();

            var actualPrice = manager.GetParkingPriceForDateRange(from, to);

            decimal expectedPrice = 115.0M;
            Assert.Equal(expectedPrice, actualPrice);
        }
    }
}