namespace StockFlow.Application.Dtos.OrderItems;

public record CreateOrderItemDto(Guid ProductId, int Quantity);