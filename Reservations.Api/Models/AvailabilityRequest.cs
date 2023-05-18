namespace Reservations.Api.Models;

public class AvailabilityRequest
{
    public DateTime From { get; set; }
    public DateTime To { get; set; }
}