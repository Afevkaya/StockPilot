using StockPilot.Domain.Enums;

namespace StockPilot.Application.Features.StockMovements.Commands.CreateStockMovement;

public record CreateStockMovementCommand(Guid ProductId, int Quantity, MovementType MovementType);
