namespace StockFlow.Application.Dtos.Products;

public record ProductSupplierDto(Guid SupplierId, string SupplierName, decimal PurchasePrice, int LeadTimeDays);