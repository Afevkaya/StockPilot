using Microsoft.AspNetCore.Mvc;
using StockPilot.Application.Features.Products.Commands.CreateProduct;
using StockPilot.Application.Features.Products.Queries.GetProductById;

namespace StockPilot.API.Controllers;

[ApiController]
[Route("api/products")]
public class ProductsController(
    CreateProductHandler createProductHandler,
    GetProductByIdHandler getProductByIdHandler) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateProduct([FromBody] CreateProductCommand command, CancellationToken cancellationToken)
    {
        var productResponse = await createProductHandler.HandleAsync(command, cancellationToken);
        return Ok(productResponse);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetProductById(Guid id, CancellationToken cancellationToken)
    {
        var product = await getProductByIdHandler.HandleAsync(new GetProductByIdQuery(id), cancellationToken);
        if (product == null)
        {
            return NotFound();
        }
        return Ok(product);
    }
}
