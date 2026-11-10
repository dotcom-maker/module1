using System.Text;
using DeliveryApp.Payments;
using DeliveryApp.Services;

namespace DeliveryApp;

internal class Program
{
    private static void Section(string title)
    {
        Console.WriteLine();
        Console.WriteLine($"===== {title} =====");
    }

    private static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;
        var service = new DeliveryService();

        Section("1. Створення транспорту");
        var bike = service.AddBike();
        var car = service.AddCar();
        var truck = service.AddTruck();

        Section("1. Створення замовлень");
        var documents = service.CreateOrder("Документи", 1, 5);       // може будь-хто
        var pizza = service.CreateOrder("Піца", 2, 3);                // може будь-хто
        var laptop = service.CreateOrder("Ноутбук", 3, 25);           // велосипед — задалеко
        var fridge = service.CreateOrder("Холодильник", 80, 12);      // велосипед — заважко
        var furniture = service.CreateOrder("Меблі", 1200, 40);       // тільки вантажівка
        var tooHeavy = service.CreateOrder("Верстат", 8000, 15);      // ніхто

        try
        {
            service.CreateOrder("Порожнє", 0, 5);
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"Не вдалося створити замовлення: {ex.Message}");
        }

        Section("2. Вартість доставки");
        service.ShowDeliveryCosts(documents);
        service.ShowDeliveryCosts(laptop);
        service.ShowDeliveryCosts(furniture);
        service.ShowDeliveryCosts(tooHeavy);

        Section("3. Призначення замовлень");
        service.AssignOrder(laptop, bike);       // велосипед: далі 10 км
        service.AssignOrder(fridge, bike);       // велосипед: заважко
        service.AssignOrder(documents, bike);    // успішно
        service.AssignOrder(pizza, bike);        // велосипед зайнятий
        service.AssignOrder(laptop, car);        // успішно
        service.AssignOrder(furniture, car);     // автомобіль зайнятий
        service.AssignOrder(furniture, truck);   // успішно
        service.AssignOrder(documents, truck);   // замовлення вже призначене
        service.AssignOrder(tooHeavy, truck);    // вантажівка зайнята

        service.PrintTransports();
        service.PrintActiveOrders();

        Section("4. Доставка");
        service.DeliverOrder(pizza, new CashPayment(100));                       // не призначене
        service.DeliverOrder(documents, new CardPayment("4441111122223333", 1000)); // карткою
        service.DeliverOrder(laptop, new CashPayment(500));                      // готівкою з рештою
        service.DeliverOrder(furniture, new CardPayment("5168000011112222", 300)); // недостатньо на картці
        service.DeliverOrder(furniture, new CashPayment(500));                   // недостатньо готівки
        service.DeliverOrder(furniture, new CardPayment("5168000011112222", 2000)); // успішно
        service.DeliverOrder(documents, new CashPayment(100));                   // вже доставлене

        Section("Повторне використання звільненого транспорту");
        service.ShowDeliveryCosts(pizza);
        service.AssignOrder(pizza, bike);
        service.AssignOrder(fridge, bike);       // велосипед і так зайнятий
        service.AssignOrder(fridge, car);
        service.AssignOrder(tooHeavy, truck);    // вантажівка вільна, але не потягне
        service.DeliverOrder(pizza, new CashPayment(35));   // без решти

        Section("5. Підсумок");
        service.PrintTransports();
        Console.WriteLine();
        service.PrintActiveOrders();
        Console.WriteLine();
        service.PrintHistory();
    }
}
