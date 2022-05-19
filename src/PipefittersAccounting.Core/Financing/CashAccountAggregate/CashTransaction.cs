#pragma warning disable CS8618

using PipefittersAccounting.Core.Financing.CashAccountAggregate.ValueObjects;
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
            EntityGuidID cashAccountID,
            CashTransactionDate transactionDate,
            CashTransactionAmount cashTransactionAmount,
            EntityGuidID agentId,
            EntityGuidID eventId,
            CheckNumber checkNumber,
            RemittanceAdvice remittanceAdvice,
            EntityGuidID userId
        )
            : this()
        {
            CashTransactionType = cashTransactionType;
            CashAccountId = cashAccountID;
            CashTransactionDate = transactionDate ?? throw new ArgumentNullException("The cash transaction date is required.");
            CashTransactionAmount = cashTransactionAmount ?? throw new ArgumentNullException("The cash transaction amount is required.");
            AgentId = agentId ?? throw new ArgumentNullException("The external agent (customer, financier, vendor, etc.)is required.");
            EventId = eventId ?? throw new ArgumentNullException("The economic event (loan agreement, stock subscription, etc.)is required.");
            CheckNumber = checkNumber ?? throw new ArgumentNullException("The check number is required.");
            RemittanceAdvice = remittanceAdvice;
            UserId = userId ?? throw new ArgumentNullException("The user Id is required.");

            CheckValidity();
        }

        public virtual CashTransactionTypeEnum CashTransactionType { get; private set; }
        public void UpdateCashTransactionType(CashTransactionTypeEnum cashTransactionType)
        {
            CashTransactionType = cashTransactionType;
            UpdateLastModifiedDate();
            CheckValidity();
        }

        public Guid CashAccountId { get; private set; }

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
            CashTransactionAmount = transactionAmount ?? throw new ArgumentNullException("The cash transaction amount is required.");
            UpdateLastModifiedDate();
            CheckValidity();
        }

        public Guid AgentId { get; private set; }
        public void UpdateAgentId(EntityGuidID value)
        {
            AgentId = value ?? throw new ArgumentNullException("The external agent (customer, financier, vendor, etc.)is required.");
            UpdateLastModifiedDate();
            CheckValidity();
        }

        public Guid EventId { get; set; }
        public void UpdateEventId(EntityGuidID value)
        {
            EventId = value ?? throw new ArgumentNullException("The economic event (loan agreement, stock subscription, etc.)is required.");
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