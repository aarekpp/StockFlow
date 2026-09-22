namespace StockFlow.Application.Dtos.Products;

public record UpdateProductDto(string Name, string? Description, decimal UnitPrice, string Unit, Guid? CategoryId);
