using StockFlow.Application.Dtos.OrderItems;
using StockFlow.Domain.Entities;

namespace StockFlow.Application.Mapping;

internal static class OrderItemMappingExtensions
{
    public static OrderItemDto ToDto(this OrderItem item)
    {
        return new OrderItemDto(item.Id, item.ProductId, item.Product.Name, item.Quantity, item.UnitPriceAtOrderTime, item.LineTotal);
    }
}