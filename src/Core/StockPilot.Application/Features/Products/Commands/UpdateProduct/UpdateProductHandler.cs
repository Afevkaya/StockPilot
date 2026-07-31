using StockPilot.Application.Abstractions.Persistence.Commands;
using StockPilot.Domain.Entities;

namespace StockPilot.Application.Features.Products.Commands.UpdateProduct;

public class UpdateProductHandler(
    IProductCommandRepository productCommandRepository,
    ICategoryCommandRepository categoryCommandRepository)
{
    public async Task<UpdateProductResponse> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
    {
        Product? product = await productCommandRepository.GetByIdAsync(command.Id, cancellationToken);
        if (product == null)
        {
            throw new KeyNotFoundException($"Product bulunamadı. Id: {command.Id}");
        }

        Category? category = await categoryCommandRepository.GetByIdAsync(command.CategoryId, cancellationToken);
        if (category == null)
        {
            throw new InvalidOperationException($"Kategori bulunamadı. Id: {command.CategoryId}");
        }

        product.Update(command.Name, command.Description, command.PurchasePrice, command.SalePrice, command.CategoryId);

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
