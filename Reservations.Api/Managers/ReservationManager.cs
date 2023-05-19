using Reservations.Api.Models;
using Reservations.Api.Repositories;

namespace Reservations.Api.Managers;


public interface IReservationManager
{
    bool IsParkingAvailable(DateTime from, DateTime to);
    List<Space> GetParkingPricesForDateRange(DateTime from, DateTime to);
    void ReserveParking(DateTime from, DateTime to, string? name);
    void AmendReservation(DateTime from, DateTime to, string? name);
    Dictionary<string, Reservation> GetReservations();
    void CancelReservation(string? name);
    List<SpaceAvailability> GetSpaceAvailabilities(DateTime from, DateTime to);
}

public class ReservationManager : IReservationManager
{
    private readonly IParkingSpaceManager _parkingSpaceManager;
    private readonly IReservationsRepository _reservationsRepository;
    private readonly IPricingManager _pricingManager;

    public ReservationManager(IParkingSpaceManager parkingSpaceManager, IReservationsRepository reservationsRepository, IPricingManager pricingManager)
    {
        _parkingSpaceManager = parkingSpaceManager;
        _reservationsRepository = reservationsRepository;
        _pricingManager = pricingManager;
    }

    public Dictionary<string, Reservation> GetReservations()
    {
        return _reservationsRepository.GetReservations();
    }

    public List<SpaceAvailability> GetSpaceAvailabilities(DateTime from, DateTime to)
    {
        var spaces = new List<SpaceAvailability>();
        for (DateTime date = from; date <= to; date = date.AddDays(1))
        {
            var totalParkingSpacesAvailable = _parkingSpaceManager.GetTotalSpaceAvailabilitiesByDate(date);
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
            throw new UnableToReserveSpaceException($"UnableToReserveSpaceException: ReservationManager threw an exception when trying to reserves space for given date range {from} - {to}");
        }
    }


    public void AmendReservation(DateTime from, DateTime to, string? name)
    {
        if (IsParkingAvailable(from, to))
        {
            CancelReservation(name);
            ReserveParking(from, to, name);
        }
    }

    public void CancelReservation(string? name)
    {
        if (name == null) throw new ArgumentNullException(nameof(name));
        
        if (_reservationsRepository.ReservationExists(name))
        {
            var reservation = _reservationsRepository.GetReservationByName(name);
            for (DateTime date = reservation.From; date <= reservation.To; date = date.AddDays(1))
            {
                _parkingSpaceManager.DeallocateSpace(date);
            }

            _reservationsRepository.RemoveReservation(name);
        }
        else
        {
            throw new ReservationNotFoundException($"ReservationNotFoundException: Reservation not found in reservations for {name}");
        }
    }

    public bool IsParkingAvailable(DateTime from, DateTime to)
    {
        return GetDateRange(from, to).Any(_parkingSpaceManager.IsSpaceAvailable);
    }

    public static IEnumerable<DateTime> GetDateRange(DateTime from, DateTime to)
    {
        return Enumerable.Range(0, 1 + to.Subtract(from).Days).Select(offset => from.AddDays(offset));
    }

    public List<Space> GetParkingPricesForDateRange(DateTime from, DateTime to)
    {
        var prices = new List<Space>();

        for (DateTime date = from; date <= to; date = date.AddDays(1))
        {
            prices.Add(new Space { Date = date, Price = _pricingManager.GetPrice(date) } );
        }

        return prices;
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