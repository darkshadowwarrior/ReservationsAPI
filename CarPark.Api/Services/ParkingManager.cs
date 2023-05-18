using CarPark.Api.Models;

namespace CarPark.Api.Services;


public interface IParkingManager
{
    bool IsParkingAvailable(DateTime from, DateTime to);
    decimal GetParkingPriceForDateRange(DateTime from, DateTime to);
    void ReserveParking(DateTime from, DateTime to, string? name);
    Dictionary<string, ParkingReservation> GetReservations();
    void CancelParking(string? name);
}

public class ParkingManager : IParkingManager
{
    private readonly Dictionary<DateTime, int> _parkingSpaceAllocations;
    private readonly Dictionary<string, ParkingReservation> _parkingReservations;

    public ParkingManager()
    {
        _parkingSpaceAllocations = new Dictionary<DateTime, int>()
        {
            { new DateTime(2023, 1, 1), 1 },
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

        _parkingReservations = new Dictionary<string, ParkingReservation>()
        {
            {
                "Ian Richards ",
                new ParkingReservation()
                    { Name = "Ian Richards", From = new DateTime(2023, 1, 1), To = new DateTime(2023, 1, 6) }
            }
        };
    }

    public Dictionary<string, ParkingReservation> GetReservations()
    {
        return _parkingReservations;
    }

    public void ReserveParking(DateTime from, DateTime to, string? name)
    {
        if (name != null && IsParkingAvailable(from, to))
        {
            for (DateTime date = from; date <= to; date = date.AddDays(1))
            {
                _parkingSpaceAllocations.TryAdd(date, 0);
                _parkingSpaceAllocations[date] += 1;
            }

            _parkingReservations.Add(name, new ParkingReservation() { From = from, To = to, Name = name });
        }
        else
        {
            throw new UnableToReserveSpaceException(
                "UnableToReserveSpaceException: ParkingManager threw an exception when trying to reserves space for given date range");
        }
    }


    public void AmendReservation(DateTime from, DateTime to, string? name)
    {
        if (IsParkingAvailable(from, to))
        {
            CancelParking(name);
            ReserveParking(from, to, name);
        }
    }

    public void CancelParking(string? name)
    {
        if (name != null && _parkingReservations.TryGetValue(name, out var reservation))
        {
            for (DateTime date = reservation.From; date <= reservation.To; date = date.AddDays(1))
            {
                _parkingSpaceAllocations[date] -= 1;
            }

            _parkingReservations.Remove(name);
        }
    }

    public bool IsParkingAvailable(DateTime from, DateTime to)
    {
        for (DateTime date = from; date <= to; date = date.AddDays(1))
        {
            if (_parkingSpaceAllocations.TryGetValue(date, out var reservation))
            {
                if (reservation >= 10)
                {
                    return false;
                }
            }
        }

        return true;
    }

    public decimal GetParkingPriceForDateRange(DateTime from, DateTime to)
    {
        decimal totalCost = 0;

        for (DateTime date = from; date <= to; date = date.AddDays(1))
        {
            var dailyPrice = CalculateDailyPrice(date);
            totalCost += dailyPrice;
        }

        return totalCost;
    }

    private decimal CalculateDailyPrice(DateTime date)
    {
        var weekDayPrice = 10.0M;
        var weekendPrice = 15.0M;
        var summerPriceIncrease = 12.0M;
        var winterPriceIncrease = 8.0M;

        var pricePerDay = 0.0M;

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
        return date.DayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday;
    }

    private bool IsSummer(DateTime date)
    {
        return date.Month is >= 6 and <= 8;
    }

    private bool IsWinter(DateTime date)
    {
        return date.Month is 12 or <= 2;
    }
}

public class UnableToReserveSpaceException : Exception
{
    public UnableToReserveSpaceException(string message) : base(message) { }
}