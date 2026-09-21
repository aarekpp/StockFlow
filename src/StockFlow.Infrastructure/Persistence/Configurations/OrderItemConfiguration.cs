using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StockFlow.Domain.Entities;

namespace StockFlow.Infrastructure.Persistence.Configurations;

internal sealed class OrderItemConfiguration : EntityConfiguration<OrderItem>
{
    protected override void ConfigureEntity(EntityTypeBuilder<OrderItem> builder)
    {
        builder.ToTable("OrderItems", t =>
        {
            t.HasCheckConstraint("CK_OrderItems_Quantity_Positive", "[Quantity] > 0");
            t.HasCheckConstraint("CK_OrderItems_UnitPriceAtOrderTime_NonNegative", "[UnitPriceAtOrderTime] >= 0");
        });

        builder.Property(i => i.Quantity).IsRequired();
        builder.Property(i => i.UnitPriceAtOrderTime).HasPrecision(18, 2);
        builder.Ignore(i => i.LineTotal);
        builder.HasIndex(i => new { i.OrderId, i.ProductId });

        builder.HasOne(i => i.Order).WithMany(o => o.OrderItems).HasForeignKey(i => i.OrderId).OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(i => i.Product).WithMany(p => p.OrderItems).HasForeignKey(i => i.ProductId).OnDelete(DeleteBehavior.Restrict);
    }
}
