using DeliveryApp.Models;

namespace DeliveryApp.Transports;

public class Bike : Transport
{
    public const double MaxDistance = 10;

    public Bike(int id, double maxWeight = 15) : base(id, $"Велосипед {id}", maxWeight) { }

    // Велосипед додатково обмежений відстанню
    public override bool CanTake(Order order) => base.CanTake(order) && order.Distance <= MaxDistance;

    public override decimal CalculateCost(Order order) => 20m + 5m * (decimal)order.Distance;
}
