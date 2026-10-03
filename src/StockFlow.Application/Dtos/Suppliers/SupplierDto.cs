namespace StockFlow.Application.Dtos.Suppliers;

public record SupplierDto(Guid id, string Name, string? TaxId, string Address, string PhoneNumber, string Email);
