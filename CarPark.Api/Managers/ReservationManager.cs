using CarPark.Api.Models;
using CarPark.Api.Repositories;

namespace CarPark.Api.Managers;


public interface IReservationManager
{
    bool IsParkingAvailable(DateTime from, DateTime to);
    decimal GetParkingPriceForDateRange(DateTime from, DateTime to);
    void ReserveParking(DateTime from, DateTime to, string? name);
    void AmendReservation(DateTime from, DateTime to, string? name);
    Dictionary<string, Reservation> GetReservations();
    void CancelParking(string? name);
    List<SpaceAvailability> GetAvailableParking(DateTime from, DateTime to);
}

public class ReservationManager : IReservationManager
{
    private readonly IParkingSpaceManager _parkingSpaceManager;
    private readonly IReservationsRepository _reservationsRepository;
    public ReservationManager(IParkingSpaceManager parkingSpaceManager, IReservationsRepository reservationsRepository)
    {
        _parkingSpaceManager = parkingSpaceManager;
        _reservationsRepository = reservationsRepository;
    }

    public Dictionary<string, Reservation> GetReservations()
    {
        return _reservationsRepository.GetReservations();
    }

    public List<SpaceAvailability> GetAvailableParking(DateTime from, DateTime to)
    {
        var spaces = new List<SpaceAvailability>();
        for (DateTime date = from; date <= to; date = date.AddDays(1))
        {
            var totalParkingSpacesAvailable = _parkingSpaceManager.GetTotalParkingSpacesAvailableByDate(date);
            spaces.Add(new SpaceAvailability() { Date = date, SpacesAvailable = totalParkingSpacesAvailable });
        }

        return spaces;
    }

    public void ReserveParking(DateTime from, DateTime to, string? name)
    {
        if (name != null && IsParkingAvailable(from, to))
        {
            for (DateTime date = from; date <= to; date = date.AddDays(1))
            {
                _parkingSpaceManager.ReserveSpace(date);
            }

            _reservationsRepository.AddReservation(new Reservation() { From = from, To = to, Name = name });
        }
        else
        {
            throw new UnableToReserveSpaceException("UnableToReserveSpaceException: ReservationManager threw an exception when trying to reserves space for given date range");
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
        if (name != null && _reservationsRepository.ReservationExists(name))
        {
            var reservation = _reservationsRepository.GetReservationByName(name);
            for (DateTime date = reservation.From; date <= reservation.To; date = date.AddDays(1))
            {
                _parkingSpaceManager.UnReserveSpace(date);
            }

            _reservationsRepository.RemoveReservation(name);
        }
        else
        {
            throw new ReservationNotFoundException("ReservationNotFoundException: Reservation not found in reservations");
        }
    }

    public bool IsParkingAvailable(DateTime from, DateTime to)
    {
        for (DateTime date = from; date <= to; date = date.AddDays(1))
        {
            if (!_parkingSpaceManager.IsSpaceAvailable(date))
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
}

public class ReservationNotFoundException : Exception
{
    public ReservationNotFoundException(string message) : base(message) { }
}

public class UnableToReserveSpaceException : Exception
{
    public UnableToReserveSpaceException(string message) : base(message) { }
}