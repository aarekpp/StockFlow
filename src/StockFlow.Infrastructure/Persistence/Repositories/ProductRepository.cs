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

    public override async Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await DbSet.Include(p => p.Category).FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    }

    public override async Task<IReadOnlyList<Product>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await DbSet.Include(p => p.Category).OrderBy(p => p.Name).ToListAsync(cancellationToken);
    }

    public async Task<Product?> GetWithSuppliersAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await DbSet.Include(p => p.ProductSuppliers).ThenInclude(ps => ps.Supplier).FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    }
}
