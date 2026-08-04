using FluentValidation;

namespace StockPilot.Application.Features.Sales.Queries.GetSales;

public class GetSalesQueryValidator : AbstractValidator<GetSalesQuery>
{
    public GetSalesQueryValidator()
    {
        RuleFor(x => x.Page)
            .GreaterThan(0).WithMessage("Sayfa numarası 0 dan büyük olmalıdır");

        RuleFor(x => x.PageSize)
            .GreaterThan(0).WithMessage("Sayfa boyutu 0 dan büyük olmalıdır")
            .LessThanOrEqualTo(100).WithMessage("Sayfa boyutu en fazla 100 olabilir");
    }
}
