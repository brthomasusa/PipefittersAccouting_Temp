using PipefittersAccounting.Core.Financing.CashAccountAggregate;
using PipefittersAccounting.Core.Financing.CashAccountAggregate.ValueObjects;
using PipefittersAccounting.Core.Shared;

namespace PipefittersAccounting.Core.Interfaces.Financing
{
    public interface ICashTransactionValidationService
    {
        Task<ValidationResult> IsValid(CashTransaction cashTransaction);

        Task<ValidationResult> IsValidCashDisbursement(CashTransactionTypeEnum disbursementType,
                                                       EconomicEvent goodsOrServiceReceived,
                                                       ExternalAgent soldBy,
                                                       CashTransactionAmount transactionAmount);

        Task<ValidationResult> IsValidCashDeposit(CashTransactionTypeEnum depositType,
                                                  EconomicEvent goodsOrServiceProvided,
                                                  ExternalAgent purchasedBy,
                                                  CashTransactionAmount transactionAmount);
    }
}