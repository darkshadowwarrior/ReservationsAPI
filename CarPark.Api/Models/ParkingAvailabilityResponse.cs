namespace CarPark.Api.Models;

public class ParkingAvailabilityResponse
{
    public DateTime From { get; set; }
    public DateTime To { get; set; }
    public bool IsSpaceAvailable { get; set; }
}