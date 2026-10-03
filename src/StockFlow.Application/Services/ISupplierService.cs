using StockFlow.Application.Dtos.Suppliers;
namespace StockFlow.Application.Services;

public interface ISupplierService
{
    Task<SupplierDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<SupplierDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<SupplierDto> CreateAsync(CreateSupplierDto dto, CancellationToken cancellationToken = default);
    Task<SupplierDto> UpdateAsync(Guid id, UpdateSupplierDto dto, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
