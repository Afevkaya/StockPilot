using FluentValidation;

namespace StockPilot.Application.Features.Sales.Commands.CreateSale;

public class CreateSaleCommandValidator : AbstractValidator<CreateSaleCommand>
{
    public CreateSaleCommandValidator()
    {
        RuleFor(c => c.ProductId)
            .NotEmpty().WithMessage("Ürün zorunludur");
        RuleFor(c => c.Quantity)
            .GreaterThan(0).WithMessage("Miktar 0'dan büyük olmalıdır");
        RuleFor(c => c.UnitPrice)
            .GreaterThanOrEqualTo(0).WithMessage("Birim fiyat 0'dan büyük veya eşit olmalıdır");
        RuleFor(c => c.SaleDate)
            .NotEmpty().WithMessage("Satış tarihi boş olamaz")
            .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("Satış tarihi gelecekte olamaz");
    }
}
