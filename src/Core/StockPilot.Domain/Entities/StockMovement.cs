using StockPilot.Domain.Enums;

namespace StockPilot.Domain.Entities;

public class StockMovement
{
    public Guid Id { get; private set; }
    public Guid ProductId { get; private set; }
    public int Quantity { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public MovementType MovementType { get; private set; }

    public StockMovement(Guid productId, int quantity, MovementType movementType)
    {
        ValidProductId(productId);
        ValidQuantity(quantity);
        ValidMovementType(movementType);

        Id = Guid.NewGuid();
        ProductId = productId;
        Quantity = quantity;
        MovementType = movementType;
        CreatedAt = DateTime.UtcNow;
    }

    private void ValidProductId(Guid productId)
    {
        if (productId == Guid.Empty)
        {
            throw new ArgumentException("Geçerli bir ürün ID'si girin", nameof(productId));
        }
    }

    private void ValidMovementType(MovementType movementType)
    {
        if (!Enum.IsDefined(movementType))
        {
            throw new ArgumentException("Geçerli bir hareket tipi girin", nameof(movementType));
        }
    }

    private void ValidQuantity(int quantity)
    {
        if (quantity <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(quantity), "Miktar sıfırdan büyük olmalıdır");
        }
    }
}
