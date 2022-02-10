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
                throw new ArgumentNullException("A financier agent is required.");
            }
            Id = agent.ExternalAgent.Id;
            ExternalAgent = agent.ExternalAgent;

            FinancierName = name ?? throw new ArgumentNullException("A financier name is required."); ;
            FinancierTelephone = telephone ?? throw new ArgumentNullException("A financier telephone number is required."); ;
            FinancierAddress = address ?? throw new ArgumentNullException("A financier address is required."); ;
            PointOfContact = contact ?? throw new ArgumentNullException("A point of contact is required."); ;
            UserId = userId ?? throw new ArgumentNullException("The user Id is required."); ;
            IsActive = isActive;
        }

        public virtual ExternalAgent ExternalAgent { get; private set; }

        public OrganizationName FinancierName { get; private set; }
        public void UpdateFinancierName(OrganizationName value)
        {
            FinancierName = value ?? throw new ArgumentNullException("A financier name is required.");
            UpdateLastModifiedDate();
        }

        public PhoneNumber FinancierTelephone { get; private set; }
        public void UpdateFinancierTelephone(PhoneNumber value)
        {
            FinancierTelephone = value ?? throw new ArgumentNullException("A financier telephone number is required.");
            UpdateLastModifiedDate();
        }

        public Address FinancierAddress { get; private set; }
        public void UpdateFinancierAddress(Address value)
        {
            FinancierAddress = value ?? throw new ArgumentNullException("A financier address is required.");
            UpdateLastModifiedDate();
        }

        public PointOfContact PointOfContact { get; private set; }
        public void UpdatePointOfContact(PointOfContact value)
        {
            PointOfContact = value ?? throw new ArgumentNullException("A point of contact is required.");
            UpdateLastModifiedDate();
        }

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