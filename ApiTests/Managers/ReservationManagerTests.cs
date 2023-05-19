using Moq;
using Reservations.Api.Managers;
using Reservations.Api.Models;
using Reservations.Api.Repositories;

namespace ApiTests.Services
{
    public class ReservationManagerTests
    {
        private readonly ReservationManager _manager;
        private readonly Mock<IParkingSpaceManager> _parkingSpaceManagerMock;
        private readonly Mock<IReservationsRepository> _parkingReservationsRepository;
        private readonly Mock<IPricingManager> _pricingManagerMock;
        
        public ReservationManagerTests()
        {
            _parkingSpaceManagerMock = new Mock<IParkingSpaceManager>();
            _parkingReservationsRepository = new Mock<IReservationsRepository>();
            _pricingManagerMock = new Mock<IPricingManager>();
            _manager = new ReservationManager(_parkingSpaceManagerMock.Object, _parkingReservationsRepository.Object, _pricingManagerMock.Object);
        }

        [Fact]
        public void GivenADateRange_ReturnsFalseIfSpacesNotAvailable()
        {
            _parkingSpaceManagerMock.Setup(x => x.IsSpaceAvailable(It.IsAny<DateTime>())).Returns(false);

            var from = new DateTime(2023, 1, 1);
            var to = new DateTime(2023, 1, 10);

            var spacesAvailable = _manager.IsParkingAvailable(from, to);

            Assert.False(spacesAvailable);
        }

        [Fact]
        public void GivenADateRange_ReturnsTrueIfSpacesAvailable()
        {
            _parkingSpaceManagerMock.Setup(x => x.IsSpaceAvailable(It.IsAny<DateTime>())).Returns(true);

            var from = new DateTime(2023, 1, 1);
            var to = new DateTime(2023, 1, 3);

            var spacesAvailable = _manager.IsParkingAvailable(from, to);

            Assert.True(spacesAvailable);
        }

        

        [Fact]
        public void GivenADateRange_ReserveParking_ThrowsException()
        {
            _parkingSpaceManagerMock.Setup(x => x.IsSpaceAvailable(It.IsAny<DateTime>())).Returns(false);

            var from = new DateTime(2023, 1, 6);
            var to = new DateTime(2023, 1, 9);

            Assert.Throws<UnableToReserveSpaceException>(() => _manager.ReserveParking(from, to, "Bill Gates" ));
        }

        [Fact]
        public void GivenNull_CancelReservation_ThrowsException()
        {
            Assert.Throws<ArgumentNullException>(() => _manager.CancelReservation(null));
        }

        [Fact]
        public void GivenAInvalidReservationName_CancelReservation_ThrowsException()
        {
            Assert.Throws<ReservationNotFoundException>(() => _manager.CancelReservation("Bob Johnson"));
        }

        [Fact]
        public void GivenADateRange_ReserveParking_ReservesParkingWithGivenDates()
        {
            var startDate = new DateTime(2023, 1, 1);
            var endDate = new DateTime(2023, 1, 2);

            _parkingSpaceManagerMock.Setup(x => x.IsSpaceAvailable(It.IsAny<DateTime>())).Returns(true);

            _manager.ReserveParking(startDate, endDate, "Bill Gates");

            _parkingSpaceManagerMock.Verify(x => x.ReserveSpace(It.IsAny<DateTime>()), Times.Exactly(2));
            _parkingReservationsRepository.Verify(x => x.AddReservation(It.IsAny<Reservation>()), Times.Once);
        }

        [Fact]
        public void GivenANewDateRange_AmendReservation_UpdatesReservationWithNewDates()
        {
            var amendedStartDate = new DateTime(2023, 1, 1);
            var amendedEndDate = new DateTime(2023, 1, 2);

            _parkingSpaceManagerMock.Setup(x => x.IsSpaceAvailable(It.IsAny<DateTime>())).Returns(true);
            _parkingReservationsRepository.Setup(x => x.ReservationExists(It.IsAny<string>())).Returns(true);
            _parkingReservationsRepository.Setup(x => x.GetReservationByName(It.IsAny<string>())).Returns(new Reservation { From = new DateTime(2023, 1, 1), To = new DateTime(2023, 1, 6) });

            _manager.AmendReservation(amendedStartDate, amendedEndDate, "Bill Gates");

            _parkingSpaceManagerMock.Verify(x => x.DeallocateSpace(It.IsAny<DateTime>()), Times.Exactly(6));
            _parkingSpaceManagerMock.Verify(x => x.ReserveSpace(It.IsAny<DateTime>()), Times.Exactly(2));
            _parkingReservationsRepository.Verify(x => x.RemoveReservation(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public void GivenAName_AndReservationExists_WhenCancelParkingIsCalled_TheReservationIsCancelled()
        {
            var originalReservation = new Reservation { Name = "Bill Gates", From = new DateTime(2023, 1, 1), To = new DateTime(2023, 1, 6) };

            _parkingReservationsRepository.Setup(x => x.ReservationExists(It.IsAny<string>())).Returns(true);
            _parkingReservationsRepository.Setup(x => x.GetReservationByName("Bill Gates")).Returns(originalReservation);

            _manager.CancelReservation("Bill Gates");

            _parkingSpaceManagerMock.Verify(x => x.DeallocateSpace(It.IsAny<DateTime>()), Times.Exactly(6));
            _parkingReservationsRepository.Verify(x => x.RemoveReservation(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public void GivenADateRange_GetAvailableParking_ReturnsAllAvailableParkingSpaces()
        {
            _parkingSpaceManagerMock.Setup(x => x.GetTotalParkingSpacesAvailableByDate(It.IsAny<DateTime>())).Returns(9);

            var from = new DateTime(2023, 1, 1);
            var to = new DateTime(2023, 1, 4);

            _manager.GetAvailableParking(from, to);

            _parkingSpaceManagerMock.Verify(x => x.GetTotalParkingSpacesAvailableByDate(It.IsAny<DateTime>()), Times.Exactly(4));
        }
    }
}