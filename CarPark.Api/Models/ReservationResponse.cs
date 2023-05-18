namespace CarPark.Api.Models;

public class ReservationResponse
{
    public DateTime From { get; set; }
    public DateTime To { get; set; }
    public string? Name { get; set; }
    public string? Status { get; set; }
}