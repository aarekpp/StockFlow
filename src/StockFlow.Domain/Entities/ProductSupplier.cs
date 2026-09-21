namespace StockFlow.Domain.Entities;

public sealed class ProductSupplier
{
    public Guid ProductId { get; private set; }
    public Guid SupplierId { get; private set; }
    public decimal PurchasePrice { get; private set; }
    public int LeadTimeDays { get; private set; }

    public Product Product { get; private set; } = null!;
    public Supplier Supplier { get; private set; } = null!;

    private ProductSupplier() { }

    internal ProductSupplier(Guid productId, Guid supplierId, decimal purchasePrice, int leadTimeDays)
    {
        ProductId = productId;
        SupplierId = supplierId;
        UpdateTerms(purchasePrice, leadTimeDays);
    }

    internal void UpdateTerms(decimal purchasePrice, int leadTimeDays)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(purchasePrice);
        ArgumentOutOfRangeException.ThrowIfNegative(leadTimeDays);

        PurchasePrice = purchasePrice;
        LeadTimeDays = leadTimeDays;
    }
}
