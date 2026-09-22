using Microsoft.EntityFrameworkCore;
using StockFlow.Domain.Entities;
using StockFlow.Domain.Repositories;

namespace StockFlow.Infrastructure.Persistence.Repositories;

internal sealed class ProductRepository(StockFlowDbContext context) : Repository<Product>(context), IProductRepository
{
    public async Task<IReadOnlyList<Product>> GetBelowStockLevelAsync(int threshold, CancellationToken cancellationToken = default)
    {
        return await DbSet.Where(p => p.StockQuantity < threshold).OrderBy(p => p.StockQuantity).ThenBy(p => p.Name).ToListAsync(cancellationToken);
    }
}
