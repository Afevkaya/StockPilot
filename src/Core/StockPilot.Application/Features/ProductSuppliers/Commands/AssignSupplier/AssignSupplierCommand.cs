namespace StockPilot.Application.Features.ProductSuppliers.Commands.AssignSupplier;

public record AssignSupplierCommand(
    Guid ProductId,
    Guid SupplierId);
