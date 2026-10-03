namespace StockFlow.Application.Dtos.OrderItems;

public record OrderItemDto(Guid Id, Guid ProductId, string ProductName, int Quantity, decimal UnitPriceAtOrderTime, decimal LineTotal);
