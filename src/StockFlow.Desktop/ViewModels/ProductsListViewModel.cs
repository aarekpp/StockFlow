using StockFlow.Desktop.Common;
using StockFlow.Desktop.Services;
using System.Collections.ObjectModel;

namespace StockFlow.Desktop.ViewModels;

public class ProductsListViewModel(ApiClient apiClient) : ViewModelBase
{
    private readonly ApiClient _apiClient = apiClient;
    private bool _isLoading;
    private string? _errorMessage;

    public ObservableCollection<ProductViewModel> Products { get; } = [];

    public bool IsLoading
    {
        get => _isLoading;
        private set => SetProperty(ref _isLoading, value);
    }

    public string? ErrorMessage
    {
        get => _errorMessage;
        private set => SetProperty(ref _errorMessage, value);
    }

    public async Task LoadProductsAsync()
    {
        IsLoading = true;
        ErrorMessage = null;

        try
        {
            var products = await _apiClient.GetProductsAsync();
            Products.Clear();
            foreach (var product in products)
            {
                Products.Add(new ProductViewModel(product));
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Failed to load products: {ex.Message}";
        }
        finally
        {
            IsLoading = false;
        }
    }
}
