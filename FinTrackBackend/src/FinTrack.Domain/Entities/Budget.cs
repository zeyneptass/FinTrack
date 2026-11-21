using FinTrack.Domain.Events;
using FinTrack.Domain.Primitives;
using FinTrack.Domain.ValueObjects;

namespace FinTrack.Domain.Entities;

public sealed class Budget : Entity
{
    // Bu sınıf, bir kategori için aylık harcama bütçesini temsil eder ve eşik aşımı bildirimini domain event ile fırlatan en "akıllı" entity'lerden biridir.
    private const decimal AlertThresholdPercentage = 80m;

    private Budget(Guid id, Guid categoryId, int year, int month, Money limitAmount)
        : base(id)
    {
        CategoryId = categoryId;
        Year = year;
        Month = month;
        LimitAmount = limitAmount;
        SpentAmount = Money.Zero(limitAmount.Currency);
    }

    private Budget()
    {
        // EF Core okuma için. Non-nullable Money property'lerine başlangıç değeri verilir.
        LimitAmount = Money.Zero();
        SpentAmount = Money.Zero();
    }

    public Guid CategoryId { get; private set; }
    public int Year { get; private set; }
    public int Month { get; private set; }
    public Money LimitAmount { get; private set; }
    public Money SpentAmount { get; private set; }

    public decimal UsagePercentage => LimitAmount.Amount == 0
        ? 0
        : decimal.Round((SpentAmount.Amount / LimitAmount.Amount) * 100m, 2);

    public static Budget Create(Guid categoryId, int year, int month, Money limitAmount)
    {
        if (categoryId == Guid.Empty)
            throw new DomainException("Bütçe için geçerli bir kategori belirtilmelidir.");

        if (year < 2000 || year > 2100)
            throw new DomainException("Geçersiz yıl değeri.");

        if (month < 1 || month > 12)
            throw new DomainException("Ay değeri 1 ile 12 arasında olmalıdır.");

        if (limitAmount.Amount <= 0)
            throw new DomainException("Bütçe limiti sıfırdan büyük olmalıdır.");

        return new Budget(Guid.NewGuid(), categoryId, year, month, limitAmount);
    }

    public void RecordExpense(Money expenseAmount)
    {
        if (expenseAmount.Amount <= 0)
            throw new DomainException("Harcama tutarı sıfırdan büyük olmalıdır.");

        var previousPercentage = UsagePercentage;
        SpentAmount += expenseAmount;
        var currentPercentage = UsagePercentage;

        if (previousPercentage < AlertThresholdPercentage && currentPercentage >= AlertThresholdPercentage)
        {
            RaiseDomainEvent(new BudgetThresholdExceededDomainEvent(
                Guid.NewGuid(),
                DateTime.UtcNow,
                Id,
                CategoryId,
                LimitAmount,
                SpentAmount,
                currentPercentage));
        }
    }

    public void UpdateLimit(Money newLimit)
    {
        if (newLimit.Amount <= 0)
            throw new DomainException("Yeni bütçe limiti sıfırdan büyük olmalıdır.");

        if (newLimit.Currency != LimitAmount.Currency)
            throw new DomainException("Bütçe para birimi değiştirilemez.");

        LimitAmount = newLimit;
    }
}