using StockPilot.Application.Abstractions.Persistence.Commands;
using StockPilot.Domain.Entities;

namespace StockPilot.Application.Features.Products.CreateProduct;

public class CreateProductHandler(IProductCommandRepository productCommandRepository)
{
    public async Task<CreateProductResponse> HandleAsync(CreateProductCommand command, CancellationToken cancellationToken = default)
    {
        var product = new Product(
            command.Name,
            command.Description,
            command.PurchasePrice,
            command.SalePrice
        );

        await productCommandRepository.AddAsync(product, cancellationToken);
        return new CreateProductResponse(product.Id);
    }
}
