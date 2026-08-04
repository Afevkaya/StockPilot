namespace StockPilot.Domain.Entities;

public class Sale
{
    public Guid Id { get; private set; }
    public Guid ProductId { get; private set; }
    public int Quantity { get; private set; }
    public decimal UnitPrice { get; private set; }
    public DateTime SaleDate { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public Sale(Guid productId, int quantity, decimal unitPrice, DateTime saleDate)
    {
        ValidateGuidEmpty(productId);
        Validate(quantity, unitPrice, saleDate);
        Id = Guid.NewGuid();
        ProductId = productId;
        Quantity = quantity;
        UnitPrice = unitPrice;
        SaleDate = saleDate;
        CreatedAt = DateTime.UtcNow;
    }

    private static void ValidateGuidEmpty(Guid guid)
    {
        if (guid == Guid.Empty)
        {
            throw new ArgumentException("Ürün boş olamaz");
        }
    }

    private static void Validate(int quantity, decimal unitPrice, DateTime saleDate)
    {
        if (quantity <= 0)
        {
            throw new ArgumentException("Ürün adedi 0 dan büyük olmalıdır");
        }

        if (unitPrice < 0)
        {
            throw new ArgumentException("Birim fiyatı negatif olamaz");
        }

        if (saleDate > DateTime.UtcNow)
        {
            throw new ArgumentException("Satış tarihi gelecekte olamaz");
        }
    }
}
