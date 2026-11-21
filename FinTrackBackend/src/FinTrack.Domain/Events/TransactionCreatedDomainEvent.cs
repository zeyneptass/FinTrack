using FinTrack.Domain.Enums;
using FinTrack.Domain.Primitives;
using FinTrack.Domain.ValueObjects;

namespace FinTrack.Domain.Events;

public sealed record TransactionCreatedDomainEvent(
    Guid EventId,
    DateTime OccurredOnUtc,
    Guid TransactionId,
    Guid AccountId,
    Guid CategoryId,
    TransactionType Type,
    Money Amount,
    string Description) : IDomainEvent;

// Bir finansal işlem (transaction) oluşturulduğunda, domain içinde "şu olay gerçekleşti" bilgisini taşıyan değişmez bir mesaj nesnesidir.

