namespace Reservations.Api.Managers;

public interface IParkingSpaceManager
{
    void ReserveSpace(DateTime date);
    void DeallocateSpace(DateTime date);
    bool IsSpaceAvailable(DateTime date);
    int GetTotalSpaceAvailabilitiesByDate(DateTime date);
}

public class ParkingSpaceManager : IParkingSpaceManager
{
    private readonly Dictionary<DateTime, int> _availableParkingSpacesByDate;

    public ParkingSpaceManager()
    {
        _availableParkingSpacesByDate = new Dictionary<DateTime, int>();
    }

    public void ReserveSpace(DateTime date)
    {
        _availableParkingSpacesByDate.TryAdd(date, 0);
        _availableParkingSpacesByDate[date] += 1;
    }

    public void DeallocateSpace(DateTime date)
    {
        _availableParkingSpacesByDate[date] -= 1;
    }

    public bool IsSpaceAvailable(DateTime date)
    {
        if (!_availableParkingSpacesByDate.TryGetValue(date, out var reservation)) return true;

        return reservation < 10;
    }

    public int GetTotalSpaceAvailabilitiesByDate(DateTime date)
    {
        _availableParkingSpacesByDate.TryGetValue(date, out int value);

        return 10 - value;
    }
}