using FluentValidation;

namespace StockFlow.Application.Common;

internal static class Guard
{
    public static async Task ValidateAsync<T>(IValidator<T> validator, T instance, CancellationToken cancellationToken)
    {
        var result = await validator.ValidateAsync(instance, cancellationToken);
        if (!result.IsValid) throw new ValidationException(result.Errors);
    }
}
