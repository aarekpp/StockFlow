namespace StockFlow.Application.Dtos.Products;

public record AssignSupplierDto(Guid SupplierId, decimal PurchasePrice, int LeadTimeDays);
