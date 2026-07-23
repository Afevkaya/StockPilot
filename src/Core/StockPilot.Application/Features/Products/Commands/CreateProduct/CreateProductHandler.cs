using FluentValidation;
using StockPilot.Application.Abstractions.Persistence.Commands;
using StockPilot.Domain.Entities;

namespace StockPilot.Application.Features.Products.Commands.CreateProduct;

public class CreateProductHandler(
    IProductCommandRepository productCommandRepository,
    IValidator<CreateProductCommand> validator)
{
    public async Task<CreateProductResponse> HandleAsync(
        CreateProductCommand command,
        CancellationToken cancellationToken = default)
    {
        await validator.ValidateAndThrowAsync(command, cancellationToken);

        var product = new Product(
            command.Name,
            command.Description,
            command.PurchasePrice,
            command.SalePrice);

        await productCommandRepository.AddAsync(product, cancellationToken);

        return new CreateProductResponse(product.Id);
    }
}
