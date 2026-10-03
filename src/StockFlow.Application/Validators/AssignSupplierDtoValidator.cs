using FluentValidation;
using StockFlow.Application.Dtos.Products;

namespace StockFlow.Application.Validators;

public class AssignSupplierDtoValidator : AbstractValidator<AssignSupplierDto>
{
    public AssignSupplierDtoValidator()
    {
        RuleFor(x => x.SupplierId).NotEmpty();
        RuleFor(x => x.PurchasePrice).GreaterThanOrEqualTo(0);
        RuleFor(x => x.LeadTimeDays).GreaterThanOrEqualTo(0);
    }
}
