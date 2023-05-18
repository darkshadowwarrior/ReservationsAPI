using CarPark.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace CarPark.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ParkingController : ControllerBase
    {
        public List<ParkingSpace> GetAvailableSlots(DateTime startDate, DateTime endDate)
        {
            return new List<ParkingSpace>();
        }
    }
}