using StockFlow.Domain.Entities;

namespace StockFlow.Domain.Repositories;

public interface IUnitOfWork
{
    IProductRepository Products { get; }
    IOrderRepository Orders { get; }
    IRepository<Category> Categories { get; }
    IRepository<Supplier> Suppliers { get; }
    IRepository<Customer> Customers { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
