using Microsoft.EntityFrameworkCore;
using StockFlow.Domain.Common;
using StockFlow.Domain.Repositories;

namespace StockFlow.Infrastructure.Persistence.Repositories;

internal class Repository<T> : IRepository<T> where T : BaseEntity
{
    protected StockFlowDbContext Context { get; }
    protected DbSet<T> DbSet { get; }

    public Repository(StockFlowDbContext context)
    {
        ArgumentNullException.ThrowIfNull(context);
        Context = context;
        DbSet = context.Set<T>();
    }

    public virtual async Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await DbSet.FindAsync([id], cancellationToken);
    }

    public virtual async Task<IReadOnlyList<T>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await DbSet.ToListAsync(cancellationToken);
    }

    public virtual async Task AddAsync(T entity, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(entity);
        await DbSet.AddAsync(entity, cancellationToken);
    }

    public virtual void Update(T entity)
    {
        ArgumentNullException.ThrowIfNull(entity);
        if (Context.Entry(entity).State != EntityState.Detached) return;
        DbSet.Update(entity);
    }

    public virtual void Remove(T entity)
    {
        ArgumentNullException.ThrowIfNull(entity);
        DbSet.Remove(entity);
    }
}
