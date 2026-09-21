using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StockFlow.Domain.Entities;

namespace StockFlow.Infrastructure.Persistence.Configurations{
    internal sealed class ProductConfiguration : EntityConfiguration<Product>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Products", t =>
            {
                t.HasCheckConstraint("CK_Products_UnitPrice_NonNegative", "[UnitPrice] >= 0");
                t.HasCheckConstraint("CK_Products_StockQuantity_NonNegative", "[StockQuantity] >= 0");
            });

            builder.Property(p => p.Name).IsRequired().HasMaxLength(255);
            builder.Property(p => p.Description).HasMaxLength(1000);
            builder.Property(p => p.Unit).IsRequired().HasMaxLength(20);
            builder.Property(p => p.UnitPrice).HasPrecision(18, 2);

            builder.HasOne(p => p.Category).WithMany(c => c.Products).HasForeignKey(p => p.CategoryId).OnDelete(DeleteBehavior.SetNull);

            builder.Navigation(p => p.ProductSuppliers).UsePropertyAccessMode(PropertyAccessMode.Field);
            builder.Navigation(p => p.OrderItems).UsePropertyAccessMode(PropertyAccessMode.Field);
            builder.Navigation(p => p.StockMovements).UsePropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}
