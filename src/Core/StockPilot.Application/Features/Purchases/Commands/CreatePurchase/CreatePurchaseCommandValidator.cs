using FluentValidation;

namespace StockPilot.Application.Features.Purchases.Commands.CreatePurchase;

public class CreatePurchaseCommandValidator : AbstractValidator<CreatePurchaseCommand>
{
    public CreatePurchaseCommandValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty().WithMessage("Ürün ID'si boş olamaz.");

        RuleFor(x => x.SupplierId)
            .NotEmpty().WithMessage("Tedarikçi ID'si boş olamaz.");

        RuleFor(x => x.Quantity)
            .GreaterThan(0).WithMessage("Miktar 0'dan büyük olmalıdır.");

        RuleFor(x => x.UnitPrice)
            .GreaterThanOrEqualTo(0).WithMessage("Alış fiyatı negatif olmamalıdır");

        RuleFor(x => x.PurchaseDate)
            .NotEmpty().WithMessage("Satın alma tarihi boş olamaz.")
            .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("Satın alma tarihi gelecekte olamaz.");
    }
}
