using DeliveryApp.Interfaces;

namespace DeliveryApp.Payments;

public class CardPayment : IPayment
{
    private readonly string _cardNumber;
    public decimal Balance { get; private set; }
    public string MethodName => $"картка {MaskedNumber}";

    private string MaskedNumber => "**** " + _cardNumber[^4..];

    public CardPayment(string cardNumber, decimal balance)
    {
        if (cardNumber.Length < 4) throw new ArgumentException("Некоректний номер картки");
        _cardNumber = cardNumber;
        Balance = balance;
    }

    public bool Pay(decimal amount, out string message)
    {
        if (Balance < amount)
        {
            message = $"Недостатньо коштів на картці {MaskedNumber} (баланс {Balance:0.00} грн, потрібно {amount:0.00} грн)";
            return false;
        }

        Balance -= amount;
        message = $"Списано {amount:0.00} грн з картки {MaskedNumber}, залишок {Balance:0.00} грн";
        return true;
    }
}
