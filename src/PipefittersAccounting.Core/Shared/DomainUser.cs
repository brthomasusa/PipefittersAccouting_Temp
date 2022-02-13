#pragma warning disable CS8618

using PipefittersAccounting.SharedKernel;

namespace PipefittersAccounting.Core.Shared
{
    public class DomainUser : Entity<Guid>
    {
        protected DomainUser() { }

        public DomainUser(ExternalAgent agent, string userName, string email)
        {
            Agent = agent ?? throw new ArgumentNullException("The external agent (employee, customer, etc.) is required.");
            Id = agent.Id;
            UserName = userName ?? throw new ArgumentNullException("The user name is required.");
            Email = email ?? throw new ArgumentNullException("The domain user email is required.");
        }

        public string UserName { get; private set; }
        public string Email { get; private set; }
        public virtual ExternalAgent Agent { get; private set; }
    }
}