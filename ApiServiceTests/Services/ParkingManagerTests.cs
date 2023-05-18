using CarPark.Api.Services;

namespace ApiServiceTests.Services
{
    public class ParkingManagerTests
    {
        private readonly ParkingManager _manager;
        public ParkingManagerTests()
        {
            _manager = new ParkingManager();
        }

        [Fact]
        public void GivenADateRange_ReturnsFalseIfSpacesAvailable()
        {
            var from = new DateTime(2023, 1, 1);
            var to = new DateTime(2023, 1, 10);

            var spacesAvailable = _manager.IsParkingAvailable(from, to);

            Assert.False(spacesAvailable);
        }

        [Fact]
        public void GivenADateRange_ReturnsTrueIfSpacesAvailable()
        {
            var from = new DateTime(2023, 1, 1);
            var to = new DateTime(2023, 1, 3);

            var spacesAvailable = _manager.IsParkingAvailable(from, to);

            Assert.True(spacesAvailable);
        }

        [Fact]
        public void GivenAWeekDayDateRange_GetParkingPriceForDateRange_ReturnsPriceForParking()
        {
            var from = new DateTime(2023, 7, 1);
            var to = new DateTime(2023, 7, 7);

            var actualPrice = _manager.GetParkingPriceForDateRange(from, to);

            decimal expectedPrice = 164.0M;
            Assert.Equal(expectedPrice, actualPrice);
        }

        [Fact]
        public void GivenADateRangeOverAWeekend_GetParkingPriceForDateRange_ReturnsPriceForParking()
        {
            var from = new DateTime(2023, 7, 1);
            var to = new DateTime(2023, 7, 10);

            var actualPrice = _manager.GetParkingPriceForDateRange(from, to);

            decimal expectedPrice = 240.0M;
            Assert.Equal(expectedPrice, actualPrice);
        }

        [Fact]
        public void GivenAWeekDayDateRangAndInWinter_GetParkingPriceForDateRange_ReturnsPriceForParking()
        {
            var from = new DateTime(2023, 1, 1);
            var to = new DateTime(2023, 1, 7);

            var actualPrice = _manager.GetParkingPriceForDateRange(from, to);

            decimal expectedPrice = 136.0M;
            Assert.Equal(expectedPrice, actualPrice);
        }

        [Fact]
        public void GivenADateRangeOverAWeekendAndInWinter_GetParkingPriceForDateRange_ReturnsPriceForParking()
        {
            var from = new DateTime(2023, 2, 1);
            var to = new DateTime(2023, 2, 10);

            var actualPrice = _manager.GetParkingPriceForDateRange(from, to);

            decimal expectedPrice = 190.0M;
            Assert.Equal(expectedPrice, actualPrice);
        }

        [Fact]
        public void GivenAWeekDayDateRangNotInWinterOrSummer_GetParkingPriceForDateRange_ReturnsPriceForParking()
        {
            var from = new DateTime(2023, 10, 1);
            var to = new DateTime(2023, 10, 7);

            var actualPrice = _manager.GetParkingPriceForDateRange(from, to);

            decimal expectedPrice = 80.0M;
            Assert.Equal(expectedPrice, actualPrice);
        }

        [Fact]
        public void GivenADateRangeOverAWeekendNotInWinterOrSummer_GetParkingPriceForDateRange_ReturnsPriceForParking()
        {
            var from = new DateTime(2023, 4, 1);
            var to = new DateTime(2023, 4, 10);

            var actualPrice = _manager.GetParkingPriceForDateRange(from, to);

            decimal expectedPrice = 120.0M;
            Assert.Equal(expectedPrice, actualPrice);
        }

        [Fact]
        public void GivenADateRange_ReserveParking_ThrowsException()
        {
            var from = new DateTime(2023, 1, 6);
            var to = new DateTime(2023, 1, 9);

            Assert.Throws<UnableToReserveSpaceException>(() => _manager.ReserveParking(from, to, "Bill Gates" ));
        }
    }
}