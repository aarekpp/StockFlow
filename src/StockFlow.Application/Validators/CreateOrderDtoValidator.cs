using FluentValidation;
using StockFlow.Application.Dtos.Orders;

namespace StockFlow.Application.Validators;

public class CreateOrderDtoValidator : AbstractValidator<CreateOrderDto>
{
    public CreateOrderDtoValidator()
    {
        RuleFor(x => x.CustomerId).NotEmpty();
        RuleFor(x => x.Notes).MaximumLength(1000);
        RuleFor(x => x.OrderItems).NotEmpty().WithMessage("An order must contain at least one item.");
        RuleForEach(x => x.OrderItems).SetValidator(new CreateOrderItemDtoValidator());
    }
}
