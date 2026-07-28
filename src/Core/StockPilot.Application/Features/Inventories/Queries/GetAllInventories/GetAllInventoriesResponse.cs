namespace StockPilot.Application.Features.Inventories.Queries.GetAllInventories;

public record GetAllInventoriesResponse(
    Guid ProductId,
    string ProductName,
    string CategoryName,
    long CurrentStock);
