using System.Text.Json.Serialization;

namespace StockPilot.Application.Features.Suppliers.Commands.UpdateSupplier;

public record UpdateSupplierCommand(
    [property: JsonIgnore]
    Guid Id,
    string Name,
    string? ContactName,
    string? Email,
    string? Phone,
    string? Address);
