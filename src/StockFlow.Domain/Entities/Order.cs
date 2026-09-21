using StockFlow.Domain.Common;
using StockFlow.Domain.Enums;
using StockFlow.Domain.Exceptions;

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

    public OrderItem AddItem(Product product, int quantity)
    {
        ArgumentNullException.ThrowIfNull(product);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(quantity);
        EnsureStatus(OrderStatus.New, "Items can only be added to a new order.");

        var item = new OrderItem(this, product, quantity);
        _orderItems.Add(item);
        return item;
    }

    public void StartProcessing()
    {
        EnsureStatus(OrderStatus.New, "Only a new order can be started.");
        if (_orderItems.Count == 0) throw new DomainException("An order without items cannot be started.");
        Status = OrderStatus.InProgress;
    }

    public void Complete(DateTime completedAt)
    {
        EnsureStatus(OrderStatus.InProgress, "Only an order in progress can be completed.");
        if (completedAt < PlacedAt) throw new DomainException("Completion date cannot be earlier than the placement date.");
        Status = OrderStatus.Completed;
        CompletedAt = completedAt;
    }

    public void Cancel()
    {
        if (Status is OrderStatus.Completed or OrderStatus.Cancelled) throw new DomainException($"An order with status '{Status}' cannot be cancelled.");
        Status = OrderStatus.Cancelled;
    }

    private void EnsureStatus(OrderStatus expected, string message)
    {
        if (Status != expected) throw new DomainException(message);
    }
}
