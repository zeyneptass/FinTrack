
namespace FinTrack.Domain.Primitives;

public abstract class Entity : IEquatable<Entity>
{
    private readonly List<IDomainEvent> _domainEvents = [];

    protected Entity(Guid id)
    {
        Id = id;
    }

    // EF Core'un veritabanýndan okuma yaparken nesneyi oluþturabilmesi için
    protected Entity() { }

    public Guid Id { get; private init; }

    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    protected void RaiseDomainEvent(IDomainEvent domainEvent) => _domainEvents.Add(domainEvent);

    public void ClearDomainEvents() => _domainEvents.Clear(); // event'leri dispatch ettikten sonra temizler Aksi halde ayný event tekrar tekrar iþlenir (double processing).

    public bool Equals(Entity? other)
    {
        if (other is null || other.GetType() != GetType()) return false;
        return other.Id == Id;
    }

    public override bool Equals(object? obj) => obj is Entity entity && Equals(entity);

    public override int GetHashCode() => Id.GetHashCode() * 41;
}