using CarPark.Api.Repositories;
using CarPark.Api.Services;
using Moq;
using NuGet.Frameworks;

namespace ApiServiceTests.Services
{
    public class ParkingManagerTests
    {
        private readonly ParkingManager _manager;
        private readonly Mock<IParkingSpaceRepository> _repositoryMock;
        public ParkingManagerTests()
        {
            _repositoryMock = new Mock<IParkingSpaceRepository>();
            _manager = new ParkingManager(_repositoryMock.Object);
        }

        [Fact]
        public void GivenADateRange_ReturnsFalseIfSpacesAvailable()
        {
            _repositoryMock.Setup(x => x.IsSpaceAvailable(It.IsAny<DateTime>())).Returns(false);

            var from = new DateTime(2023, 1, 1);
            var to = new DateTime(2023, 1, 10);

            var spacesAvailable = _manager.IsParkingAvailable(from, to);

            Assert.False(spacesAvailable);
        }

        [Fact]
        public void GivenADateRange_ReturnsTrueIfSpacesAvailable()
        {
            _repositoryMock.Setup(x => x.IsSpaceAvailable(It.IsAny<DateTime>())).Returns(true);

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
            _repositoryMock.Setup(x => x.IsSpaceAvailable(It.IsAny<DateTime>())).Returns(false);

            var from = new DateTime(2023, 1, 6);
            var to = new DateTime(2023, 1, 9);

            Assert.Throws<UnableToReserveSpaceException>(() => _manager.ReserveParking(from, to, "Bill Gates" ));
        }

        [Fact]
        public void GivenANewDateRange_AmendReservation_UpdatesReservationWithNewDates()
        {
            _repositoryMock.Setup(x => x.IsSpaceAvailable(It.IsAny<DateTime>())).Returns(true);

            var from = new DateTime(2023, 1, 1);
            var to = new DateTime(2023, 1, 4);

            _manager.ReserveParking(from, to, "Bill Gates");

            var amendedFrom = new DateTime(2023, 1, 1);
            var amendedTo = new DateTime(2023, 1, 2);

            _manager.AmendReservation(amendedFrom, amendedTo, "Bill Gates");

            var reservations = _manager.GetReservations();

            Assert.True(reservations.ContainsKey("Bill Gates"));
            Assert.Equal(reservations["Bill Gates"].From, new DateTime(2023, 1, 1)); 
            Assert.Equal(reservations["Bill Gates"].To, new DateTime(2023, 1, 2));
        }

        [Fact]
        public void GivenAName_CancelParking_CancelParkingForCustomer()
        {
            _manager.CancelParking("Bill Gates");

            var reservations = _manager.GetReservations();

            Assert.False(reservations.ContainsKey("Bill Gates"));
        }

        [Fact]
        public void GivenADateRange_GetAvailableParking_ReturnsAllAvailableParkingSpaces()
        {
            _repositoryMock.Setup(x => x.GetTotalParkingSpacesAvailableByDate(It.IsAny<DateTime>())).Returns(9);

            var from = new DateTime(2023, 1, 1);
            var to = new DateTime(2023, 1, 4);

            var availableSpaces = _manager.GetAvailableParking(from, to);

            Assert.Equal(9, availableSpaces[0].SpacesAvailable);
        }
    }
}