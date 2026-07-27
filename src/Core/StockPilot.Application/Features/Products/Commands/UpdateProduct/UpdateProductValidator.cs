using FluentValidation;

namespace StockPilot.Application.Features.Products.Commands.UpdateProduct;

public class UpdateProductValidator : AbstractValidator<UpdateProductCommand>
{
    public UpdateProductValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Ürün adı boş olamaz.")
            .MaximumLength(100).WithMessage("Ürün adı en fazla 100 karakter olabilir.");

        RuleFor(x => x.Description)
            .MaximumLength(500).WithMessage("Ürün açıklaması en fazla 500 karakter olabilir.");

        RuleFor(x => x.PurchasePrice)
            .GreaterThan(0).WithMessage("Alış fiyatı sıfırdan büyük olmalıdır.");

        RuleFor(x => x.SalePrice)
            .GreaterThan(0).WithMessage("Satış fiyatı sıfırdan büyük olmalıdır.");

        RuleFor(x => x.CategoryId)
            .NotEmpty().WithMessage("Kategori Id boş olamaz.");
    }
}
