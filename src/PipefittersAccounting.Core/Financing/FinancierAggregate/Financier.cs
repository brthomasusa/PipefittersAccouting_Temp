#pragma warning disable CS8618

using PipefittersAccounting.SharedKernel;
using PipefittersAccounting.SharedKernel.CommonValueObjects;
using PipefittersAccounting.SharedKernel.Interfaces;
using PipefittersAccounting.Core.Shared;

namespace PipefittersAccounting.Core.Financing.FinancierAggregate
{
    public class Financier : AggregateRoot<Guid>
    {
        protected Financier() { }

        public Financier
        (
            EntityGuidID agentId,
            OrganizationName name,
            PhoneNumber telephone,
            EmailAddress emailAddress,
            Address address,
            PointOfContact contact,
            EntityGuidID userId,
            bool isActive
        )
        {
            Id = agentId ?? throw new ArgumentNullException("A financier id is required.");
            FinancierName = name ?? throw new ArgumentNullException("A financier name is required.");
            FinancierTelephone = telephone ?? throw new ArgumentNullException("A financier telephone number is required.");
            EmailAddress = emailAddress ?? throw new ArgumentNullException("An email address is required.");
            FinancierAddress = address ?? throw new ArgumentNullException("A financier address is required.");
            PointOfContact = contact ?? throw new ArgumentNullException("A point of contact is required.");
            UserId = userId ?? throw new ArgumentNullException("The user Id is required.");
            IsActive = isActive;
        }

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

        public EmailAddress EmailAddress { get; private set; }
        public void UpdateEmailAddress(EmailAddress value)
        {
            EmailAddress = value ?? throw new ArgumentNullException("An email address is required.");
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