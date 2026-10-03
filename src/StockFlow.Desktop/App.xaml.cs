using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using StockFlow.Desktop.Services;
using StockFlow.Desktop.ViewModels;
using System.Windows;

namespace StockFlow.Desktop;

public partial class App : Application
{
    private readonly IHost _host;

    public App()
    {
        _host = Host.CreateDefaultBuilder()
            .ConfigureAppConfiguration(config =>
            {
                config.SetBasePath(AppContext.BaseDirectory);
                config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: false);
            })
            .ConfigureServices((context, services) =>
            {
                var apiBaseUrl = context.Configuration["Api:BaseUrl"] ?? throw new InvalidOperationException("Configuration value 'Api:BaseUrl' is missing from appsettings.json.");

                services.AddHttpClient<ApiClient>(client =>
                {
                    client.BaseAddress = new Uri(apiBaseUrl);
                });

                services.AddTransient<ProductsListViewModel>();
                services.AddTransient<MainWindow>();
            })
            .Build();
    }

    protected override async void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
        await _host.StartAsync();
        var mainWindow = _host.Services.GetRequiredService<MainWindow>();
        mainWindow.Show();
    }

    protected override async void OnExit(ExitEventArgs e)
    {
        await _host.StopAsync();
        _host.Dispose();
        base.OnExit(e);
    }
}

