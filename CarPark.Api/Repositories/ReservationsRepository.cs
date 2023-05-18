using CarPark.Api.Models;

namespace CarPark.Api.Repositories;

public interface IReservationsRepository
{
    Dictionary<string, Reservation> GetReservations();
    void AddReservation(Reservation parkingReservation);
    bool ReservationExists(string name);
    Reservation GetReservationByName(string name);
    void RemoveReservation(string name);
}

public class ReservationsRepository : IReservationsRepository
{
    private readonly Dictionary<string, Reservation> _parkingReservations;

    public ReservationsRepository()
    {
        _parkingReservations = new Dictionary<string, Reservation>()
        {
            { "Ian Richards ", new Reservation() { Name = "Ian Richards", From = new DateTime(2023, 1, 1), To = new DateTime(2023, 1, 6) } }
        };
    }

    public Dictionary<string, Reservation> GetReservations()
    {
        return _parkingReservations;
    }

    public void AddReservation(Reservation parkingReservation)
    {
        if (parkingReservation.Name != null) _parkingReservations.TryAdd(parkingReservation.Name, parkingReservation);
    }

    public bool ReservationExists(string name)
    {
        return _parkingReservations.ContainsKey(name);
    }

    public Reservation GetReservationByName(string name)
    {
        _parkingReservations.TryGetValue(name, out var reservation);
        return reservation!;
    }

    public void RemoveReservation(string name)
    {
        _parkingReservations.Remove(name);
    }
}