namespace StockFlow.Application.Dtos.Suppliers;

public record SupplierDto(Guid Id, string Name, string? TaxId, string Address, string PhoneNumber, string Email);
