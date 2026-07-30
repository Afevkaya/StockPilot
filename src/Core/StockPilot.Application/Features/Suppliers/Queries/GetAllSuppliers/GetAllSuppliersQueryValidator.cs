using FluentValidation;

namespace StockPilot.Application.Features.Suppliers.Queries.GetAllSuppliers;

public class GetAllSuppliersQueryValidator : AbstractValidator<GetAllSuppliersQuery>
{
    public GetAllSuppliersQueryValidator()
    {
        RuleFor(query => query.Page)
            .GreaterThan(0).WithMessage("Sayfa numarası 0'dan büyük olmalıdır.");

        RuleFor(query => query.PageSize)
            .GreaterThan(0).WithMessage("Sayfa boyutu 0'dan büyük olmalıdır.")
            .LessThanOrEqualTo(50).WithMessage("Sayfa boyutu 50'den büyük olamaz.");

        RuleFor(query => query.Name)
            .MaximumLength(100).WithMessage("Tedarikçi adı en fazla 100 karakter olmalıdır.")
            .When(query => !string.IsNullOrWhiteSpace(query.Name));

        RuleFor(query => query.SortDirection)
            .Must(direction => direction is null || direction.Equals("asc", StringComparison.OrdinalIgnoreCase) ||
                               direction.Equals("desc", StringComparison.OrdinalIgnoreCase))
            .WithMessage("Sıralama yönü 'asc' veya 'desc' olmalıdır.")
            .When(query => !string.IsNullOrWhiteSpace(query.SortDirection));

        RuleFor(query => query.SortBy)
            .Must(sortBy =>
                sortBy is null ||
                new[] { "name", "email", "createdat", "updatedat" }.Contains(sortBy.ToLower()))
            .WithMessage("Sıralama alanı 'name', 'email', 'createdat' veya 'updatedat' olmalıdır.")
            .When(query => !string.IsNullOrWhiteSpace(query.SortBy));

        RuleFor(query => query.Search)
            .MaximumLength(200)
            .WithMessage("Arama terimi en fazla 200 karakter olmalıdır.")
            .When(query => !string.IsNullOrWhiteSpace(query.Search));
    }
}
