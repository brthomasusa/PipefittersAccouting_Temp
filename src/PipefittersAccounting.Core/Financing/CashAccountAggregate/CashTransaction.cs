#pragma warning disable CS8618

using PipefittersAccounting.Core.Financing.CashAccountAggregate.ValueObjects;
using PipefittersAccounting.Core.Shared;
using PipefittersAccounting.SharedKernel;
using PipefittersAccounting.SharedKernel.CommonValueObjects;

namespace PipefittersAccounting.Core.Financing.CashAccountAggregate
{
    public class CashTransaction : Entity<int>
    {
        protected CashTransaction() { }

        public CashTransaction
        (
            CashTransactionTypeEnum cashTransactionType,
            CashAccount cashAccount,
            CashTransactionDate transactionDate,
            ExternalAgent agentId,
            EconomicEvent eventId,
            CheckNumber checkNumber,
            RemittanceAdvice remittanceAdvice,
            EntityGuidID userId
        )
            : this()
        {
            CashTransactionType = cashTransactionType;
            CashAccount = cashAccount ?? throw new ArgumentNullException("This transaction requires a cash account.");
            CashTransactionDate = transactionDate ?? throw new ArgumentNullException("The cash transaction date is required.");
            ExternalAgent = agentId ?? throw new ArgumentNullException("The external agent (customer, financier, vendor, etc.)is required.");
            EconomicEvent = eventId ?? throw new ArgumentNullException("The economic event (loan agreement, stock subscription, etc.)is required.");
            CheckNumber = checkNumber ?? throw new ArgumentNullException("The check number is required.");
            RemittanceAdvice = remittanceAdvice;
            UserId = userId ?? throw new ArgumentNullException("The user Id is required.");

            CheckValidity();
        }

        public virtual CashTransactionTypeEnum CashTransactionType { get; private set; }
        public void UpdateCashTransactionType(CashTransactionTypeEnum cashTransactionType)
        {
            //TODO add validation to CashTransactionType
            CashTransactionType = cashTransactionType;
            UpdateLastModifiedDate();
            CheckValidity();
        }

        public virtual CashAccount CashAccount { get; private set; }
        public void UpdateCashAccount(CashAccount value)
        {
            CashAccount = value ?? throw new ArgumentNullException("This transaction requires a cash account.");
            UpdateLastModifiedDate();
            CheckValidity();
        }

        public virtual CashTransactionDate CashTransactionDate { get; private set; }
        public void UpdateCashTransactionDate(CashTransactionDate transactionDate)
        {
            CashTransactionDate = transactionDate ?? throw new ArgumentNullException("The cash transaction date is required.");
            UpdateLastModifiedDate();
            CheckValidity();
        }

        public virtual CashTransactionAmount CashTransactionAmount { get; private set; }
        public void UpdateCashTransactionAmount(CashTransactionAmount transactionAmount)
        {
            CashTransactionAmount = transactionAmount ?? throw new ArgumentNullException("The transaction amount is required.");
            UpdateLastModifiedDate();
            CheckValidity();
        }

        public virtual ExternalAgent ExternalAgent { get; private set; }
        public void UpdateExternalAgent(ExternalAgent value)
        {
            ExternalAgent = value ?? throw new ArgumentNullException("The external agent (customer, financier, vendor, etc.)is required.");
            UpdateLastModifiedDate();
            CheckValidity();
        }

        public virtual EconomicEvent EconomicEvent { get; set; }
        public void UpdateEconomicEvent(EconomicEvent value)
        {
            EconomicEvent = value ?? throw new ArgumentNullException("The economic event (loan agreement, stock subscription, etc.)is required.");
            UpdateLastModifiedDate();
            CheckValidity();
        }

        public virtual CheckNumber CheckNumber { get; private set; }
        public void UpdateCheckNumber(CheckNumber value)
        {
            CheckNumber = value ?? throw new ArgumentNullException("The check number is required.");
            UpdateLastModifiedDate();
            CheckValidity();
        }

        public virtual RemittanceAdvice? RemittanceAdvice { get; private set; }
        public void UpdateRemittanceAdvice(RemittanceAdvice value)
        {
            RemittanceAdvice = value;
            UpdateLastModifiedDate();
            CheckValidity();
        }

        public Guid UserId { get; private set; }
        public void UpdateUserId(EntityGuidID value)
        {
            UserId = value ?? throw new ArgumentNullException("The User id can not be null.");
            UpdateLastModifiedDate();
        }
    }
}