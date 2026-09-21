using StockFlow.Domain.Common;

namespace StockFlow.Domain.Entities;

public sealed class Product : BaseEntity
{
    private readonly List<ProductSupplier> _productSuppliers = [];
    private readonly List<OrderItem> _orderItems = [];
    private readonly List<StockMovement> _stockMovements = [];

    public string Name { get; private set; } = string.Empty;
    public string? Description { get; private set; }
    public decimal UnitPrice { get; private set; }
    public string Unit { get; private set; } = string.Empty;
    public int StockQuantity { get; private set; }
    public Guid? CategoryId { get; private set; }
    public Category? Category { get; private set; }

    public IReadOnlyCollection<ProductSupplier> ProductSuppliers => _productSuppliers.AsReadOnly();
    public IReadOnlyCollection<OrderItem> OrderItems => _orderItems.AsReadOnly();
    public IReadOnlyCollection<StockMovement> StockMovements => _stockMovements.AsReadOnly();

    private Product () { }

    public Product(string name, decimal unitPrice, string unit, string? description = null, Guid? categoryId = null)
    {
        Update(name, unitPrice, unit, description);
        CategoryId = categoryId;
    }

    public void Update(string name, decimal unitPrice, string unit, string? description = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentException.ThrowIfNullOrWhiteSpace(unit);
        ArgumentOutOfRangeException.ThrowIfNegative(unitPrice);

        Name = name.Trim();
        Unit = Unit.Trim();
        Description= description?.Trim();
        UnitPrice = unitPrice;
    }

    public void ChangeCategory(Guid? categoryId = null)
    {
        CategoryId = categoryId;
        Category = null;
    }
}
