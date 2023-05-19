using Moq;
using Reservations.Api.Controllers;
using Reservations.Api.Models;
using Reservations.Api.Services;

namespace ApiTests.Controllers
{
    public class ReservationControllerTests
    {
        private ReservationController _controller;
        private readonly Mock<IReservationService> _reservationService;

        public ReservationControllerTests()
        {
            _reservationService = new Mock<IReservationService>();

            _controller = new ReservationController(_reservationService.Object);
        }

        [Fact]
        public void GivenARequest_GetSpaceAvailabilitiesForDateRange_VerifiesGetSpaceAvailabilitiesWasCalled()
        {
            var request = new AvailabilityRequest { From = new DateTime(2023, 1, 1), To = new DateTime(2020, 1, 4) };

            _controller.GetSpaceAvailabilitiesForDateRange(request);

            _reservationService.Verify(x => x.GetSpaceAvailabilities(It.IsAny<AvailabilityRequest>()), Times.Once);
        }

        [Fact]
        public void GivenARequest_GetParkingPriceForDateRange_VerifiesGetParkingPriceForDateRangeWasCalled()
        {
            var request = new PriceRequest { From = new DateTime(2023, 1, 1), To = new DateTime(2020, 1, 4) };

            _controller.GetParkingPriceForDateRange(request);

            _reservationService.Verify(x => x.GetParkingPriceForDateRange(It.IsAny<PriceRequest>()), Times.Once);
        }

        [Fact]
        public void GivenARequest_ReserveParking_VerifiesReserveParkingWasCalled()
        {
            var request = new ReservationRequest { From = new DateTime(2023, 1, 1), To = new DateTime(2020, 1, 4) };

            _controller.ReserveParking(request);

            _reservationService.Verify(x => x.ReserveParking(It.IsAny<ReservationRequest>()), Times.Once);
        }

        [Fact]
        public void GivenARequest_AmendReservation_VerifiesAmendReservationWasCalled()
        {
            var request = new ReservationRequest { From = new DateTime(2023, 1, 1), To = new DateTime(2020, 1, 4) };

            _controller.AmendReservation(request);

            _reservationService.Verify(x => x.AmendReservation(It.IsAny<ReservationRequest>()), Times.Once);
        }

        [Fact]
        public void GivenARequest_CancelReservation_VerifiesCancelReservationWasCalled()
        {
            var request = new ReservationCancellationRequest { Name = "" };

            _controller.CancelReservation(request);

            _reservationService.Verify(x => x.CancelReservation(It.IsAny<ReservationCancellationRequest>()), Times.Once);
        }
    }
}
