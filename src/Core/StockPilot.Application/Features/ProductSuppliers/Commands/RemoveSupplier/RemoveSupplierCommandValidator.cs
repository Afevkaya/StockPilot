using FluentValidation;

namespace StockPilot.Application.Features.ProductSuppliers.Commands.RemoveSupplier;

public class RemoveSupplierCommandValidator : AbstractValidator<RemoveSupplierCommand>
{
    public RemoveSupplierCommandValidator()
    {
        RuleFor(command => command.ProductId).NotEmpty().WithMessage("Ürün boş olamaz");
        RuleFor(command => command.SupplierId).NotEmpty().WithMessage("tedarikçi boş olamaz");
    }
}
