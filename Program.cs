using System;
using static System.Console;
class PizzaOrderEventArgs : EventArgs{
    public int OrderId { get; }
    public string CustomerName { get; }
    public string CustomerTelNum { get; }
    public Dictionary<string,int> Order { get; } = new Dictionary<string, int>();
    public PizzaOrderEventArgs(int orderId, string customerName, string customerTelNum, 
        Dictionary<string,int> order){
        OrderId = orderId;
        CustomerName = customerName;
        CustomerTelNum = customerTelNum;
        Order = order;
    } 
}
class OrderSystem{
    public event EventHandler<PizzaOrderEventArgs>? OrderPlaced;
    public event EventHandler<PizzaOrderEventArgs>? OrderStatusNotified;
    public event EventHandler<PizzaOrderEventArgs>? OrderShipped;
    public void PlaceOrder(int orderId, string customerName, string customerTelNum, 
        Dictionary<string,int> order){
        WriteLine($"\n\tOrder #{orderId} placed by {customerName} with {customerTelNum} number.");
        OrderPlaced?.Invoke(this,new PizzaOrderEventArgs(orderId,customerName,customerTelNum,order));
    }
    public void NotifyAboutStatus(int orderId, string customerName, string customerTelNum, 
        Dictionary<string,int> order){
            WriteLine($"\n\tOrder #{orderId} {customerName} with {customerTelNum} "
            + "number was notified that order is ready.");
        OrderStatusNotified?.Invoke(this,new PizzaOrderEventArgs(orderId,
            customerName,customerTelNum,order));
    }
    public void ShipOrder(int orderId, string customerName, string customerTelNum, 
        Dictionary<string,int> order){
            WriteLine($"\n\tOrder #{orderId} shipped to {customerName} with {customerTelNum}");
        OrderShipped?.Invoke(this,new PizzaOrderEventArgs(orderId,
            customerName,customerTelNum,order));
    }
}
class NotifyService{
    public void OnOrderPlaced(object? sender, PizzaOrderEventArgs e)
        => WriteLine($"Confirmation message sent to {e.CustomerName} with {e.CustomerTelNum}");
    public void OnStatusNotified(object? sender, PizzaOrderEventArgs e)
        => WriteLine($"Show queue place to {e.CustomerName} with {e.CustomerTelNum}");
    public void OnOrderShipped(object? sender, PizzaOrderEventArgs e)
        => WriteLine($"Shipped message and review to {e.CustomerName} with {e.CustomerTelNum}");
}
class InventoryService{
    public void OnOrderPlaced(object? sender, PizzaOrderEventArgs e)
        => WriteLine($"Products ready to {e.CustomerName} with {e.CustomerTelNum}");
    public void OnStatusNotified(object? sender, PizzaOrderEventArgs e)
        => WriteLine($"Show cooking info to {e.CustomerName} with {e.CustomerTelNum}");
    public void OnOrderShipped(object? sender, PizzaOrderEventArgs e)
        => WriteLine($"Pizza is ready to {e.CustomerName} with {e.CustomerTelNum}");
}
class BillingService{
    public void OnOrderPlaced(object? sender, PizzaOrderEventArgs e)
        => WriteLine($"Money paid by {e.CustomerName} with {e.CustomerTelNum}");
    public void OnOrderShipped(object? sender, PizzaOrderEventArgs e)
        => WriteLine($"Cashback to {e.CustomerName} with {e.CustomerTelNum}");
}
class Program{
    static void Main(string[] args){
        var orders = new OrderSystem();
        var notify = new NotifyService();
        var inventory = new InventoryService();
        var billing = new BillingService();

        orders.OrderPlaced += notify.OnOrderPlaced;
        orders.OrderPlaced += inventory.OnOrderPlaced;
        orders.OrderPlaced += billing.OnOrderPlaced;

        orders.OrderStatusNotified += notify.OnStatusNotified;
        orders.OrderStatusNotified += inventory.OnStatusNotified;

        orders.OrderShipped += notify.OnOrderShipped;
        orders.OrderShipped += inventory.OnOrderShipped;
        orders.OrderShipped += billing.OnOrderShipped;

        orders.PlaceOrder(1,"Bob","+996707707070",new Dictionary<string,int>{["peperony"]=1});
        orders.NotifyAboutStatus(1,"Bob","+996707707070",new Dictionary<string,int>{["peperony"]=1});
        orders.ShipOrder(1,"Bob","+996707707070",new Dictionary<string,int>{["peperony"]=1});
        orders.PlaceOrder(2,"Ann","+996708808080",new Dictionary<string,int>{["5 cheeses"]=3});
    }
}