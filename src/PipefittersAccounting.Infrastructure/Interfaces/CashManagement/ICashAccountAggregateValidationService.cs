using PipefittersAccounting.Core.CashManagement.CashAccountAggregate;
using PipefittersAccounting.Core.Interfaces;
using PipefittersAccounting.SharedKernel;
using PipefittersAccounting.SharedModel.WriteModels.CashManagement;

namespace PipefittersAccounting.Infrastructure.Interfaces.CashManagement
{
    public interface ICashAccountAggregateValidationService : IValidationService
    {
        Task<ValidationResult> IsValidCashAccount(CashAccount cashAccount);

        Task<ValidationResult> IsValidCreateCashAccountInfo(CashAccountWriteModel writeModel);

        Task<ValidationResult> IsValidEditCashAccountInfo(CashAccountWriteModel writeModel);

        Task<ValidationResult> IsValidDeleteCashAccountInfo(CashAccountWriteModel writeModel);

        Task<ValidationResult> IsValidCashDepositOfSalesProceeds(CashTransactionWriteModel transactionInfo);

        Task<ValidationResult> IsValidCashDepositOfDebtIssueProceeds(CashTransactionWriteModel transactionInfo);

        Task<ValidationResult> IsValidCashDepositOfStockIssueProceeds(CashTransactionWriteModel transactionInfo);

        Task<ValidationResult> IsValidCashDisbursementForLoanPayment(CashTransactionWriteModel transactionInfo);

        Task<ValidationResult> IsValidCashDisbursementForDividendPayment(CashTransactionWriteModel transactionInfo);

        Task<ValidationResult> IsValidCreateCashAccountTransferInfo(CashAccountTransferWriteModel transactionInfo);

        Task<ValidationResult> IsValidTimeCardPaymentInfo(List<CashTransactionWriteModel> writeModelCollection);
    }
}