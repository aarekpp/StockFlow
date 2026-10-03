namespace StockFlow.Application.Dtos.Categories;

public record CategoryDto(Guid Id, string Name, string? Description, int ProductCount);
