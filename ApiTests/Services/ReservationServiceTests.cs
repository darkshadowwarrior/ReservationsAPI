using Moq;
using Reservations.Api.Managers;
using Reservations.Api.Models;
using Reservations.Api.Services;

namespace ApiTests.Services
{
    public class ReservationServiceTests
    {
        private readonly ReservationService _reservationService;
        private readonly Mock<IReservationManager> _reservationManagerMock;
        public ReservationServiceTests()
        {
            _reservationManagerMock = new Mock<IReservationManager>();
            _reservationService = new ReservationService(_reservationManagerMock.Object);
        }

        [Fact]
        public void GivenAFromAndToDate_GetSpaceAvailabilities_VerifiesGetSpaceAvailabilitiesWasCalled()
        {
            var request = new AvailabilityRequest { From = new DateTime(2023, 1, 1), To = new DateTime(2020, 1, 4) };

            _reservationService.GetSpaceAvailabilities(request);

            _reservationManagerMock.Verify(x => x.GetSpaceAvailabilities(It.IsAny<DateTime>(), It.IsAny<DateTime>()), Times.Once);
        }

        [Fact]
        public void GivenARequest_GetParkingPriceForDateRange_VerifiesGetParkingPriceForDateRangeWasCalled()
        {
            var request = new PriceRequest { From = new DateTime(2023, 1, 1), To = new DateTime(2020, 1, 4) };

            _reservationService.GetParkingPriceForDateRange(request);

            _reservationManagerMock.Verify(x => x.GetParkingPricesForDateRange(It.IsAny<DateTime>(), It.IsAny<DateTime>()), Times.Once);
        }

        [Fact]
        public void GivenARequest_ReserveParking_VerifiesReserveParkingWasCalled()
        {
            var request = new ReservationRequest { From = new DateTime(2023, 1, 1), To = new DateTime(2020, 1, 4) };

            _reservationService.ReserveParking(request);

            _reservationManagerMock.Verify(x => x.ReserveParking(It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public void GivenARequest_ReserveParking_ThrowsException()
        {
            var request = new ReservationRequest { From = new DateTime(2023, 1, 1), To = new DateTime(2020, 1, 4) };
            _reservationManagerMock.Setup(x => x.ReserveParking(It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<string>())).Throws<UnableToReserveSpaceException>(null);

            var response = _reservationService.ReserveParking(request);

            Assert.Equal($"Unable to reserved reservation due to insufficient spaces available for the given date range {request.From} - {request.To}", response.Status);
        }

        [Fact]
        public void GivenARequest_AmendReservation_VerifiesAmendReservationWasCalled()
        {
            var request = new ReservationRequest { From = new DateTime(2023, 1, 1), To = new DateTime(2020, 1, 4) };

            _reservationService.AmendReservation(request);

            _reservationManagerMock.Verify(x => x.AmendReservation(It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public void GivenARequest_AmendReservation_ThrowsException()
        {
            var request = new ReservationRequest { From = new DateTime(2023, 1, 1), To = new DateTime(2020, 1, 4) };
            _reservationManagerMock.Setup(x => x.AmendReservation(It.IsAny<DateTime>(), It.IsAny<DateTime>(),It.IsAny<string>())).Throws<ReservationNotFoundException>(null);
            
            var response = _reservationService.AmendReservation(request);

            Assert.Equal($"Unable to amend reservation due to insufficient spaces available for the given date range {request.From} - {request.To}", response.Status);
        }

        [Fact]
        public void GivenARequest_CancelReservation_VerifiesCancelReservationWasCalled()
        {
            var request = new ReservationCancellationRequest { Name = "" };

            _reservationService.CancelReservation(request);

            _reservationManagerMock.Verify(x => x.CancelReservation(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public void GivenARequestWithNoNameSet_CancelReservation_ThrowsException()
        {
            _reservationManagerMock.Setup(x => x.CancelReservation(null)).Throws<ReservationNotFoundException>(null);
            var request = new ReservationCancellationRequest();

            var response = _reservationService.CancelReservation(request);

            Assert.Equal("Unable to find reservation!", response.Status);
        }
    }
}
