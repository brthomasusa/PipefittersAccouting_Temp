namespace PipefittersAccounting.SharedKernel.Interfaces
{
    public interface IDomainEventHandler<T> where T : IDomainEvent
    {
        void Handle(T domainEvent);
    }
}