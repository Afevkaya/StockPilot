namespace StockPilot.Domain.Entities;

public class Purchase
{
    public Guid Id { get; private set; }
    public Guid SupplierId { get; private set; }
    public Guid ProductId { get; private set; }
    public int Quantity { get; private set; }
    public decimal UnitPrice { get; private set; }
    public DateTime PurchaseDate { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public Purchase(Guid productId, Guid supplierId, int quantity, decimal unitPrice, DateTime purchaseDate)
    {
        ValidateGuidEmpty(productId);
        ValidateGuidEmpty(supplierId);
        Validate(quantity, unitPrice, purchaseDate);
        Id = Guid.NewGuid();
        SupplierId = supplierId;
        ProductId = productId;
        Quantity = quantity;
        UnitPrice = unitPrice;
        PurchaseDate = purchaseDate;
        CreatedAt = DateTime.UtcNow;
    }

    private static void ValidateGuidEmpty(Guid guid)
    {
        if (guid == Guid.Empty)
        {
            throw new ArgumentException("Tedarikçi veya Ürün boş olamaz");
        }
    }

    private static void Validate(int quantity, decimal unitPrice, DateTime purchaseDate)
    {
        if (quantity <= 0)
        {
            throw new ArgumentException("Ürün adedi 0 dan büyük olmalıdır");
        }

        if (unitPrice < 0)
        {
            throw new ArgumentException("Birim fiyatı negatif olamaz");
        }

        if (purchaseDate > DateTime.UtcNow)
        {
            throw new ArgumentException("Satış tarihi gelecekte olamaz");
        }
    }
}
