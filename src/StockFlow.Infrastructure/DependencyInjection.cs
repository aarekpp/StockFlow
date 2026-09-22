using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using StockFlow.Domain.Repositories;
using StockFlow.Infrastructure.Persistence;
using StockFlow.Infrastructure.Persistence.Repositories;

namespace StockFlow.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, string connectionString)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(connectionString);
        services.AddDbContext<StockFlowDbContext>(options => options.UseSqlServer(connectionString));
        
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}