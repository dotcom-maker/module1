using DeliveryApp.Models;

namespace DeliveryApp.Transports;

public class Car : Transport
{
    public Car(int id, double maxWeight = 300) : base(id, $"Автомобіль {id}", maxWeight) { }

    public override decimal CalculateCost(Order order) =>
        50m + 8m * (decimal)order.Distance + 1m * (decimal)order.Weight;
}
