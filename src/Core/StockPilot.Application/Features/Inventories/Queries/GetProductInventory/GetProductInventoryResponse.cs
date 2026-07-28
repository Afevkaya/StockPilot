namespace StockPilot.Application.Features.Inventories.Queries.GetProductInventory;

public record GetProductInventoryResponse(
    Guid ProductId,
    string ProductName,
    long CurrentStock);
