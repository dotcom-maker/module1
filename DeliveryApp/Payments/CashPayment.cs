using DeliveryApp.Interfaces;

namespace DeliveryApp.Payments;

public class CashPayment : IPayment
{
    public decimal GivenAmount { get; }
    public string MethodName => "готівка";

    public CashPayment(decimal givenAmount)
    {
        GivenAmount = givenAmount;
    }

    public bool Pay(decimal amount, out string message)
    {
        if (GivenAmount < amount)
        {
            message = $"Недостатньо готівки: дали {GivenAmount:0.00} грн, потрібно {amount:0.00} грн";
            return false;
        }

        decimal change = GivenAmount - amount;
        message = change > 0
            ? $"Отримано {GivenAmount:0.00} грн готівкою, решта {change:0.00} грн"
            : $"Отримано {amount:0.00} грн готівкою без решти";
        return true;
    }
}
