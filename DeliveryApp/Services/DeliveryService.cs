using DeliveryApp.Interfaces;
using DeliveryApp.Models;
using DeliveryApp.Transports;

namespace DeliveryApp.Services;

public class DeliveryService
{
    private readonly List<Transport> _transports = new();
    private readonly List<Order> _activeOrders = new();
    private readonly List<DeliveryRecord> _history = new();
    private int _nextTransportId = 1;
    private int _nextOrderNumber = 1;

    public IReadOnlyList<Transport> Transports => _transports;
    public IReadOnlyList<Order> ActiveOrders => _activeOrders;
    public IReadOnlyList<DeliveryRecord> History => _history;

    // 1. Створення транспорту і замовлень
    public Bike AddBike(double maxWeight = 15) => Register(new Bike(_nextTransportId++, maxWeight));
    public Car AddCar(double maxWeight = 300) => Register(new Car(_nextTransportId++, maxWeight));
    public Truck AddTruck(double maxWeight = 5000) => Register(new Truck(_nextTransportId++, maxWeight));

    private T Register<T>(T transport) where T : Transport
    {
        _transports.Add(transport);
        Console.WriteLine($"Додано транспорт: {transport}");
        return transport;
    }

    public Order CreateOrder(string description, double weight, double distance)
    {
        var order = new Order(_nextOrderNumber++, description, weight, distance);
        _activeOrders.Add(order);
        Console.WriteLine($"Створено замовлення: {order}");
        return order;
    }

    // 2. Вартість доставки кожним транспортом, що може взяти замовлення
    public void ShowDeliveryCosts(Order order)
    {
        Console.WriteLine($"Вартість доставки замовлення #{order.Number} ({order.Weight} кг, {order.Distance} км):");

        var suitable = _transports.Where(t => t.CanTake(order)).ToList();
        if (suitable.Count == 0)
        {
            Console.WriteLine("  Жоден транспорт не може взяти це замовлення");
            return;
        }

        foreach (var t in suitable)
        {
            string state = t.IsFree ? "вільний" : "зайнятий";
            Console.WriteLine($"  {t.Name} ({state}): {t.CalculateCost(order):0.00} грн");
        }
    }

    // 3. Призначення замовлення на транспорт
    public bool AssignOrder(Order order, Transport transport)
    {
        if (order.Status != OrderStatus.Created)
        {
            Console.WriteLine($"Помилка: замовлення #{order.Number} вже має статус {order.Status}");
            return false;
        }
        if (!transport.IsFree)
        {
            Console.WriteLine($"Помилка: {transport.Name} зайнятий замовленням #{transport.CurrentOrder!.Number}");
            return false;
        }
        if (!transport.CanTake(order))
        {
            Console.WriteLine($"Помилка: {transport.Name} не може взяти замовлення #{order.Number} " +
                              $"({order.Weight} кг, {order.Distance} км)");
            return false;
        }

        transport.Occupy(order);
        order.Transport = transport;
        order.Status = OrderStatus.Assigned;
        Console.WriteLine($"Замовлення #{order.Number} призначено на {transport.Name}");
        return true;
    }

    // 4. Доставка з оплатою
    public bool DeliverOrder(Order order, IPayment payment)
    {
        if (order.Status != OrderStatus.Assigned || order.Transport == null)
        {
            Console.WriteLine($"Помилка: замовлення #{order.Number} не можна доставити, статус {order.Status}");
            return false;
        }

        Transport transport = order.Transport;
        decimal cost = transport.CalculateCost(order);

        if (!payment.Pay(cost, out string message))
        {
            Console.WriteLine($"Оплата не пройшла: {message}. Замовлення #{order.Number} лишається активним");
            return false;
        }

        Console.WriteLine(message);
        transport.Release();
        order.Status = OrderStatus.Delivered;
        _activeOrders.Remove(order);
        _history.Add(new DeliveryRecord(order, transport.Name, cost, payment.MethodName));
        Console.WriteLine($"Замовлення #{order.Number} доставлено, {transport.Name} знову вільний");
        return true;
    }

    // 5. Виведення
    public void PrintTransports()
    {
        Console.WriteLine("Транспорт:");
        foreach (var t in _transports)
            Console.WriteLine($"  {t}");
    }

    public void PrintActiveOrders()
    {
        Console.WriteLine("Активні замовлення:");
        if (_activeOrders.Count == 0)
            Console.WriteLine("  немає");
        foreach (var o in _activeOrders)
            Console.WriteLine($"  {o}");
    }

    public void PrintHistory()
    {
        Console.WriteLine("Історія доставок:");
        if (_history.Count == 0)
            Console.WriteLine("  порожня");
        foreach (var r in _history)
            Console.WriteLine($"  {r}");
        if (_history.Count > 0)
            Console.WriteLine($"  Разом: {_history.Sum(r => r.Cost):0.00} грн");
    }
}
