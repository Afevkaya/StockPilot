using StockPilot.Domain.Entities.Common;

namespace StockPilot.Domain.Entities;

public class Product : BaseEntity
{
    public string Name { get; private set; } = null!;
    public string? Description { get; private set; }
    public decimal PurchasePrice { get; private set; }
    public decimal SalePrice { get; private set; }
    public Guid CategoryId { get; set; }

    public Product()
    {

    }

    public Product(string name, string? description, decimal purchasePrice, decimal salePrice, Guid categoryId)
    {
        ValidateName(name);
        ValidatePrices(purchasePrice, salePrice);

        Id = Guid.NewGuid();
        CategoryId = categoryId;
        Name = name.Trim();
        Description = description?.Trim();
        PurchasePrice = purchasePrice;
        SalePrice = salePrice;
        CategoryId = categoryId;
        CreatedAt = DateTime.UtcNow;
    }

    public void Update(string name, string? description, decimal purchasePrice, decimal salePrice, Guid categoryId)
    {
        ValidateName(name);
        ValidatePrices(purchasePrice, salePrice);

        Name = name;
        Description = description;
        PurchasePrice = purchasePrice;
        SalePrice = salePrice;
        UpdatedAt = DateTime.UtcNow;
    }

    private static void ValidateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Ürün adı boş olamaz.", nameof(name));
        }
    }

    private static void ValidatePrices(decimal purchasePrice, decimal salePrice)
    {
        if (purchasePrice < 0)
        {
            throw new ArgumentException("Alış fiyatı negatif olamaz.", nameof(purchasePrice));
        }

        if (salePrice < 0)
        {
            throw new ArgumentException("Satış fiyatı negatif olamaz.", nameof(salePrice));
        }
    }
}
