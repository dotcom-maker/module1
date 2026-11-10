namespace DeliveryApp.Models;

public class DeliveryRecord
{
    public Order Order { get; }
    public string TransportName { get; }
    public decimal Cost { get; }
    public string PaymentMethod { get; }

    public DeliveryRecord(Order order, string transportName, decimal cost, string paymentMethod)
    {
        Order = order;
        TransportName = transportName;
        Cost = cost;
        PaymentMethod = paymentMethod;
    }

    public override string ToString() =>
        $"#{Order.Number} \"{Order.Description}\" — {TransportName}, {Cost:0.00} грн, оплата: {PaymentMethod}";
}
