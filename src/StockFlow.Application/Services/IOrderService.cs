using StockFlow.Application.Dtos.Orders;

namespace StockFlow.Application.Services;

public interface IOrderService
{
    Task<OrderDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<OrderDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<OrderDto> CreateAsync(CreateOrderDto dto, CancellationToken cancellationToken = default);
    Task<OrderDto> UpdateStatusAsync(Guid id, UpdateOrderStatusDto dto, CancellationToken cancellationToken = default);
}
