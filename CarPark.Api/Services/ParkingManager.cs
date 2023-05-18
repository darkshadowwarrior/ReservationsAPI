using CarPark.Api.Models;
using CarPark.Api.Repositories;

namespace CarPark.Api.Services;


public interface IParkingManager
{
    bool IsParkingAvailable(DateTime from, DateTime to);
    decimal GetParkingPriceForDateRange(DateTime from, DateTime to);
    void ReserveParking(DateTime from, DateTime to, string? name);
    Dictionary<string, ParkingReservation> GetReservations();
    void CancelParking(string? name);
    List<ParkingSpace> GetAvailableParking(DateTime from, DateTime to);
}

public class ParkingManager : IParkingManager
{
    private readonly IParkingSpaceRepository _parkingSpaceRepository;
    private readonly Dictionary<string, ParkingReservation> _parkingReservations;
    public ParkingManager(IParkingSpaceRepository parkingSpaceRepository)
    {
        _parkingSpaceRepository = parkingSpaceRepository;

        
        _parkingReservations = new Dictionary<string, ParkingReservation>()
        {
            { "Ian Richards ", new ParkingReservation() { Name = "Ian Richards", From = new DateTime(2023, 1, 1), To = new DateTime(2023, 1, 6) } }
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
                _parkingSpaceRepository.ReserveSpace(date);
            }

            _parkingReservations.Add(name, new ParkingReservation() { From = from, To = to, Name = name });
        }
        else
        {
            throw new UnableToReserveSpaceException("UnableToReserveSpaceException: ParkingManager threw an exception when trying to reserves space for given date range");
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
                _parkingSpaceRepository.UnReserveSpace(date);
            }

            _parkingReservations.Remove(name);
        }
    }

    public bool IsParkingAvailable(DateTime from, DateTime to)
    {
        for (DateTime date = from; date <= to; date = date.AddDays(1))
        {
            if (!_parkingSpaceRepository.IsSpaceAvailable(date))
            {
                return false;
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

    public List<ParkingSpace> GetAvailableParking(DateTime from, DateTime to)
    {
        var spaces = new List<ParkingSpace>();
        for (DateTime date = from; date <= to; date = date.AddDays(1))
        {
            var totalParkingSpacesAvailable = _parkingSpaceRepository.GetTotalParkingSpacesAvailableByDate(date);
            spaces.Add(new ParkingSpace() {Date = date, SpacesAvailable = totalParkingSpacesAvailable });
        }

        return spaces;
    }
}

public class ParkingSpace
{
    public DateTime Date { get; set; }
    public int SpacesAvailable { get; set; }
}

public class UnableToReserveSpaceException : Exception
{
    public UnableToReserveSpaceException(string message) : base(message) { }
}