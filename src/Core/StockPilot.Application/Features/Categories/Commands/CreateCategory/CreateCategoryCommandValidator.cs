using FluentValidation;

namespace StockPilot.Application.Features.Categories.Commands.CreateCategory;

public class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
{
    public CreateCategoryCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Kategori ismi zornuludur")
            .MaximumLength(100).WithMessage("Kategori ismi en fazla 100 karakter olmalıdır");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Kategori açıklaması zornuludur")
            .MaximumLength(500).WithMessage("Kategori açıklaması en fazla 500 karakter olmalıdır");
    }
}
