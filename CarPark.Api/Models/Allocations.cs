namespace CarPark.Api.Models;

public class Allocations : List<Customer>
{
    public List<Customer> Customers = new();
    public int SpacesAvailable { get; set; }
}