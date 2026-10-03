using StockFlow.Domain.Enums;

namespace StockFlow.Application.Dtos.Customers;

public record CreateCustomerDto(CustomerType Type, string Name, string Address, string Email, string PhoneNumber, string? TaxId);
