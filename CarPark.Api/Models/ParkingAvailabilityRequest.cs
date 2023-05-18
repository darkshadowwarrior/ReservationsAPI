namespace CarPark.Api.Models;

public class ParkingAvailabilityRequest
{
    public DateTime From { get; set; }
    public DateTime To { get; set; }
}