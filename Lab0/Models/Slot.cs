using VendingMachineApp.Models;

namespace VendingMachineApp.Models;

// Ячейка
public class Slot
{
    public Product? Product { get; private set; }
    public decimal Price { get; private set; }
    public int Quantity { get; private set; }

    public bool IsEmpty() => Product == null || Quantity <= 0;

    public void SetProduct(Product product, decimal price, int quantity)
    {
        Product = product;
        Price = price;
        Quantity = quantity;
    }

    public void RemoveQuantity(int amount)
    {
        Quantity -= amount;
        if (Quantity < 0) Quantity = 0;
    }
}