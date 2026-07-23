using FluentValidation;

namespace StockPilot.Application.Features.Products.Commands.CreateProduct;

public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(product => product.Name)
            .NotEmpty().WithMessage("Ürün ismi zorunludur.")
            .MaximumLength(100).WithMessage("Ürün ismi en fazla 100 karakter olmalıdır.");
        RuleFor(product => product.Description)
            .MaximumLength(500).WithMessage("Ürün açıklaması en fazla 500 karakter olmalıdır.");
        RuleFor(product => product.PurchasePrice)
            .GreaterThanOrEqualTo(0).WithMessage("Alış fiyatı sıfırdan büyük veya eşit olmalıdır.");
        RuleFor(product => product.SalePrice)
            .GreaterThanOrEqualTo(0).WithMessage("Satış fiyatı sıfırdan büyük veya eşit olmalıdır.");
    }
}
