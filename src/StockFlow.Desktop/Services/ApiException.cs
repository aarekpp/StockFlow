namespace StockFlow.Desktop.Services;

public class ApiException(int statusCode, string responseBody) : Exception($"The API returned status code {statusCode}.")
{
    public int StatusCode { get; } = statusCode;
    public string ResponseBody { get; } = responseBody;
}
