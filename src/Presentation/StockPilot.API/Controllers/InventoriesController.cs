using Microsoft.AspNetCore.Mvc;
using StockPilot.Application.Features.Inventories.Queries.GetAllInventories;
using StockPilot.Application.Features.Inventories.Queries.GetProductInventory;

namespace StockPilot.API.Controllers;

[ApiController]
[Route("api/inventories")]
public class InventoriesController(
    GetProductInventoryHandler getProductInventoryHandler,
    GetAllInventoriesHandler getAllInventoriesHandler) : ControllerBase
{
    [HttpGet("products/{productId:guid}")]
    public async Task<IActionResult> GetInventoryByProductId([FromRoute] Guid productId, CancellationToken cancellationToken)
    {
        return Ok(await getProductInventoryHandler.Handle(new GetProductInventoryQuery(productId), cancellationToken));
    }

    [HttpGet("products/list")]
    public async Task<IActionResult> GetAllInventories(CancellationToken cancellationToken)
    {
        return Ok(await getAllInventoriesHandler.Handle(cancellationToken));
    }
}
