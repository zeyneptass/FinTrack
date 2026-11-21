namespace FinTrack.Domain.Primitives;

public interface IDomainEvent
{
	Guid EventId { get; }
	DateTime OccurredOnUtc { get; }
}
//Domain içinde önemli bir iþ olayý gerçekleþtiðinde fýrlatýlacak olaylarýn ortak arayüzü