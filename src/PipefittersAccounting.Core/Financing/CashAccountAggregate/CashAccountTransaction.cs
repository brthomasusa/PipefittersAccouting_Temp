#pragma warning disable CS8618

using PipefittersAccounting.Core.Financing.CashAccountAggregate.ValueObjects;
using PipefittersAccounting.Core.Interfaces.Financing;
using PipefittersAccounting.SharedKernel;
using PipefittersAccounting.SharedKernel.CommonValueObjects;

namespace PipefittersAccounting.Core.Financing.CashAccountAggregate
{
    public class CashAccountTransaction : Entity<int>
    {
        protected CashAccountTransaction
        (
            CashTransactionAmount receiptAmount,
            CashTransactionDate receiptDate,
            CheckNumber checkNumber,
            RemittanceAdvice remittanceAdvice,
            EntityGuidID userId,
            ICashTransactionValidationService validationService
        )
        {
            TransactionAmount = receiptAmount;
            TransactionDate = receiptDate;
            CheckNumber = checkNumber;
            RemittanceAdvice = remittanceAdvice;
            UserId = userId;
            ValidationService = validationService;
        }

        public CashTransactionAmount TransactionAmount { get; init; }
        public CashTransactionDate TransactionDate { get; init; }
        public CheckNumber CheckNumber { get; init; }
        public RemittanceAdvice? RemittanceAdvice { get; init; }
        public EntityGuidID UserId { get; init; }
        protected ICashTransactionValidationService ValidationService { get; init; }
    }
}