namespace CarPark.Api.Models;

public class ParkingPriceResponse
{
    public DateTime From { get; set; }
    public DateTime To { get; set; }
    public decimal Price { get; set; }
}