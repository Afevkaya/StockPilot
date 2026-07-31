using FluentValidation;

namespace StockPilot.Application.Features.ProductSuppliers.Commands.AssignSupplier;

public class AssignSupplierCommandValidator : AbstractValidator<AssignSupplierCommand>
{
    public AssignSupplierCommandValidator()
    {
        RuleFor(c => c.ProductId)
            .NotEmpty().WithMessage("Ürün Id boş olamaz");
        RuleFor(c => c.SupplierId)
            .NotEmpty().WithMessage("Tedarikçi Id boş olamaz");
    }
}
