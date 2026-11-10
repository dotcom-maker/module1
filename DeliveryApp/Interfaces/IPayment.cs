namespace DeliveryApp.Interfaces;

public interface IPayment
{
    string MethodName { get; }

    // Повертає true, якщо оплата пройшла
    bool Pay(decimal amount, out string message);
}
