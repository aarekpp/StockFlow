using FluentValidation;
using StockFlow.Application.Common;
using StockFlow.Application.Dtos.Products;
using StockFlow.Application.Mapping;
using StockFlow.Domain.Entities;
using StockFlow.Domain.Repositories;

namespace StockFlow.Application.Services;

public class ProductService : IProductService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<CreateProductDto> _createValidator;
    private readonly IValidator<UpdateProductDto> _updateValidator;

    public ProductService(
        IUnitOfWork unitOfWork,
        IValidator<CreateProductDto> createValidator,
        IValidator<UpdateProductDto> updateValidator)
    {
        _unitOfWork = unitOfWork;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

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

        if (dto.CategoryId.HasValue)
        {
            await EnsureCategoryExistsAsync(dto.CategoryId.Value, cancellationToken);
        }

        var product = new Product(dto.Name, dto.UnitPrice, dto.Unit, dto.Description, dto.CategoryId);

        await _unitOfWork.Products.AddAsync(product, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return product.ToDto() with { CategoryName = dto.CategoryId.HasValue ? (await _unitOfWork.Categories.GetByIdAsync(dto.CategoryId.Value, cancellationToken))?.Name : null };
    }

    public async Task<ProductDto> UpdateAsync(Guid id, UpdateProductDto dto, CancellationToken cancellationToken = default)
    {
        await Guard.ValidateAsync(_updateValidator, dto, cancellationToken);

        if (dto.CategoryId.HasValue)
        {
            await EnsureCategoryExistsAsync(dto.CategoryId.Value, cancellationToken);
        }

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

    public Task<ProductSupplierDto> AssignSupplierAsync(Guid productId, AssignSupplierDto dto, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException("Assigning suppliers to a product is implemented in Stage 5.");
    }

    private async Task<Product> GetExistingAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _unitOfWork.Products.GetByIdAsync(id, cancellationToken)
            ?? throw new NotFoundException(nameof(Product), id);
    }

    private async Task EnsureCategoryExistsAsync(Guid categoryId, CancellationToken cancellationToken)
    {
        var category = await _unitOfWork.Categories.GetByIdAsync(categoryId, cancellationToken);

        if (category is null)
        {
            throw new NotFoundException(nameof(Category), categoryId);
        }
    }
}
