using FluentValidation.Results;

namespace StockFlow.Application.Common;

public class ValidationException(IEnumerable<ValidationFailure> failures) : Exception("One or more validation errors occurred.")
{
    public IReadOnlyDictionary<string, string[]> Errors { get; } = failures.GroupBy(f => f.PropertyName).ToDictionary(g => g.Key, g => g.Select(f => f.ErrorMessage).ToArray());
}
