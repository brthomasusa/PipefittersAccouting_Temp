using PipefittersAccounting.Core.Financing.CashAccountAggregate;
using PipefittersAccounting.Core.Interfaces;
using PipefittersAccounting.SharedKernel;
using PipefittersAccounting.SharedModel.WriteModels.Financing;

namespace PipefittersAccounting.Infrastructure.Interfaces.Financing
{
    public interface ICashAccountAggregateValidationService : IDomainService
    {
        Task<ValidationResult> IsValidCashAccount(CashAccount cashAccount);

        Task<ValidationResult> IsValidCreateCashAccountInfo(CashAccountWriteModel writeModel);

        Task<ValidationResult> IsValidEditCashAccountInfo(CashAccountWriteModel writeModel);

        Task<ValidationResult> IsValidDeleteCashAccountInfo(CashAccountWriteModel writeModel);

        Task<ValidationResult> IsValidCashDepositOfSalesProceeds(CreateCashAccountTransactionInfo transactionInfo);

        Task<ValidationResult> IsValidCashDepositOfDebtIssueProceeds(CreateCashAccountTransactionInfo transactionInfo);

        Task<ValidationResult> IsValidCashDepositOfStockIssueProceeds(CreateCashAccountTransactionInfo transactionInfo);

        Task<ValidationResult> IsValidCashDisbursementForLoanPayment(CreateCashAccountTransactionInfo transactionInfo);

        Task<ValidationResult> IsValidCreateCashAccountTransferInfo(CashAccountTransferWriteModel transactionInfo);
    }
}