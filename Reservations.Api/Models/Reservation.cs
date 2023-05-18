namespace CarPark.Api.Models;

public class Reservation
{
    public DateTime From { get; set; }
    public DateTime To { get; set; }
    public string? Name { get; set; }
}