using DeliveryApp.Transports;

namespace DeliveryApp.Models;

public class Order
{
    public int Number { get; }
    public string Description { get; }
    public double Weight { get; }
    public double Distance { get; }
    public OrderStatus Status { get; internal set; } = OrderStatus.Created;
    public Transport? Transport { get; internal set; }

    public Order(int number, string description, double weight, double distance)
    {
        if (weight <= 0) throw new ArgumentException("Вага має бути більше 0", nameof(weight));
        if (distance <= 0) throw new ArgumentException("Відстань має бути більше 0", nameof(distance));

        Number = number;
        Description = description;
        Weight = weight;
        Distance = distance;
    }

    public override string ToString()
    {
        string transport = Transport != null ? Transport.Name : "—";
        return $"#{Number} \"{Description}\", {Weight} кг, {Distance} км, статус: {Status}, транспорт: {transport}";
    }
}
