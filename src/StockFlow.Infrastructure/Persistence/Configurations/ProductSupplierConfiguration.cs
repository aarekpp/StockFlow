using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StockFlow.Domain.Entities;

namespace StockFlow.Infrastructure.Persistence.Configurations;

internal sealed class ProductSupplierConfiguration : IEntityTypeConfiguration<ProductSupplier>
{
    public void Configure(EntityTypeBuilder<ProductSupplier> builder)
    {
        builder.ToTable("ProductSuppliers", t =>
        {
            t.HasCheckConstraint("CK_ProductSuppliers_PurchasePrice_NonNegative", "[PurchasePrice] >= 0");
            t.HasCheckConstraint("CK_ProductSuppliers_LeadTimeDays_NonNegative", "[LeadTimeDays] >= 0");
        });

        builder.HasKey(ps => new { ps.ProductId, ps.SupplierId });
        builder.Property(ps => ps.PurchasePrice).HasPrecision(18, 2);

        builder.HasOne(ps => ps.Product).WithMany(p => p.ProductSuppliers).HasForeignKey(ps => ps.ProductId).OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(ps => ps.Supplier).WithMany(s => s.ProductSuppliers).HasForeignKey(ps => ps.SupplierId).OnDelete(DeleteBehavior.Cascade);
    }
}
