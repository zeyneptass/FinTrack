using FinTrack.Domain.Primitives;

namespace FinTrack.Domain.ValueObjects;

public sealed record Money
{
    public decimal Amount { get; init; }
    public string Currency { get; init; }

    private Money(decimal amount, string currency)
    {
        Amount = amount;
        Currency = currency;
    }

    // EF Core mapping için
    private Money()
    {
        Currency = string.Empty;
    }

    public static Money Create(decimal amount, string currency = "TRY")
    {
        if (string.IsNullOrWhiteSpace(currency) || currency.Trim().Length != 3)
            throw new DomainException("Para birimi 3 harfli ISO formatında olmalıdır (Örn: TRY, USD, EUR).");

        return new Money(decimal.Round(amount, 2), currency.Trim().ToUpperInvariant());
    }

    public static Money Zero(string currency = "TRY") => Create(0m, currency);

    public static Money operator +(Money left, Money right)
    {
        EnsureSameCurrency(left, right);
        return new Money(left.Amount + right.Amount, left.Currency);
    }

    public static Money operator -(Money left, Money right)
    {
        EnsureSameCurrency(left, right);
        return new Money(left.Amount - right.Amount, left.Currency);
    }

    private static void EnsureSameCurrency(Money left, Money right)
    {
        if (left.Currency != right.Currency)
            throw new DomainException($"Farklı para birimleri arasında işlem yapılamaz: {left.Currency} ve {right.Currency}.");
    }

    public override string ToString() => $"{Amount:N2} {Currency}";
}