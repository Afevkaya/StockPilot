using System.Text.Json.Serialization;

namespace StockPilot.Application.Features.Categories.Commands.UpdateCategory;

public record UpdateCategoryCommand(
    [property: JsonIgnore]
    Guid Id,
    string Name,
    string Description);
