namespace CarPark.Api.Repositories;

public interface IParkingSpaceRepository
{
    void ReserveSpace(DateTime date);
    void UnReserveSpace(DateTime date);
    bool IsSpaceAvailable(DateTime date);
    int GetTotalParkingSpacesAvailableByDate(DateTime date);
}

public class ParkingSpaceRepository : IParkingSpaceRepository
{
    private readonly Dictionary<DateTime, int> _parkingSpaceAllocations;

    public ParkingSpaceRepository()
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

    }
    public void ReserveSpace(DateTime date)
    {
        _parkingSpaceAllocations.TryAdd(date, 0);
        _parkingSpaceAllocations[date] += 1;
    }

    public void UnReserveSpace(DateTime date)
    {
        _parkingSpaceAllocations[date] -= 1;
    }

    public bool IsSpaceAvailable(DateTime date)
    {
        if (!_parkingSpaceAllocations.TryGetValue(date, out var reservation)) return true;

        return reservation < 10;
    }

    public int GetTotalParkingSpacesAvailableByDate(DateTime date)
    {
        _parkingSpaceAllocations.TryGetValue(date, out int value);

        return (10 - value);
    }
}