#pragma warning disable CS8618

using PipefittersAccounting.Core.Shared;
using PipefittersAccounting.SharedKernel;
using PipefittersAccounting.SharedKernel.CommonValueObjects;
using PipefittersAccounting.SharedKernel.Interfaces;

namespace PipefittersAccounting.Core.Financing.FinancierAggregate
{
    public class Financier : AggregateRoot<Guid>, IAggregateRoot
    {
        protected Financier() { }

        public Financier
        (
            FinancierAgent agent,
            OrganizationName name,
            PhoneNumber telephone,
            Address address,
            PointOfContact contact,
            EntityGuidID userId,
            bool isActive
        )
        {
            if (agent is null)
            {
                throw new ArgumentNullException("An ExternalAgent is required.");
            }
            Id = agent.ExternalAgent.Id;
            ExternalAgent = agent.ExternalAgent;

            FinancierName = name;
            FinancierTelephone = telephone;
            FinancierAddress = address;
            PointOfContact = contact;
            UserId = userId;
            IsActive = isActive;
        }

        public virtual ExternalAgent ExternalAgent { get; private set; }

        public OrganizationName FinancierName { get; private set; }

        public PhoneNumber FinancierTelephone { get; private set; }

        public Address FinancierAddress { get; private set; }

        public PointOfContact PointOfContact { get; private set; }

        public bool IsActive { get; private set; }

        public void UpdateFinancierStatus(bool value)
        {
            IsActive = value;
            UpdateLastModifiedDate();
        }

        public Guid UserId { get; private set; }

        public void UpdateUserId(EntityGuidID value)
        {
            UserId = value ?? throw new ArgumentNullException("The User id can not be null.");
            UpdateLastModifiedDate();
        }
    }
}