using DeliveryApp.Models;

namespace DeliveryApp.Transports;

public class Truck : Transport
{
    public Truck(int id, double maxWeight = 5000) : base(id, $"Вантажівка {id}", maxWeight) { }

    public override decimal CalculateCost(Order order) =>
        200m + 15m * (decimal)order.Distance + 0.5m * (decimal)order.Weight;
}
