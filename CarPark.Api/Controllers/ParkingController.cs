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

        [HttpGet]
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
    }
}