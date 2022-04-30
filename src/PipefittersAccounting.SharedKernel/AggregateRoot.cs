using PipefittersAccounting.SharedKernel.Interfaces;

namespace PipefittersAccounting.SharedKernel
{
    public abstract class AggregateRoot<T> : Entity<T>, IAggregateRoot
    {
        private readonly List<IDomainEvent> _domainEvents = new List<IDomainEvent>();

        public virtual IReadOnlyList<IDomainEvent> DomainEvents => _domainEvents;

        protected virtual void AddDomainEvent(IDomainEvent newEvent) => _domainEvents.Add(newEvent);

        public virtual void ClearEvents() => _domainEvents.Clear();
    }
}