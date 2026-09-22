using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StockFlow.Domain.Entities;

namespace StockFlow.Infrastructure.Persistence.Configurations;

internal sealed class SupplierConfiguration : EntityConfiguration<Supplier>
{
    protected override void ConfigureEntity(EntityTypeBuilder<Supplier> builder)
    {
        builder.ToTable("Suppliers");
        builder.Property(s => s.Name).IsRequired().HasMaxLength(200);
        builder.Property(s => s.TaxId).HasMaxLength(20);
        builder.Property(s => s.Address).IsRequired().HasMaxLength(300);
        builder.Property(s => s.PhoneNumber).IsRequired().HasMaxLength(30);
        builder.Property(s => s.Email).IsRequired().HasMaxLength(254);

        builder.Navigation(s => s.ProductSuppliers).UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}
