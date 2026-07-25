using FluentValidation;

namespace StockPilot.Application.Features.Products.Queries.GetProducts;

public class GetProductsQueryValidator : AbstractValidator<GetProductsQuery>
{
    public GetProductsQueryValidator()
    {
        RuleFor(query => query.Page)
            .GreaterThan(0).WithMessage("Sayfa numarası 0'dan büyük olmalıdır.");

        RuleFor(query => query.PageSize)
            .GreaterThan(0).WithMessage("Sayfa boyutu 0'dan büyük olmalıdır.")
            .LessThanOrEqualTo(50).WithMessage("Sayfa boyutu 50'den büyük olamaz.");

        RuleFor(query => query.Name)
            .MaximumLength(100).WithMessage("Ürün adı en fazla 100 karakter olmalıdır.")
            .When(query => !string.IsNullOrWhiteSpace(query.Name));

        RuleFor(query => query.MinSalePrice)
            .GreaterThanOrEqualTo(0).WithMessage("Minimum satış fiyatı 0 veya daha büyük olmalıdır.")
            .When(query => query.MinSalePrice is not null);

        RuleFor(query => query.MaxSalePrice)
            .GreaterThanOrEqualTo(0).WithMessage("Maksimum satış fiyatı 0 veya daha büyük olmalıdır.")
            .When(query => query.MaxSalePrice is not null);

        RuleFor(query => query)
            .Must(query => query.MinSalePrice is null || query.MaxSalePrice is null ||
                           query.MinSalePrice!.Value <= query.MaxSalePrice!.Value)
            .WithMessage("Minimum satış fiyatı maksimum satış fiyatından büyük olamaz.")
            .When(query => query.MinSalePrice is not null && query.MaxSalePrice is not null);

        RuleFor(query => query.SortDirection)
            .Must(direction => direction is null || direction.Equals("asc", StringComparison.OrdinalIgnoreCase) ||
                               direction.Equals("desc", StringComparison.OrdinalIgnoreCase))
            .WithMessage("Sıralama yönü 'asc' veya 'desc' olmalıdır.")
            .When(query => !string.IsNullOrWhiteSpace(query.SortDirection));

        RuleFor(query => query.SortBy)
            .Must(sortBy =>
                sortBy is null ||
                new[] { "name", "saleprice", "purchaseprice", "createdat" }.Contains(sortBy.ToLower()))
            .WithMessage("Sıralama alanı 'name', 'saleprice', 'purchaseprice' veya 'createdat' olmalıdır.")
            .When(query => !string.IsNullOrWhiteSpace(query.SortBy));

        RuleFor(query => query.Search)
            .MaximumLength(200)
            .WithMessage("Arama terimi en fazla 200 karakter olmalıdır.")
            .When(query => !string.IsNullOrWhiteSpace(query.Search));
    }
}
