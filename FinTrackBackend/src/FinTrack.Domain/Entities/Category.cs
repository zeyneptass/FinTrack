using FinTrack.Domain.Enums;
using FinTrack.Domain.Primitives;

namespace FinTrack.Domain.Entities;

public sealed class Category : Entity
// : Entity --> Id, DomainEvents, Equals vb. miras alınır
{
    private Category(Guid id, string name, TransactionType type, string? icon)
        : base(id)
    {
        Name = name;
        Type = type;
        Icon = icon;
    }

    private Category()
    {
        Name = string.Empty;
    }

    public string Name { get; private set; }
    public TransactionType Type { get; private set; }
    public string? Icon { get; private set; }

    public static Category Create(string name, TransactionType type, string? icon = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Kategori adı boş olamaz.");

        return new Category(Guid.NewGuid(), name.Trim(), type, icon?.Trim());
    }

    public void UpdateDetails(string name, string? icon)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Kategori adı boş olamaz.");

        Name = name.Trim();
        Icon = icon?.Trim();
    }
}