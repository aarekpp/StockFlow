using StockFlow.Domain.Common;

namespace StockFlow.Domain.Entities;

public sealed class Supplier : BaseEntity
{
    private readonly List<ProductSupplier> _productSuppliers = [];

    public string Name { get; private set; } = string.Empty;
    public string? TaxId {  get; private set; }
    public string Address { get; private set; } = string.Empty;
    public string PhoneNumber { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;

    public IReadOnlyCollection<ProductSupplier> ProductSuppliers => _productSuppliers.AsReadOnly();
    
    private Supplier() { }

    public Supplier(string name, string address, string phoneNumber, string email, string? taxId = null) => Update(name, address, phoneNumber, email, taxId);

    public void Update(string name, string address, string phoneNumber, string email, string? taxId = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentException.ThrowIfNullOrWhiteSpace(address);
        ArgumentException.ThrowIfNullOrWhiteSpace(phoneNumber);
        ArgumentException.ThrowIfNullOrWhiteSpace(email);

        Name = name.Trim();
        Address = address.Trim();
        PhoneNumber = phoneNumber.Trim();
        Email = email.Trim();
        TaxId = string.IsNullOrEmpty(taxId) ? null : taxId.Trim();
    }
}
