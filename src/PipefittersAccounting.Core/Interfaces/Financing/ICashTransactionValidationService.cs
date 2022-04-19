using PipefittersAccounting.Core.Financing.CashAccountAggregate;
using PipefittersAccounting.Core.Financing.CashAccountAggregate.ValueObjects;
using PipefittersAccounting.Core.Interfaces;
using PipefittersAccounting.SharedKernel.CommonValueObjects;

namespace PipefittersAccounting.Core.Interfaces.Financing
{
    public interface ICashTransactionValidationService
    {
        Task<ValidationResult> IsValid(CashTransaction cashTransaction);
        Task<ValidationResult> IsValidCashDisbursement(CashTransactionTypeEnum transactionType,
                                                       EntityGuidID goodsOrServiceReceived,
                                                       EntityGuidID soldBy,
                                                       CashTransactionAmount transactionAmount);
    }
}