using StockFlow.Domain.Entities;

namespace StockFlow.Domain.Repositories;

public interface IOrderRepository : IRepository<Order>
{
    Task<Order?> GetWithItemsAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Order>> GetAllWithItemsAsync(CancellationToken cancellationToken = default);
}
