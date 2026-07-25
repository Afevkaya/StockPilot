using StockPilot.Domain.Entities.Common;

namespace StockPilot.Domain.Entities;

public class Category : BaseEntity
{
    public string Name { get; private set; } = null!;
    public string Description { get; private set; } = null!;

    public Category()
    {

    }

    public Category(string name, string description)
    {
        ValidateName(name);
        ValidateDescription(description);

        Id = Guid.NewGuid();
        Name = name.Trim();
        Description = description.Trim();
        CreatedAt = DateTime.UtcNow;
    }

    private static void ValidateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Kategori adı boş olamaz.", nameof(name));
        }
    }

    private static void ValidateDescription(string description)
    {
        if (string.IsNullOrWhiteSpace(description))
        {
            throw new ArgumentException("Kategori açıklaması boş olamaz.", nameof(description));
        }
    }
}
