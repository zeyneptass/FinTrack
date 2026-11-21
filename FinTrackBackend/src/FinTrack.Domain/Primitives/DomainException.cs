namespace FinTrack.Domain.Primitives;

public sealed class DomainException : Exception
{
    public DomainException(string message) : base(message) { }
}

// Ýþ kurallarý (Invariant) ihlal edildiðinde fýrlatýlacak özel domain hatasý: