using StockFlow.Domain.Common;
using StockFlow.Domain.Enums;
using StockFlow.Domain.Exceptions;

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
        Unit = unit.Trim();
        Description= description?.Trim();
        UnitPrice = unitPrice;
    }

    public void ChangeCategory(Guid? categoryId = null)
    {
        CategoryId = categoryId;
        Category = null;
    }

    public StockMovement RegisterStockMovement(StockMovementType type, int quantity, DateTime occurredAt, Guid? orderId = null, string? comment = null)
    {
        if(type == StockMovementType.Proofreading)
        {
            if (quantity == 0) throw new ArgumentOutOfRangeException(nameof(quantity), "Adjustment quantity cannot be zero.");
        }
        else
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(quantity);
        }

        var delta = type switch
        {
            StockMovementType.Receipt => quantity,
            StockMovementType.Issue => -quantity,
            StockMovementType.Proofreading => quantity,
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, "Unknown stock movement type.")
        };

        if(StockQuantity + delta < 0) throw new DomainException($"Insufficient stock for product '{Name}'. Available: {StockQuantity}, requested change: {delta}.");

        StockQuantity += delta;
        var movement = new StockMovement(Id, type, quantity, occurredAt, orderId, comment);
        _stockMovements.Add(movement);
        return movement;
    }

    public ProductSupplier AddSupplier(Guid supplierId, decimal purchasePrice, int leadTimeDays)
    {
        if(_productSuppliers.Any(ps => ps.SupplierId == supplierId)) throw new DomainException("This supplier is already assigned to the product.");

        var productSupplier = new ProductSupplier(Id, supplierId, purchasePrice, leadTimeDays);
        _productSuppliers.Add(productSupplier);
        return productSupplier;
    }

    public void UpdateSupplierTerms(Guid supplierId, decimal purchasePrice, int leadTimeDays) => GetProductSupplier(supplierId).UpdateTerms(purchasePrice, leadTimeDays);

    public void RemoveSupplier(Guid supplierId) => _productSuppliers.Remove(GetProductSupplier(supplierId));

    private ProductSupplier GetProductSupplier(Guid supplierId)
    {
        return _productSuppliers.FirstOrDefault(ps => ps.SupplierId == supplierId) ?? throw new DomainException("This supplier is not assigned to the product.");
    }
}
