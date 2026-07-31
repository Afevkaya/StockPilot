namespace StockPilot.Application.Features.ProductSuppliers.Commands.RemoveSupplier;

public record RemoveSupplierCommand(Guid ProductId, Guid SupplierId);
