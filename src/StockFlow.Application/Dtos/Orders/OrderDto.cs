using StockFlow.Application.Dtos.OrderItems;

namespace StockFlow.Application.Dtos.Orders;

public record OrderDto(Guid Id, Guid CustomerId, string CustomerName, DateTime PlacedAt, DateTime? CompletedAt, string Status, string? Notes, IReadOnlyList<OrderItemDto> OrderItems, decimal TotalAmount);
