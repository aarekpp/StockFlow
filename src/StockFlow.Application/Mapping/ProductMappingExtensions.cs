using StockFlow.Application.Dtos.Products;
using StockFlow.Domain.Entities;

namespace StockFlow.Application.Mapping;

internal static class ProductMappingExtensions
{
    public static ProductDto ToDto(this Product product)
    {
        return new ProductDto(product.Id, product.Name, product.Description, product.UnitPrice, product.Unit, product.StockQuantity, product.CategoryId, product.Category?.Name);
    }

    public static ProductSupplierDto ToDto(this ProductSupplier productSupplier)
    {
        return new ProductSupplierDto(productSupplier.SupplierId, productSupplier.Supplier.Name, productSupplier.PurchasePrice, productSupplier.LeadTimeDays);
    }
}