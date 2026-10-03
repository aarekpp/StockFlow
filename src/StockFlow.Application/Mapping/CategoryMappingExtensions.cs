using StockFlow.Application.Dtos.Categories;
using StockFlow.Domain.Entities;

namespace StockFlow.Application.Mapping;

internal static class CategoryMappingExtensions
{
    public static CategoryDto ToDto(this Category category, int productCount)
    {
        return new CategoryDto(category.Id, category.Name, category.Description, productCount);
    }
}