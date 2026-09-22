namespace StockFlow.Application.Common;

public class NotFoundException(string entityName, object key) : Exception($"{entityName} with key '{key}' was not found.")
{
    public string EntityName { get; } = entityName;
    public object Key { get; } = key;
}
