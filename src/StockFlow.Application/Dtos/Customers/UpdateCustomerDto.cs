namespace StockFlow.Application.Dtos.Customers;

public record UpdateCustomerDto(string Name, string Address, string Email, string PhoneNumber, string? TaxId);
