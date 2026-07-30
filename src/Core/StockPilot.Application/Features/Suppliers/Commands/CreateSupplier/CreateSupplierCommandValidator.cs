using FluentValidation;

namespace StockPilot.Application.Features.Suppliers.Commands.CreateSupplier;

public class CreateSupplierCommandValidator : AbstractValidator<CreateSupplierCommand>
{
    public CreateSupplierCommandValidator()
    {
        RuleFor(command => command.Name)
            .NotEmpty().WithMessage("Tedarikçi ismi zorunludur");
        RuleFor(command => command.Email)
            .EmailAddress().WithMessage("Geçerli bir e-posta adresi giriniz")
            .When(command => !string.IsNullOrEmpty(command.Email));
        RuleFor(command => command.Phone)
            .Matches(@"^\+?\d{10,15}$").WithMessage("Geçerli bir telefon numarası giriniz")
            .When(command => !string.IsNullOrEmpty(command.Phone));
        RuleFor(command => command.Address)
            .MaximumLength(200).WithMessage("Adres 200 karakterden uzun olamaz")
            .When(command => !string.IsNullOrEmpty(command.Address));
        RuleFor(command => command.ContactName)
            .MaximumLength(100).WithMessage("İletişim ismi 100 karakterden uzun olamaz")
            .When(command => !string.IsNullOrEmpty(command.ContactName));
    }
}
