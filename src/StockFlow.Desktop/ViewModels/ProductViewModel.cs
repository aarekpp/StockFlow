using StockFlow.Desktop.Common;
using StockFlow.Desktop.Models;

namespace StockFlow.Desktop.ViewModels;

public class ProductViewModel(ProductDto product) : ViewModelBase
{
    private readonly ProductDto _product = product;

    public Guid Id => _product.Id;
    public string Name => _product.Name;
    public string? Description => _product.Description;
    public decimal UnitPrice => _product.UnitPrice;
    public string Unit => _product.Unit;
    public int StockQuantity => _product.StockQuantity;
    public string CategoryDisplay => _product.CategoryName ?? "(no category)";
}
