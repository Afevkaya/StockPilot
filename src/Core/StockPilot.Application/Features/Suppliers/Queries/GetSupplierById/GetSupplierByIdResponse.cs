namespace StockPilot.Application.Features.Suppliers.Queries.GetSupplierById;

public record GetSupplierByIdResponse(
    Guid Id,
    string Name,
    string? ContactName,
    string? Email,
    string? Phone,
    string? Address,
    DateTime CreatedAt,
    DateTime? UpdatedAt);
