#pragma warning disable CS8604
#pragma warning disable CS8618

using PipefittersAccounting.Core.Financing.CashAccountAggregate.ValueObjects;
using PipefittersAccounting.Core.Interfaces;
using PipefittersAccounting.Core.Interfaces.Financing;
using PipefittersAccounting.Core.Shared;
using PipefittersAccounting.SharedKernel;
using PipefittersAccounting.SharedKernel.CommonValueObjects;
using PipefittersAccounting.SharedKernel.Interfaces;

namespace PipefittersAccounting.Core.Financing.CashAccountAggregate
{
    public class CashAccount : AggregateRoot<Guid>, IAggregateRoot
    {
        private List<CashTransaction> _cashTransactions;
        private readonly ICashTransactionValidationService _validationService;

        protected CashAccount() => _cashTransactions = new();

        public CashAccount
        (
            EntityGuidID cashAcctId,
            BankName bankName,
            CashAccountName acctName,
            CashAccountNumber acctNumber,
            RoutingTransitNumber routingTransitNumber,
            DateOpened openedDate,
            EntityGuidID userId,
            ICashTransactionValidationService validationService
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
            _validationService = validationService;

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

        public Guid UserId { get; private set; }
        public void UpdateUserId(EntityGuidID value)
        {
            UserId = value ?? throw new ArgumentNullException("The User id can not be null.");
            UpdateLastModifiedDate();
        }

        public virtual IReadOnlyCollection<CashTransaction> CashTransactions => _cashTransactions.ToList();

        public async Task AddDeposit(CashDeposit receipt)
        {
            ValidationResult result = await _validationService.IsValidCashDeposit(receipt);

            if (!result.IsValid)
            {
                throw new ArgumentException(result.Messages[0]);
            }

            _cashTransactions.Add
            (
                new CashTransaction
                (
                    CashTransactionTypeEnum.CashReceiptDebtIssueProceeds,
                    EntityGuidID.Create(this.Id),  // CashAccount Id financing proceeds
                    CashTransactionDate.Create(receipt.TransactionDate),
                    CashTransactionAmount.Create(receipt.TransactionAmount),
                    EntityGuidID.Create(receipt.Payor.Id),
                    EntityGuidID.Create(receipt.GoodsOrServiceSold.Id),
                    CheckNumber.Create(receipt.CheckNumber),
                    RemittanceAdvice.Create(receipt.RemittanceAdvice),
                    EntityGuidID.Create(receipt.UserId)
                )
            );
        }

        public void Disburse(CashTransaction cashTransaction) { }

        public void Transfer(decimal amount) { }

        protected override void CheckValidity()
        {
            //
        }
    }
}