namespace StockFlow.Application.Dtos.Products;

public record CreateProductDto(string Name, string? Description, decimal UnitPrice, string Unit, Guid? CategoryId);
