using StockFlow.Desktop.Models;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;

namespace StockFlow.Desktop.Services;

public class ApiClient
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    private readonly HttpClient _httpClient;

    public ApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IReadOnlyList<ProductDto>> GetProductsAsync(CancellationToken cancellationToken = default)
    {
        using var response = await _httpClient.GetAsync("api/products", cancellationToken);
        await EnsureSuccessAsync(response, cancellationToken);
        var products = await response.Content.ReadFromJsonAsync<List<ProductDto>>(JsonOptions, cancellationToken);
        return products ?? new List<ProductDto>();
    }

    private static async Task EnsureSuccessAsync(HttpResponseMessage response, CancellationToken cancellationToken)
    {
        if (response.IsSuccessStatusCode) return;
        var body = await response.Content.ReadAsStringAsync(cancellationToken);
        throw new ApiException((int)response.StatusCode, body);
    }
}
