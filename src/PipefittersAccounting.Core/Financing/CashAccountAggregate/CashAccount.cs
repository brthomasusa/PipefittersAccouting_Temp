#pragma warning disable CS8604
#pragma warning disable CS8618

using PipefittersAccounting.Core.Financing.CashAccountAggregate.Events;
using PipefittersAccounting.Core.Financing.CashAccountAggregate.ValueObjects;
using PipefittersAccounting.Core.Interfaces;
using PipefittersAccounting.Core.Interfaces.Financing;
using PipefittersAccounting.SharedKernel;
using PipefittersAccounting.SharedKernel.CommonValueObjects;
using PipefittersAccounting.SharedKernel.Interfaces;

namespace PipefittersAccounting.Core.Financing.CashAccountAggregate
{
    public class CashAccount : AggregateRoot<Guid>
    {
        private List<CashTransaction> _cashTransactions = new();
        private AccountBalanceInformation? _balanceInfo;
        private CashTransfer? _cashTransfer;
        private readonly ICashAccountAggregateValidationService _validationService;

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
            EntityGuidID userId,
            ICashAccountAggregateValidationService validationService
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
            _validationService = validationService;

            CheckValidity();
        }

        public CashAccountTypeEnum CashAccountType { get; private set; }
        public void UpdateCashAccountType(CashAccountTypeEnum cashAccountType)
        {
            //TODO Don't allow editing if the account has transactions
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

        public CashTransactionAmount CashInflow
        {
            get
            {
                if (_balanceInfo is null)
                {

                }

                throw new NotImplementedException();
            }
        }

        public CashTransactionAmount CashOutflow
        {
            get
            {
                if (_balanceInfo is null)
                {

                }
                throw new NotImplementedException();
            }
        }

        public CashTransactionAmount CurrentBalance
        {
            get
            {
                if (_balanceInfo is null)
                {

                }
                throw new NotImplementedException();
            }
        }

        public async Task DepositCash(CashDeposit deposit)
        {
            ValidationResult result = await _validationService.IsValidCashDeposit(deposit);

            if (!result.IsValid)
            {
                throw new ArgumentException(result.Messages[0]);
            }

            _cashTransactions.Add
            (
                new CashTransaction
                (
                    deposit.DepositType,
                    EntityGuidID.Create(this.Id),  // CashAccount Id 
                    deposit.TransactionDate,
                    deposit.TransactionAmount,
                    EntityGuidID.Create(deposit.Payor.Id),
                    EntityGuidID.Create(deposit.GoodsOrServiceSold.Id),
                    deposit.CheckNumber,
                    deposit.RemittanceAdvice,
                    EntityGuidID.Create(deposit.UserId)
                )
            );

            AddDomainEvent(CashDepositCreated.Create(deposit));
        }

        public async Task DisburseCash(CashDisbursement disbursement)
        {
            ValidationResult result = await _validationService.IsValidCashDisbursement(disbursement);

            if (!result.IsValid)
            {
                throw new ArgumentException(result.Messages[0]);
            }

            _cashTransactions.Add
            (
                new CashTransaction
                (
                    disbursement.DisbursementType,
                    EntityGuidID.Create(this.Id),  // CashAccount Id financing proceeds
                    disbursement.TransactionDate,
                    disbursement.TransactionAmount,
                    EntityGuidID.Create(disbursement.Payee.Id),
                    EntityGuidID.Create(disbursement.GoodsOrServicePurchased.Id),
                    disbursement.CheckNumber,
                    disbursement.RemittanceAdvice,
                    EntityGuidID.Create(disbursement.UserId)
                )
            );

            AddDomainEvent(CashDisbursementCreated.Create(disbursement));
        }

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