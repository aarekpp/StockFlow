using StockFlow.Application.Dtos.Customers;
using StockFlow.Domain.Entities;

namespace StockFlow.Application.Mapping;

internal static class CustomerMappingExtensions
{
    public static CustomerDto ToDto(this Customer customer)
    {
        return new CustomerDto(customer.Id, customer.Type, customer.Name, customer.TaxId, customer.Address, customer.Email, customer.PhoneNumber);
    }
}