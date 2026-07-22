using Microsoft.AspNetCore.Mvc;
using StockPilot.Application.Features.Products.CreateProduct;

namespace StockPilot.API.Controllers;

[ApiController]
[Route("api/products")]
public class ProductsController(CreateProductHandler createProductHandler) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateProduct([FromBody] CreateProductCommand command, CancellationToken cancellationToken)
    {
        var productResponse = await createProductHandler.HandleAsync(command, cancellationToken);
        return Ok(productResponse);
    }
}
