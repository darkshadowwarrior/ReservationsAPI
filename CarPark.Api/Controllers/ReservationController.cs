using CarPark.Api.Models;
using CarPark.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace CarPark.Api.Controllers
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

        [HttpGet("IsParkingAvailable")]
        public async Task<AvailabilityResponse> IsParkingAvailable([FromQuery] AvailabilityRequest request)
        {
            return await Task.FromResult(_service.GetAvailableParking(request));
        }

        [HttpGet("GetParkingPriceForDateRange")]
        public async Task<PriceResponse> GetParkingPriceForDateRange([FromQuery] PriceRequest request)
        {
            return await Task.FromResult(_service.GetParkingPriceForDateRange(request));
        }

        [HttpPost("ReserveParking")]
        public async Task<ReservationResponse> ReserveParking([FromQuery] ReservationRequest request)
        {
            return await Task.FromResult(_service.ReserveParking(request));
        }

        [HttpPost("AmendReservation")]
        public async Task<ReservationResponse> AmendReservation([FromQuery] ReservationRequest request)
        {
            return await Task.FromResult(_service.AmendReservation(request));
        }

        [HttpPost("CancelReservation")]
        public async Task<ReservationCancellationResponse> CancelParking([FromQuery] ReservationCancellationRequest request)
        {
            return await Task.FromResult(_service.CancelReservation(request));
        }
    }
}