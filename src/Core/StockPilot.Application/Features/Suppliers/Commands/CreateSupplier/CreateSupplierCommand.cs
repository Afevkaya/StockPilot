namespace StockPilot.Application.Features.Suppliers.Commands.CreateSupplier;

public record CreateSupplierCommand(string Name, string? ContactName, string? Email, string? Phone, string? Address);
