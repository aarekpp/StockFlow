namespace StockFlow.Application.Dtos.Products;

public record ProductDto(Guid Id, string Name, string? Description, decimal UnitPrice, string Unit, int StockQuantity, Guid? CategoryId, string? CategoryName);