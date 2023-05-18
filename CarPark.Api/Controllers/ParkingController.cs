using CarPark.Api.Models;
using CarPark.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace CarPark.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ParkingController : ControllerBase
    {
        private readonly IParkingManager _manager;

        public ParkingController(IParkingManager manager)
        {
            _manager = manager;
        }

        [HttpGet("IsParkingAvailable")]
        public async Task<ParkingAvailabilityResponse> IsParkingAvailable([FromQuery] ParkingAvailabilityRequest request)
        {
            var result = _manager.IsParkingAvailable(request.From, request.To);

            return await Task.FromResult(new ParkingAvailabilityResponse()
            {
                From = request.From,
                To = request.To,
                IsSpaceAvailable = result
            });
        }

        [HttpGet("GetParkingPriceForDateRange")]
        public async Task<ParkingPriceResponse> GetParkingPriceForDateRange([FromQuery] ParkingPriceRequest request)
        {
            var result = _manager.GetParkingPriceForDateRange(request.From, request.To);

            return await Task.FromResult(new ParkingPriceResponse()
            {
                From = request.From,
                To = request.To,
                Price = result
            });
        }

        [HttpPost("ReserveParking")]
        public async Task<ParkingReservationResponse> ReserveParking([FromQuery] ParkingReservationRequest request)
        {
            _manager.ReserveParking(request.From, request.To, request.Name);

            return await Task.FromResult(new ParkingReservationResponse()
            {
                From = request.From,
                To = request.To,
                Name = request.Name
            });
        }
    }
}