using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StockFlow.Domain.Entities;

namespace StockFlow.Infrastructure.Persistence.Configurations;

internal sealed class StockMovementConfiguration : EntityConfiguration<StockMovement>
{
    protected override void ConfigureEntity(EntityTypeBuilder<StockMovement> builder)
    {
        builder.ToTable("StockMovements", t =>
        {
            t.HasCheckConstraint("CK_StockMovements_Quantity_NonZero", "[Quantity] <> 0");
        });

        builder.Property(m => m.Type).IsRequired().HasConversion<string>().HasMaxLength(20);
        builder.Property(m => m.Quantity).IsRequired();
        builder.Property(m => m.OccurredAt).IsRequired();
        builder.Property(m => m.Comment).HasMaxLength(500);

        builder.HasOne(m => m.Product).WithMany(p => p.StockMovements).HasForeignKey(m => m.ProductId).IsRequired().OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(m => m.Order).WithMany(o => o.StockMovements).HasForeignKey(m => m.OrderId).IsRequired(false).OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(m => new { m.ProductId, m.OccurredAt });
    }
}
