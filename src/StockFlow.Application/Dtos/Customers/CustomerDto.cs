using StockFlow.Domain.Enums;

namespace StockFlow.Application.Dtos.Customers;

public record CustomerDto(Guid Id, CustomerType Type, string Name, string? TaxId, string Address, string Email, string PhoneNumber);