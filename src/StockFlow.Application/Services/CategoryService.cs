using FluentValidation;
using StockFlow.Application.Common;
using StockFlow.Application.Dtos.Categories;
using StockFlow.Application.Mapping;
using StockFlow.Domain.Entities;
using StockFlow.Domain.Repositories;

namespace StockFlow.Application.Services;

public class CategoryService : ICategoryService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<CreateCategoryDto> _createValidator;
    private readonly IValidator<UpdateCategoryDto> _updateValidator;

    public CategoryService(
        IUnitOfWork unitOfWork,
        IValidator<CreateCategoryDto> createValidator,
        IValidator<UpdateCategoryDto> updateValidator)
    {
        _unitOfWork = unitOfWork;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    public async Task<CategoryDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var category = await GetExistingAsync(id, cancellationToken);

        return category.ToDto(await CountProductsAsync(category.Id, cancellationToken));
    }

    public async Task<IReadOnlyList<CategoryDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var categories = await _unitOfWork.Categories.GetAllAsync(cancellationToken);
        var products = await _unitOfWork.Products.GetAllAsync(cancellationToken);

        var countsByCategory = products
            .Where(p => p.CategoryId.HasValue)
            .GroupBy(p => p.CategoryId!.Value)
            .ToDictionary(g => g.Key, g => g.Count());

        return categories
            .Select(c => c.ToDto(countsByCategory.GetValueOrDefault(c.Id)))
            .ToList();
    }

    public async Task<CategoryDto> CreateAsync(CreateCategoryDto dto, CancellationToken cancellationToken = default)
    {
        await Guard.ValidateAsync(_createValidator, dto, cancellationToken);

        var category = new Category(dto.Name, dto.Description);

        await _unitOfWork.Categories.AddAsync(category, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return category.ToDto(productCount: 0);
    }

    public async Task<CategoryDto> UpdateAsync(Guid id, UpdateCategoryDto dto, CancellationToken cancellationToken = default)
    {
        await Guard.ValidateAsync(_updateValidator, dto, cancellationToken);

        var category = await GetExistingAsync(id, cancellationToken);

        category.Update(dto.Name, dto.Description);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return category.ToDto(await CountProductsAsync(category.Id, cancellationToken));
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var category = await GetExistingAsync(id, cancellationToken);

        _unitOfWork.Categories.Remove(category);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    private async Task<Category> GetExistingAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _unitOfWork.Categories.GetByIdAsync(id, cancellationToken)
            ?? throw new NotFoundException(nameof(Category), id);
    }

    private async Task<int> CountProductsAsync(Guid categoryId, CancellationToken cancellationToken)
    {
        var products = await _unitOfWork.Products.GetAllAsync(cancellationToken);

        return products.Count(p => p.CategoryId == categoryId);
    }
}
