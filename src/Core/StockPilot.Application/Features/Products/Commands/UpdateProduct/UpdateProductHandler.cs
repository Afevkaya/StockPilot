using StockPilot.Application.Abstractions.Persistence.Commands;
using StockPilot.Domain.Entities;

namespace StockPilot.Application.Features.Products.Commands.UpdateProduct;

public class UpdateProductHandler(IProductCommandRepository productCommandRepository)
{
    public async Task<UpdateProductResponse> HandleAsync(UpdateProductCommand command, CancellationToken cancellationToken)
    {
        Product? product = await productCommandRepository.GetByIdAsync(command.Id, cancellationToken);

        if (product == null)
        {
            throw new KeyNotFoundException($"Product bulunamadı. Id: {command.Id}");
        }

        product.Update(command.Name, command.Description, command.PurchasePrice, command.SalePrice);

        await productCommandRepository.UpdateAsync(product, cancellationToken);

        return new UpdateProductResponse(
            product.Id,
            product.Name,
            product.Description,
            product.PurchasePrice,
            product.SalePrice
        );
    }
}
