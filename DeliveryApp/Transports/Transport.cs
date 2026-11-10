using DeliveryApp.Interfaces;
using DeliveryApp.Models;

namespace DeliveryApp.Transports;

public abstract class Transport : ITransport
{
    public int Id { get; }
    public string Name { get; }
    public double MaxWeight { get; }
    public bool IsFree => CurrentOrder == null;
    public Order? CurrentOrder { get; private set; }

    protected Transport(int id, string name, double maxWeight)
    {
        Id = id;
        Name = name;
        MaxWeight = maxWeight;
    }

    public virtual bool CanTake(Order order) => order.Weight <= MaxWeight;

    public abstract decimal CalculateCost(Order order);

    internal void Occupy(Order order) => CurrentOrder = order;
    internal void Release() => CurrentOrder = null;

    public override string ToString()
    {
        string state = IsFree ? "вільний" : $"зайнятий (замовлення #{CurrentOrder!.Number})";
        return $"[{Id}] {Name}, до {MaxWeight} кг, {state}";
    }
}
