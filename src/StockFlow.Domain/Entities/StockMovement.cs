using StockFlow.Domain.Common;
using StockFlow.Domain.Enums;

namespace StockFlow.Domain.Entities;

public sealed class StockMovement : BaseEntity
{
    public Guid ProductId { get; private set; }
    public StockMovementType Type { get; private set; }
    public int Quantity { get; private set; }
    public DateTime OccurredAt { get; private set; }
    public Guid? OrderId { get; private set; }
    public string? Comment { get; private set; }
    public Product Product { get; private set; } = null!;
    public Order? Order { get; private set; }

    private StockMovement() { }

    public StockMovement(Guid productId, StockMovementType type, int quantity, DateTime occurredAt, Guid? orderId = null, string? comment = null)
    {
        ProductId = productId;
        Type = type;
        Quantity = quantity;
        OccurredAt = occurredAt;
        OrderId = orderId;
        Comment = comment?.Trim();
    }
}
