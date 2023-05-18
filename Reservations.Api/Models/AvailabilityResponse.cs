namespace Reservations.Api.Models;

public class AvailabilityResponse
{
    public DateTime From { get; set; }
    public DateTime To { get; set; }
    public List<SpaceAvailability>? Spaces { get; set; }
}