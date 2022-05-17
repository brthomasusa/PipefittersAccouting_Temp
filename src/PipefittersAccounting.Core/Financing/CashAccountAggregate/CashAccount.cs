#pragma warning disable CS8604
#pragma warning disable CS8618

using PipefittersAccounting.Core.Financing.CashAccountAggregate.Events;
using PipefittersAccounting.Core.Financing.CashAccountAggregate.ValueObjects;
using PipefittersAccounting.SharedKernel;
using PipefittersAccounting.SharedKernel.CommonValueObjects;

namespace PipefittersAccounting.Core.Financing.CashAccountAggregate
{
    public class CashAccount : AggregateRoot<Guid>
    {
        private List<CashTransaction> _cashTransactions = new();
        private CashTransfer? _cashTransfer;

        protected CashAccount() => _cashTransactions = new();

        public CashAccount
        (
            EntityGuidID cashAcctId,
            CashAccountTypeEnum accountType,
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
            CashAccountType = accountType;
            BankName = bankName ?? throw new ArgumentNullException("The bank name is required.");
            CashAccountName = acctName ?? throw new ArgumentNullException("The cash account name is required.");
            CashAccountNumber = acctNumber ?? throw new ArgumentNullException("The cash account number is required.");
            RoutingTransitNumber = routingTransitNumber ?? throw new ArgumentNullException("The routing account number is required.");
            DateOpened = openedDate ?? throw new ArgumentNullException("The date that the cash account was opened is required.");
            UserId = userId ?? throw new ArgumentNullException("The user Id is required.");

            CheckValidity();
        }

        public CashAccountTypeEnum CashAccountType { get; private set; }
        public void UpdateCashAccountType(CashAccountTypeEnum cashAccountType)
        {
            //TODO Don't allow editing if the account has transactions not compatible with desired new type
            CashAccountType = cashAccountType;
            UpdateLastModifiedDate();
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

        public EntityGuidID UserId { get; private set; }
        public void UpdateUserId(EntityGuidID value)
        {
            UserId = value ?? throw new ArgumentNullException("The User id can not be null.");
            UpdateLastModifiedDate();
        }

        public virtual IReadOnlyCollection<CashTransaction> CashTransactions => _cashTransactions.ToList();

        public void DepositCash(CashTransaction deposit) => _cashTransactions.Add(deposit);


        public void DisburseCash(CashTransaction disbursement) => _cashTransactions.Add(disbursement);

        public void TransferCash(CashTransfer cashTransfer)
        {
            _cashTransfer = cashTransfer;
            AddDomainEvent(CashTransferCreated.Create(cashTransfer));
        }

        protected override void CheckValidity()
        {
            //
        }
    }
}