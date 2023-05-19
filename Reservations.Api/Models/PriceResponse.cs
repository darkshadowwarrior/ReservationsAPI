namespace Reservations.Api.Models;

public class PriceResponse
{
    public DateTime From { get; set; }
    public DateTime To { get; set; }
    public List<Space> Prices { get; set; }
}

public class Space
{
    public DateTime Date { get; set; }
    public decimal Price { get; set; }
}