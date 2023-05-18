namespace CarPark.Api.Services;

public class ParkingManager : IParkingManager
{
    private readonly Dictionary<DateTime, int> _reservations;

    public ParkingManager()
    {
        _reservations = new Dictionary<DateTime, int>()
        {
            { new DateTime(2023, 1, 1), 7 },
            { new DateTime(2023, 1, 2), 2 },
            { new DateTime(2023, 1, 3), 0 },
            { new DateTime(2023, 1, 4), 1 },
            { new DateTime(2023, 1, 5), 3 },
            { new DateTime(2023, 1, 6), 10 },
            { new DateTime(2023, 1, 7), 10 },
            { new DateTime(2023, 1, 8), 10 },
            { new DateTime(2023, 1, 9), 10 },
            { new DateTime(2023, 1, 10), 9 }
        };
    }

    public bool IsParkingAvailable(DateTime from, DateTime to)
    {
        // Check if any of the dates within the range are already reserved
        for (DateTime date = from; date <= to; date = date.AddDays(1))
        {
            if (_reservations.TryGetValue(date, out var reservation))
            {
                // A reservation exists for this date, check if all spaces are occupied
                if (reservation >= 10)
                {
                    return false;  // All spaces are occupied
                }
            }
        }

        return true;  // Parking is available for all dates in the range
    }

    public decimal GetParkingPriceForDateRange(DateTime from, DateTime to)
    {
        decimal totalCost = 0;

        // Calculate the parking price for each date within the range
        for (DateTime date = from; date <= to; date = date.AddDays(1))
        {
            decimal dailyPrice = CalculateDailyPrice(date);
            totalCost += dailyPrice;
        }

        return totalCost;
    }

    private decimal CalculateDailyPrice(DateTime date)
    {
        return 10.0M;
    }
}

public interface IParkingManager
{
    bool IsParkingAvailable(DateTime from, DateTime to);
    decimal GetParkingPriceForDateRange(DateTime from, DateTime to);
}