#pragma warning disable CS8618

using PipefittersAccounting.Core.Shared;
using PipefittersAccounting.SharedKernel;
using PipefittersAccounting.SharedKernel.CommonValueObjects;

namespace PipefittersAccounting.Core.Shared
{
    public class DomainUser : Entity<Guid>
    {
        protected DomainUser() { }

        public DomainUser(ExternalAgent agent, string userName, string email)
        {
            Id = agent.Id;
            UserName = userName;
            Email = email;
        }

        public string UserName { get; private set; }
        public string Email { get; private set; }
    }
}