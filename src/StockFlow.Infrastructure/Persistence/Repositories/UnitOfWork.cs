using StockFlow.Domain.Entities;
using StockFlow.Domain.Repositories;

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
}
