using StockFlow.Domain.Entities;

namespace StockFlow.Domain.Repositories;

public interface IProductRepository : IRepository<Product>
{
    Task<IReadOnlyList<Product>> GetBelowStockLevelAsync(int threshold, CancellationToken cancellationToken = default);
    Task<Product?> GetWithSuppliersAsync(Guid id, CancellationToken cancellationToken = default);
}
