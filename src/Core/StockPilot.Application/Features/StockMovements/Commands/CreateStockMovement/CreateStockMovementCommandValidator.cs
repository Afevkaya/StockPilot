using FluentValidation;

namespace StockPilot.Application.Features.StockMovements.Commands.CreateStockMovement;

public class CreateStockMovementCommandValidator : AbstractValidator<CreateStockMovementCommand>
{
    public CreateStockMovementCommandValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty()
            .WithMessage("Geçerli bir ürün ID'si girin");

        RuleFor(x => x.Quantity)
            .GreaterThan(0)
            .WithMessage("Miktar sıfırdan büyük olmalıdır");

        RuleFor(x => x.MovementType)
            .IsInEnum()
            .WithMessage("Geçerli bir hareket tipi girin");
    }
}
