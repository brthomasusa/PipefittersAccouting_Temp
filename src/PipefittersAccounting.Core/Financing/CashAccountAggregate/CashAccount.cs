#pragma warning disable CS8618

using PipefittersAccounting.Core.Financing.CashAccountAggregate.ValueObjects;
using PipefittersAccounting.SharedKernel;
using PipefittersAccounting.SharedKernel.CommonValueObjects;
using PipefittersAccounting.SharedKernel.Interfaces;

namespace PipefittersAccounting.Core.Financing.CashAccountAggregate
{
    public class CashAccount : AggregateRoot<Guid>, IAggregateRoot
    {
        protected CashAccount() { }

        public CashAccount
        (
            EntityGuidID cashAcctId,
            BankName bankName,
            CashAccountName acctName,
            CashAccountNumber acctNumber,
            RoutingTransitNumber routingTransitNumber,
            DateOpened openedDate,
            EntityGuidID userId
        )
            : this()
        {
            Id = cashAcctId ?? throw new ArgumentNullException("The cash account Id is required.");
            BankName = bankName ?? throw new ArgumentNullException("The bank name is required.");
            CashAccountName = acctName ?? throw new ArgumentNullException("The cash account name is required.");
            CashAccountNumber = acctNumber ?? throw new ArgumentNullException("The cash account number is required.");
            RoutingTransitNumber = routingTransitNumber ?? throw new ArgumentNullException("The routing account number is required.");
            DateOpened = openedDate ?? throw new ArgumentNullException("The date that the cash account was opened is required.");
            UserId = userId ?? throw new ArgumentNullException("The user Id is required.");

            CheckValidity();
        }

        public virtual BankName BankName { get; private set; }
        public void UpdateBankName(BankName value)
        {
            BankName = value ?? throw new ArgumentNullException("The bank name is required.");
            UpdateLastModifiedDate();
            CheckValidity();
        }

        public virtual CashAccountName CashAccountName { get; private set; }
        public void UpdateCashAccountName(CashAccountName value)
        {
            CashAccountName = value ?? throw new ArgumentNullException("The cash account name is required.");
            UpdateLastModifiedDate();
            CheckValidity();
        }

        public virtual CashAccountNumber CashAccountNumber { get; private set; }
        public void UpdateCashAccountNumber(CashAccountNumber value)
        {
            CashAccountNumber = value ?? throw new ArgumentNullException("The cash account number is required.");
            UpdateLastModifiedDate();
            CheckValidity();
        }

        public virtual RoutingTransitNumber RoutingTransitNumber { get; private set; }
        public void UpdateRoutingTransitNumber(RoutingTransitNumber value)
        {
            RoutingTransitNumber = value ?? throw new ArgumentNullException("The routing transit number is required.");
            UpdateLastModifiedDate();
            CheckValidity();
        }

        public virtual DateOpened DateOpened { get; private set; }
        public void UpdateDateOpened(DateOpened value)
        {
            DateOpened = value ?? throw new ArgumentNullException("The account open date is required.");
            UpdateLastModifiedDate();
            CheckValidity();
        }

        public Guid UserId { get; private set; }
        public void UpdateUserId(EntityGuidID value)
        {
            UserId = value ?? throw new ArgumentNullException("The User id can not be null.");
            UpdateLastModifiedDate();
        }

        protected override void CheckValidity()
        {
            //
        }
    }
}