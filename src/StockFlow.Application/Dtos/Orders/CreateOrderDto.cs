using StockFlow.Application.Dtos.OrderItems;

namespace StockFlow.Application.Dtos.Orders;

public record CreateOrderDto(Guid CustomerId, string? Notes, IReadOnlyList<CreateOrderItemDto> OrderItems);
