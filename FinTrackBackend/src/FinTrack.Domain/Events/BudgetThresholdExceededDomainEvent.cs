using FinTrack.Domain.Primitives;
using FinTrack.Domain.ValueObjects;

namespace FinTrack.Domain.Events;

public sealed record BudgetThresholdExceededDomainEvent(
    Guid EventId,
    DateTime OccurredOnUtc,
    Guid BudgetId,
    Guid CategoryId,
    Money LimitAmount,
    Money CurrentSpent,
    decimal UsagePercentage) : IDomainEvent;