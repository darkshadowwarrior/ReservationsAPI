using Microsoft.AspNetCore.Mvc;
using Reservations.Api.Models;
using Reservations.Api.Services;

namespace Reservations.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ReservationController : ControllerBase
    {
        private readonly IReservationService _service;

        public ReservationController(IReservationService service)
        {
            _service = service;
        }

        [HttpGet("GetSpaceAvailabilitiesForDateRange")]
        public AvailabilityResponse GetSpaceAvailabilitiesForDateRange([FromQuery] AvailabilityRequest request)
        {
            return _service.GetSpaceAvailabilities(request);
        }

        [HttpGet("GetParkingPriceForDateRange")]
        public PriceResponse GetParkingPriceForDateRange([FromQuery] PriceRequest request)
        {
            return _service.GetParkingPriceForDateRange(request);
        }

        [HttpPost("ReserveParking")]
        public ReservationResponse ReserveParking([FromQuery] ReservationRequest request)
        {
            return _service.ReserveParking(request);
        }

        [HttpPost("AmendReservation")]
        public ReservationResponse AmendReservation([FromQuery] ReservationRequest request)
        {
            return _service.AmendReservation(request);
        }

        [HttpPost("CancelReservation")]
        public ReservationCancellationResponse CancelReservation([FromQuery] ReservationCancellationRequest request)
        {
            return _service.CancelReservation(request);
        }
    }
}