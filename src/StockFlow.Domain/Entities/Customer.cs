using StockFlow.Domain.Common;
using StockFlow.Domain.Enums;
using StockFlow.Domain.Exceptions;

namespace StockFlow.Domain.Entities;

public sealed class Customer : BaseEntity
{
    private readonly List<Order> _orders = [];

    public CustomerType Type { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string? TaxId { get; private set; }
    public string Address { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string PhoneNumber { get; private set; } = string.Empty;

    public IReadOnlyCollection<Order> Orders => _orders.AsReadOnly();

    private Customer() { }

    public Customer(CustomerType type, string name, string address, string email, string phoneNumber, string? taxId = null)
    {
        Type = type;
        Update(name, address, email, phoneNumber, taxId);
    }

    public void Update(string name, string address, string email, string phoneNumber, string? taxId = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentException.ThrowIfNullOrWhiteSpace(address);
        ArgumentException.ThrowIfNullOrWhiteSpace(email);
        ArgumentException.ThrowIfNullOrWhiteSpace(phoneNumber);

        if(Type == CustomerType.Company && string.IsNullOrWhiteSpace(taxId))
        {
            throw new DomainException("Tax ID is required for company customers.");
        }

        Name = name.Trim();
        Address = address.Trim();
        Email = email.Trim();
        PhoneNumber = phoneNumber.Trim();
        TaxId = Type == CustomerType.Company ? taxId!.Trim() : null;
    }
}
