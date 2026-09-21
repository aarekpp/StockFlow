using StockFlow.Domain.Entities;

namespace StockFlow.Domain.Repositories;

public interface IProductRepository : IRepository<Product>
{
    Task<IReadOnlyList<Product>> GetBelowStockLevelAsync(int treshold, CancellationToken cancellationToken = default);
}
