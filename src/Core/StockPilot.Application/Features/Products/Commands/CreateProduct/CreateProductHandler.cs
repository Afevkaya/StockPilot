using FluentValidation;
using StockPilot.Application.Abstractions.Persistence.Commands;
using StockPilot.Domain.Entities;

namespace StockPilot.Application.Features.Products.Commands.CreateProduct;

public class CreateProductHandler(
    IProductCommandRepository productCommandRepository,
    ICategoryCommandRepository categoryCommandRepository,
    IValidator<CreateProductCommand> validator)
{
    public async Task<CreateProductResponse> Handle(
        CreateProductCommand command,
        CancellationToken cancellationToken = default)
    {
        await validator.ValidateAndThrowAsync(command, cancellationToken);

        var category = await categoryCommandRepository.GetByIdAsync(command.CategoryId, cancellationToken);
        if (category == null)
        {
            throw new InvalidOperationException($"Kategori bulunamadı. Id: {command.CategoryId}");
        }

        var product = new Product(
            command.Name,
            command.Description,
            command.PurchasePrice,
            command.SalePrice,
            command.CategoryId);

        await productCommandRepository.AddAsync(product, cancellationToken);

        return new CreateProductResponse(product.Id);
    }
}
