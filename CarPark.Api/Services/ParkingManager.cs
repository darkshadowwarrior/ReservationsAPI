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
        decimal weekDayPrice = 10.0M;
        decimal weekendPrice = 15.0M;
        decimal summerPriceIncrease = 12.0M;
        decimal winterPriceIncrease = 8.0M;

        decimal pricePerDay = 0.0M;

        if (IsSummer(date))
        {
            pricePerDay += summerPriceIncrease;
        }
        else if (IsWinter(date))
        {
            pricePerDay += winterPriceIncrease;
        }

        if (IsWeekend(date))
        {
            pricePerDay += weekendPrice;
            return pricePerDay;
        }

        pricePerDay += weekDayPrice;

        return pricePerDay;


    }

    private bool IsWeekend(DateTime date)
    {
        // Check if the given date falls on a weekend (Saturday or Sunday)
        return date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday;
    }

    private bool IsSummer(DateTime date)
    {
        // Define your summer season criteria here
        // For example, assuming summer season is from June 1 to August 31
        return date.Month >= 6 && date.Month <= 8;
    }

    private bool IsWinter(DateTime date)
    {
        return date.Month == 12 || date.Month <= 2;
    }
}

public interface IParkingManager
{
    bool IsParkingAvailable(DateTime from, DateTime to);
    decimal GetParkingPriceForDateRange(DateTime from, DateTime to);
}