using StockFlow.Desktop.ViewModels;
using System.Windows;

namespace StockFlow.Desktop;

public partial class MainWindow : Window
{
    private readonly ProductsListViewModel _viewModel;

    public MainWindow(ProductsListViewModel viewModel)
    {
        InitializeComponent();

        _viewModel = viewModel;
        DataContext = _viewModel;
    }

    private async void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
    {
        await _viewModel.LoadProductsAsync();
    }
}