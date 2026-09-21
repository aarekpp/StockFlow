using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StockFlow.Domain.Entities;

namespace StockFlow.Infrastructure.Persistence.Configurations;

internal sealed class CategoryConfiguration : EntityConfiguration<Category>
{
    protected override void ConfigureEntity(EntityTypeBuilder<Category> builder)
    {
        builder.ToTable("Categories");
        builder.Property(c => c.Name).IsRequired().HasMaxLength(100);
        builder.Property(c => c.Description).HasMaxLength(500);
        builder.HasIndex(c => c.Name).IsUnique();
        builder.Navigation(c => c.Products).UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}
