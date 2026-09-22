using StockFlow.Application.Dtos.Suppliers;
using StockFlow.Domain.Entities;

namespace StockFlow.Application.Mapping;

internal static class SupplierMappingExtensions
{
    public static SupplierDto ToDto(this Supplier supplier)
    {
        return new SupplierDto(supplier.Id, supplier.Name, supplier.TaxId, supplier.Address, supplier.PhoneNumber, supplier.Email);
    }
}