namespace StockFlow.Application.Dtos.Suppliers;

public record CreateSupplierDto(string Name, string Address, string PhoneNumber, string Email, string? TaxId);
