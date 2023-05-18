using CarPark.Api.Models;

namespace CarPark.Api.Services;


public interface IParkingManager
{
    bool IsParkingAvailable(DateTime from, DateTime to);
    decimal GetParkingPriceForDateRange(DateTime from, DateTime to);
}

public class ParkingManager : IParkingManager
{
    private readonly Dictionary<DateTime, Allocations> _reservations;

    public ParkingManager()
    {
        _reservations = new Dictionary<DateTime, Allocations>()
        {
            { new DateTime(2023, 1, 1), new Allocations { Customers = new List<Customer>() { new() { Name = "Ian Richards" } }, SpacesAvailable = 9 } },
            { new DateTime(2023, 1, 2), new Allocations { Customers = new List<Customer>() { new() { Name = "Ian Richards" } }, SpacesAvailable = 8 } },
            { new DateTime(2023, 1, 3), new Allocations { Customers = new List<Customer>() { new() { Name = "Ian Richards" } }, SpacesAvailable = 10 } },
            { new DateTime(2023, 1, 4), new Allocations { Customers = new List<Customer>() { new() { Name = "Ian Richards" } }, SpacesAvailable = 9 } },
            { new DateTime(2023, 1, 5), new Allocations { Customers = new List<Customer>() { new() { Name = "Ian Richards" } }, SpacesAvailable = 7 } },
            { new DateTime(2023, 1, 6), new Allocations { Customers = new List<Customer>() { new() { Name = "Ian Richards" } }, SpacesAvailable = 0 } },
            { new DateTime(2023, 1, 7), new Allocations { Customers = new List<Customer>() { new() { Name = "Ian Richards" } }, SpacesAvailable = 0 } },
            { new DateTime(2023, 1, 8), new Allocations { Customers = new List<Customer>() { new() { Name = "Ian Richards" } }, SpacesAvailable = 0 } },
            { new DateTime(2023, 1, 9), new Allocations { Customers = new List<Customer>() { new() { Name = "Ian Richards" } }, SpacesAvailable = 0 } },
            { new DateTime(2023, 1, 10), new Allocations { Customers = new List<Customer>() { new() { Name = "Ian Richards" } }, SpacesAvailable = 1 } }
        };
    }

    public void ReserveParking(DateTime from, DateTime to, Customer customer)
    {
        if (IsParkingAvailable(from, to))
        {
            for (DateTime date = from; date <= to; date = date.AddDays(1))
            {
                _reservations[date].Customers.Add(customer);
            }
        }
        else
        {
            throw new UnableToReserveSpaceException("UnableToReserveSpaceException: ParkingManager threw an exception when trying to reserves space for given date range");
        }
    }

    public bool IsParkingAvailable(DateTime from, DateTime to)
    {
        for (DateTime date = from; date <= to; date = date.AddDays(1))
        {
            if (_reservations.TryGetValue(date, out var reservation))
            {
                if (reservation.SpacesAvailable == 0)
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