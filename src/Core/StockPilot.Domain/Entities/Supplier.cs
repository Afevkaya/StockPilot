using StockPilot.Domain.Entities.Common;

namespace StockPilot.Domain.Entities;

public class Supplier : BaseEntity
{
    public string Name { get; private set; } = null!;
    public string? ContactName { get; private set; }
    public string? Email { get; private set; }
    public string? Phone { get; private set; }
    public string? Address { get; private set; }

    public Supplier()
    {

    }

    public Supplier(string name, string? contactName, string? email, string? phone, string? address)
    {
        ValidName(name);
        Id = Guid.NewGuid();
        Name = name;
        ContactName = contactName;
        Email = email;
        Phone = phone;
        Address = address;
        CreatedAt = DateTime.UtcNow;
    }

    public void Update(string name, string? contactName, string? email, string? phone, string? address)
    {
        ValidName(name);
        Name = name;
        ContactName = contactName;
        Email = email;
        Phone = phone;
        Address = address;
        UpdatedAt = DateTime.UtcNow;
    }

    private void ValidName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Tedarikçi firma ismi boş olamaz", nameof(name));
        }
    }
}
