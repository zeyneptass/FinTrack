using FinTrack.Domain.Enums;
using FinTrack.Domain.Primitives;
using FinTrack.Domain.ValueObjects;

namespace FinTrack.Domain.Entities;

public sealed class Account : Entity
{
    private Account(Guid id, string name, AccountType type, Money initialBalance)
        : base(id)
    {
        Name = name;
        Type = type;
        Balance = initialBalance;
        CreatedAtUtc = DateTime.UtcNow;
    }

    private Account()
    {
        Name = string.Empty;
        Balance = Money.Zero();
    }

    public string Name { get; private set; }
    public AccountType Type { get; private set; }
    public Money Balance { get; private set; }
    public DateTime CreatedAtUtc { get; private set; }

    public static Account Create(string name, AccountType type, Money initialBalance)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Hesap adı boş olamaz.");

        if (type != AccountType.CreditCard && initialBalance.Amount < 0)
            throw new DomainException("Nakit veya banka hesabı negatif başlangıç bakiyesi ile açılamaz.");

        return new Account(Guid.NewGuid(), name.Trim(), type, initialBalance);
    }

    public void ApplyTransaction(TransactionType transactionType, Money amount)
    {
        if (amount.Amount <= 0)
            throw new DomainException("İşlem tutarı sıfırdan büyük olmalıdır.");

        if (transactionType == TransactionType.Income)
        {
            Balance += amount;
            return;
        }

        if (Type != AccountType.CreditCard && Balance.Amount < amount.Amount)
            throw new DomainException($"Yetersiz bakiye. Mevcut bakiye: {Balance}, İstenen tutar: {amount}");

        Balance -= amount;
    }

    public void Rename(string newName)
    {
        if (string.IsNullOrWhiteSpace(newName))
            throw new DomainException("Yeni hesap adı boş olamaz.");

        Name = newName.Trim();
    }
}