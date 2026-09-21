using StockFlow.Domain.Common;
using StockFlow.Domain.Enums;

namespace StockFlow.Domain.Entities;

public sealed class Order : BaseEntity
{
    private readonly List<OrderItem> _orderItems = [];
    private readonly List<StockMovement> _stockMovements = [];

    public Guid CustomerId { get; private set; }
    public DateTime PlacedAt { get; private set; }
    public DateTime? CompletedAt { get; private set; }
    public OrderStatus Status { get; private set; } = OrderStatus.New;
    public string? Notes { get; private set; }
    public Customer Customer { get; private set; } = null!;

    public IReadOnlyCollection<OrderItem> OrderItems => _orderItems.AsReadOnly();
    public IReadOnlyCollection<StockMovement> StockMovements => _stockMovements.AsReadOnly();

    private Order() { }

    public Order(Guid customerId, DateTime placedAt, string? notes = null)
    {
        CustomerId = customerId;
        PlacedAt = placedAt;
        Notes = notes?.Trim();
    }
}
