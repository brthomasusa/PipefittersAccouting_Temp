#pragma warning disable CS8618

using PipefittersAccounting.Core.Interfaces.Financing;
using PipefittersAccounting.SharedKernel;
using PipefittersAccounting.SharedKernel.CommonValueObjects;

namespace PipefittersAccounting.Core.Financing.CashAccountAggregate.ValueObjects
{
    public abstract class CashAccountTransaction : ValueObject
    {
        protected readonly ICashTransactionValidationService _validationService;

        protected CashAccountTransaction
        (
            CashTransactionAmount receiptAmount,
            CashTransactionDate receiptDate,
            CheckNumber checkNumber,
            RemittanceAdvice remittanceAdvice,
            EntityGuidID userId
        )
        {
            TransactionAmount = receiptAmount;
            TransactionDate = receiptDate;
            CheckNumber = checkNumber;
            RemittanceAdvice = remittanceAdvice;
            UserId = userId;
        }

        public CashTransactionAmount TransactionAmount { get; init; }
        public CashTransactionDate TransactionDate { get; init; }
        public CheckNumber CheckNumber { get; init; }
        public RemittanceAdvice? RemittanceAdvice { get; init; }
        public EntityGuidID UserId { get; init; }
    }
}