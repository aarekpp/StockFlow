using Microsoft.EntityFrameworkCore;
using StockFlow.Domain.Entities;
using StockFlow.Domain.Repositories;
using System.Data;

namespace StockFlow.Infrastructure.Persistence.Repositories;

internal sealed class UnitOfWork : IUnitOfWork, IDisposable, IAsyncDisposable
{
    private readonly StockFlowDbContext _context;
    private bool _disposed;

    public UnitOfWork(StockFlowDbContext context)
    {
        ArgumentNullException.ThrowIfNull(context);
        _context = context;

        Products = new ProductRepository(context);
        Orders = new OrderRepository(context);
        Categories = new Repository<Category>(context);
        Suppliers = new Repository<Supplier>(context);
        Customers = new Repository<Customer>(context);
    }

    public IProductRepository Products { get; }
    public IOrderRepository Orders { get; }
    public IRepository<Category> Categories { get; }
    public IRepository<Supplier> Suppliers { get; }
    public IRepository<Customer> Customers { get; }

    public void Dispose()
    {
        if (_disposed) return;
        _disposed = true;
        _context.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        if (_disposed) return;
        _disposed = true;
        await _context.DisposeAsync();
    }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return _context.SaveChangesAsync(cancellationToken);
    }

    public async Task ExecuteInTransactionAsync(Func<CancellationToken, Task> operation, IsolationLevel isolationLevel = IsolationLevel.ReadCommitted, CancellationToken cancellationToken = default)
    {
        var strategy = _context.Database.CreateExecutionStrategy();

        await strategy.ExecuteAsync(async () =>
        {
            await using var transaction = await _context.Database.BeginTransactionAsync(isolationLevel, cancellationToken);

            try
            {
                await operation(cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);
            }
            catch
            {
                await transaction.RollbackAsync(cancellationToken);
                throw;
            }
        });
    }
}
