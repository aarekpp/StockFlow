using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StockFlow.Domain.Entities;

namespace StockFlow.Infrastructure.Persistence.Configurations;

internal sealed class OrderConfiguration : EntityConfiguration<Order>
{
    protected override void ConfigureEntity(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("Orders");
        builder.Property(o => o.Status).IsRequired().HasConversion<string>().HasMaxLength(20);
        builder.Property(o => o.PlacedAt).IsRequired();
        builder.Property(o => o.Notes).HasMaxLength(1000);

        builder.HasOne(o => o.Customer).WithMany(c => c.Orders).HasForeignKey(o => o.CustomerId).IsRequired().OnDelete(DeleteBehavior.Restrict);

        builder.Navigation(o => o.OrderItems).UsePropertyAccessMode(PropertyAccessMode.Field);
        builder.Navigation(o => o.StockMovements).UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}
