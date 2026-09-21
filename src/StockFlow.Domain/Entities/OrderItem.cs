using StockFlow.Domain.Common;

namespace StockFlow.Domain.Entities;

public sealed class OrderItem : BaseEntity
{
    public Guid OrderId { get; private set; }
    public Guid ProductId { get; private set; }
    public int Quantity { get; private set; }
    public decimal UnitPriceAtOrderTime { get; private set; }
    public Order Order { get; private set; } = null!;
    public Product Product { get; private set; } = null!;

    public decimal LineTotal => Quantity * UnitPriceAtOrderTime;

    private OrderItem() { }

    internal OrderItem(Order order, Product product, int quantity)
    {
        Order = order;
        OrderId = order.Id;
        Product = product;
        ProductId = product.Id;
        Quantity = quantity;
        UnitPriceAtOrderTime = product.UnitPrice;
    }
}
