using CarPark.Api.Services;

namespace CarPark.Api.Models;

public class ParkingAvailabilityResponse
{
    public DateTime From { get; set; }
    public DateTime To { get; set; }
    public List<ParkingSpace> Spaces { get; set; }
}