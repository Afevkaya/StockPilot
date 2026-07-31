using StockPilot.Application.Abstractions.Persistence.Commands;

namespace StockPilot.Application.Features.Products.Commands.DeleteProduct;

public class DeleteProductHandler(IProductCommandRepository productCommandRepository)
{
    public async Task Handle(DeleteProductCommand command, CancellationToken cancellationToken)
    {
        var product = await productCommandRepository.GetByIdAsync(command.Id, cancellationToken);
        if (product == null)
        {
            throw new InvalidOperationException($"Ürün bulunamadı. Id: {command.Id}");
        }

        await productCommandRepository.DeleteAsync(command.Id, cancellationToken);
    }
}
