namespace StockPilot.Application.Features.Suppliers.Commands.UpdateSupplier;

public record UpdateSupplierResponse(
    Guid Id,
    string Name,
    string? Email,
    string? Phone);
