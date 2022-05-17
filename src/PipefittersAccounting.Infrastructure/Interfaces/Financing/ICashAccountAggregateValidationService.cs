using PipefittersAccounting.Core.Financing.CashAccountAggregate;
using PipefittersAccounting.Core.Interfaces;
using PipefittersAccounting.SharedKernel;
using PipefittersAccounting.SharedModel.WriteModels.Financing;

namespace PipefittersAccounting.Infrastructure.Interfaces.Financing
{
    public interface ICashAccountAggregateValidationService : IDomainService
    {
        Task<ValidationResult> IsValidCashAccount(CashAccount cashAccount);

        Task<ValidationResult> IsValidCreateCashAccountInfo(CreateCashAccountInfo writeModel);

        Task<ValidationResult> IsValidEditCashAccountInfo(EditCashAccountInfo writeModel);

        Task<ValidationResult> IsValidDeleteCashAccountInfo(DeleteCashAccountInfo writeModel);

        Task<ValidationResult> IsValidCashDepositOfSalesProceeds(CreateCashAccountTransactionInfo transactionInfo);

        Task<ValidationResult> IsValidCashDepositOfDebtIssueProceeds(CreateCashAccountTransactionInfo transactionInfo);

        Task<ValidationResult> IsValidCashDepositOfStockIssueProceeds(CreateCashAccountTransactionInfo transactionInfo);

        Task<ValidationResult> IsValidCashDisbursementForLoanPayment(CreateCashAccountTransactionInfo transactionInfo);
    }
}