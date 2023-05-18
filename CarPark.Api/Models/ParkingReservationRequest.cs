namespace CarPark.Api.Models;

public class ParkingReservationRequest
{
    public DateTime From { get; set; }
    public DateTime To { get; set; }
    public string? Name { get; set; }
}