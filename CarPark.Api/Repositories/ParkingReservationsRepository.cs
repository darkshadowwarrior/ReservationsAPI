using CarPark.Api.Models;

namespace CarPark.Api.Repositories;

public interface IParkingReservationsRepository
{
    Dictionary<string, ParkingReservation> GetReservations();
    void AddReservation(ParkingReservation parkingReservation);
    bool ReservationExists(string name);
    ParkingReservation GetReservationByName(string name);
    void RemoveReservation(string name);
}

public class ParkingReservationsRepository : IParkingReservationsRepository
{
    private readonly Dictionary<string, ParkingReservation> _parkingReservations;

    public ParkingReservationsRepository()
    {
        _parkingReservations = new Dictionary<string, ParkingReservation>()
        {
            { "Ian Richards ", new ParkingReservation() { Name = "Ian Richards", From = new DateTime(2023, 1, 1), To = new DateTime(2023, 1, 6) } }
        };
    }

    public Dictionary<string, ParkingReservation> GetReservations()
    {
        return _parkingReservations;
    }

    public void AddReservation(ParkingReservation parkingReservation)
    {
        if (parkingReservation.Name != null) _parkingReservations.TryAdd(parkingReservation.Name, parkingReservation);
    }

    public bool ReservationExists(string name)
    {
        return _parkingReservations.ContainsKey(name);
    }

    public ParkingReservation GetReservationByName(string name)
    {
        _parkingReservations.TryGetValue(name, out var reservation);
        return reservation!;
    }

    public void RemoveReservation(string name)
    {
        _parkingReservations.Remove(name);
    }
}