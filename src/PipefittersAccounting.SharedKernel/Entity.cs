#pragma warning disable CS8618

namespace PipefittersAccounting.SharedKernel
{
    public abstract class Entity<T>
    {
        public T Id { get; protected set; }

        public DateTime CreatedDate { get; } = DateTime.UtcNow;

        public DateTime? LastModifiedDate { get; private set; }

        public void UpdateLastModifiedDate()
        {
            LastModifiedDate = DateTime.UtcNow;
        }

        protected virtual void CheckValidity()
        {
            // Validation involving multiple properties go here.
        }
    }
}