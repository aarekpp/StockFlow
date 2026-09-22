using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StockFlow.Domain.Entities;

namespace StockFlow.Infrastructure.Persistence.Configurations;

internal sealed class CustomerConfiguration : EntityConfiguration<Customer>
{
    protected override void ConfigureEntity(EntityTypeBuilder<Customer> builder)
    {
        builder.ToTable("Customers");
        builder.Property(c => c.Type).IsRequired().HasConversion<string>().HasMaxLength(20);
        builder.Property(c => c.Name).IsRequired().HasMaxLength(200);
        builder.Property(c => c.TaxId).HasMaxLength(20);
        builder.Property(c => c.Address).IsRequired().HasMaxLength(300);
        builder.Property(c => c.Email).IsRequired().HasMaxLength(254);
        builder.Property(c => c.PhoneNumber).IsRequired().HasMaxLength(30);
        builder.HasIndex(c => c.TaxId).IsUnique().HasFilter("[TaxId] IS NOT NULL");

        builder.Navigation(c => c.Orders).UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}
