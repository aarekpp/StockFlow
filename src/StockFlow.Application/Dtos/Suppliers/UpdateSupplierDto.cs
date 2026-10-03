namespace StockFlow.Application.Dtos.Suppliers;

public record UpdateSupplierDto(string Name, string Address, string PhoneNumber, string Email, string? TaxId);
