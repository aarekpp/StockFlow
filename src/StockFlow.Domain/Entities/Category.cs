using StockFlow.Domain.Common;

namespace StockFlow.Domain.Entities;

public sealed class Category : BaseEntity
{
    private readonly List<Product> _products = [];

    public string Name { get; private set; } = string.Empty;
    public string? Description { get; private set; }
    public IReadOnlyCollection<Product> Products => _products.AsReadOnly();

    private Category() { }

    public Category(string name, string? description = null) => Update(name, description);

    public void Update(string newName, string? newDescription = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(newName);

        Name = newName;
        Description = newDescription?.Trim();
    }
}
