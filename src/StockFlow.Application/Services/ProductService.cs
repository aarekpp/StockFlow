using FluentValidation;
using StockFlow.Application.Common;
using StockFlow.Application.Dtos.Products;
using StockFlow.Application.Mapping;
using StockFlow.Domain.Entities;
using StockFlow.Domain.Repositories;

namespace StockFlow.Application.Services;

public class ProductService(
    IUnitOfWork unitOfWork,
    IValidator<CreateProductDto> createValidator,
    IValidator<UpdateProductDto> updateValidator,
    IValidator<AssignSupplierDto> assignSupplierValidator) : IProductService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IValidator<CreateProductDto> _createValidator = createValidator;
    private readonly IValidator<UpdateProductDto> _updateValidator = updateValidator;
    private readonly IValidator<AssignSupplierDto> _assignSupplierValidator;

    public async Task<ProductDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return (await GetExistingAsync(id, cancellationToken)).ToDto();
    }

    public async Task<IReadOnlyList<ProductDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var products = await _unitOfWork.Products.GetAllAsync(cancellationToken);
        return products.Select(p => p.ToDto()).ToList();
    }

    public async Task<IReadOnlyList<ProductDto>> GetBelowStockLevelAsync(int threshold, CancellationToken cancellationToken = default)
    {
        var products = await _unitOfWork.Products.GetBelowStockLevelAsync(threshold, cancellationToken);
        return products.Select(p => p.ToDto()).ToList();
    }

    public async Task<ProductDto> CreateAsync(CreateProductDto dto, CancellationToken cancellationToken = default)
    {
        await Guard.ValidateAsync(_createValidator, dto, cancellationToken);
        if (dto.CategoryId.HasValue) await EnsureCategoryExistsAsync(dto.CategoryId.Value, cancellationToken);

        var product = new Product(dto.Name, dto.UnitPrice, dto.Unit, dto.Description, dto.CategoryId);

        await _unitOfWork.Products.AddAsync(product, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return product.ToDto() with { CategoryName = dto.CategoryId.HasValue ? (await _unitOfWork.Categories.GetByIdAsync(dto.CategoryId.Value, cancellationToken))?.Name : null };
    }

    public async Task<ProductDto> UpdateAsync(Guid id, UpdateProductDto dto, CancellationToken cancellationToken = default)
    {
        await Guard.ValidateAsync(_updateValidator, dto, cancellationToken);
        if (dto.CategoryId.HasValue) await EnsureCategoryExistsAsync(dto.CategoryId.Value, cancellationToken);

        var product = await GetExistingAsync(id, cancellationToken);
        product.Update(dto.Name, dto.UnitPrice, dto.Unit, dto.Description);
        product.ChangeCategory(dto.CategoryId);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var reloaded = await GetExistingAsync(id, cancellationToken);
        return reloaded.ToDto();
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var product = await GetExistingAsync(id, cancellationToken);
        _unitOfWork.Products.Remove(product);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<ProductSupplierDto>> GetSuppliersAsync(Guid productId, CancellationToken cancellationToken = default)
    {
        var product = await _unitOfWork.Products.GetWithSuppliersAsync(productId, cancellationToken) ?? throw new NotFoundException(nameof(Product), productId);
        return product.ProductSuppliers.Select(ps => ps.ToDto()).ToList();
    }

    private async Task<Product> GetExistingAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _unitOfWork.Products.GetByIdAsync(id, cancellationToken)
            ?? throw new NotFoundException(nameof(Product), id);
    }

    public async Task<ProductSupplierDto> AssignSupplierAsync(Guid productId, AssignSupplierDto dto, CancellationToken cancellationToken = default)
    {
        await Guard.ValidateAsync(_assignSupplierValidator, dto, cancellationToken);
        var supplier = await _unitOfWork.Suppliers.GetByIdAsync(dto.SupplierId, cancellationToken) ?? throw new NotFoundException(nameof(Supplier), dto.SupplierId);
        var product = await _unitOfWork.Products.GetWithSuppliersAsync(productId, cancellationToken) ?? throw new NotFoundException(nameof(Product), productId);
        var productSupplier = product.AddSupplier(dto.SupplierId, dto.PurchasePrice, dto.LeadTimeDays);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return new ProductSupplierDto(supplier.Id, supplier.Name, productSupplier.PurchasePrice, productSupplier.LeadTimeDays);
    }

    private async Task EnsureCategoryExistsAsync(Guid categoryId, CancellationToken cancellationToken)
    {
        var category = await _unitOfWork.Categories.GetByIdAsync(categoryId, cancellationToken);
        if (category is null) throw new NotFoundException(nameof(Category), categoryId);
    }
}
