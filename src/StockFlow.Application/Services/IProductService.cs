using StockFlow.Application.Dtos.Products;

namespace StockFlow.Application.Services;

public interface IProductService
{
    Task<ProductDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<ProductDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<ProductDto>> GetBelowStockLevelAsync(int threshold, CancellationToken cancellationToken = default);
    Task<ProductDto> CreateAsync(CreateProductDto dto, CancellationToken cancellationToken = default);
    Task<ProductDto> UpdateAsync(Guid id, UpdateProductDto dto, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<ProductSupplierDto>> GetSuppliersAsync(Guid productId, CancellationToken cancellationToken = default);
    Task<ProductSupplierDto> AssignSupplierAsync(Guid productId, AssignSupplierDto dto, CancellationToken cancellationToken = default);
}
