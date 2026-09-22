using Microsoft.EntityFrameworkCore;
using StockFlow.Domain.Entities;
using StockFlow.Domain.Repositories;

namespace StockFlow.Infrastructure.Persistence.Repositories;

internal sealed class OrderRepository(StockFlowDbContext context) : Repository<Order>(context), IOrderRepository
{
    public async Task<Order?> GetWithItemsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await DbSet.Include(o => o.OrderItems).ThenInclude(i => i.Product).FirstOrDefaultAsync(o => o.Id == id, cancellationToken);
    }
}
