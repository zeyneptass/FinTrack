using FinTrack.Domain.Enums;
using FinTrack.Domain.Events;
using FinTrack.Domain.Primitives;
using FinTrack.Domain.ValueObjects;

namespace FinTrack.Domain.Entities;
// Bu sınıf, bir finansal işlemi (gelir/gider kaydı) temsil eden Aggregate Root'tur.

public sealed class Transaction : Entity
{
    private Transaction(
        Guid id,
        Guid accountId,
        Guid categoryId,
        TransactionType type,
        Money amount,
        string description,
        DateTime transactionDateUtc)
        : base(id)
    {
        AccountId = accountId;
        CategoryId = categoryId;
        Type = type;
        Amount = amount;
        Description = description;
        TransactionDateUtc = transactionDateUtc;
        CreatedAtUtc = DateTime.UtcNow;
    }

    private Transaction()
    {
        Amount = Money.Zero();  // Non-nullable property'ler için başlangıç değerleri: Money.Zero() ve ""
        Description = string.Empty;
    }

    public Guid AccountId { get; private set; }
    public Guid CategoryId { get; private set; }
    public TransactionType Type { get; private set; } 
    public Money Amount { get; private set; }
    public string Description { get; private set; }
    public DateTime TransactionDateUtc { get; private set; } // kullanıcının belirttiği işlem tarihi
    public DateTime CreatedAtUtc { get; private set; } // kaydın sisteme girildiği an

    public static Transaction Create(
        Guid accountId,
        Guid categoryId,
        TransactionType type,
        Money amount,
        string description,
        DateTime transactionDateUtc)
    {
        if (accountId == Guid.Empty)
            throw new DomainException("Geçerli bir hesap seçilmelidir.");

        if (categoryId == Guid.Empty)
            throw new DomainException("Geçerli bir kategori seçilmelidir.");

        if (amount.Amount <= 0)
            throw new DomainException("İşlem tutarı sıfırdan büyük olmalıdır.");

        if (string.IsNullOrWhiteSpace(description))
            throw new DomainException("İşlem açıklaması boş olamaz.");

        var transaction = new Transaction(
            Guid.NewGuid(),
            accountId,
            categoryId,
            type,
            amount,
            description.Trim(),
            transactionDateUtc);

        transaction.RaiseDomainEvent(new TransactionCreatedDomainEvent(
            Guid.NewGuid(),
            DateTime.UtcNow,
            transaction.Id,
            transaction.AccountId,
            transaction.CategoryId,
            transaction.Type,
            transaction.Amount,
            transaction.Description));

        return transaction;
    }
}