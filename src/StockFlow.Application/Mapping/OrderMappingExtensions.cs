using StockFlow.Application.Dtos.Orders;
using StockFlow.Domain.Entities;

namespace StockFlow.Application.Mapping;

internal static class OrderMappingExtensions
{
    public static OrderDto ToDto(this Order order)
    {
        var items = order.OrderItems.Select(i => i.ToDto()).ToList();
        return new OrderDto(order.Id, order.CustomerId, order.Customer.Name, order.PlacedAt, order.CompletedAt, order.Status.ToString(), order.Notes, items, items.Sum(i => i.LineTotal));
    }
}