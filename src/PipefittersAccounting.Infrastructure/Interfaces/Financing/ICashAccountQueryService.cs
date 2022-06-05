using PipefittersAccounting.SharedModel.ReadModels;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedModel.Readmodels.Financing;

namespace PipefittersAccounting.Infrastructure.Interfaces.Financing
{
    public interface ICashAccountQueryService
    {
        Task<OperationResult<CashAccountDetail>> GetCashAccountDetails(GetCashAccount queryParameters);

        Task<OperationResult<CashAccountReadModel>> GetCashAccountReadModel(GetCashAccount queryParameters);

        Task<OperationResult<CashAccountReadModel>> GetCashAccountWithAccountName(GetCashAccountWithAccountName queryParameters);

        Task<OperationResult<CashAccountReadModel>> GetCashAccountWithAccountNumber(GetCashAccountWithAccountNumber queryParameters);

        Task<OperationResult<PagedList<CashAccountListItem>>> GetCashAccountListItems(GetCashAccounts queryParameters);

        Task<OperationResult<int>> GetNumberOfCashAccountTransactions(GetCashAccount queryParameters);

        Task<OperationResult<CreditorIssuedLoanAgreementValidationInfo>> GetCreditorIssuedLoanAgreementValidationInfo(CreditorLoanAgreementValidationParameters queryParameters);

        Task<OperationResult<CashReceiptOfDebtIssueProceedsInfo>> GetCashReceiptOfDebtIssueProceedsInfo(CreditorLoanAgreementValidationParameters queryParameters);

        Task<OperationResult<CashDisbursementForLoanInstallmentPaymentInfo>> GetCashDisbursementForLoanInstallmentPaymentInfo(GetLoanInstallmentInfoParameters queryParameters);

        Task<OperationResult<FinancierToLoanInstallmentValidationInfo>> GetFinancierToLoanInstallmentValidationInfo(GetLoanInstallmentInfoParameters queryParameters);

        Task<OperationResult<CashAccountTransactionDetail>> GetCashAccountTransactionDetail(GetCashAccountTransactionDetailParameters queryParameters);

        Task<OperationResult<PagedList<CashAccountTransactionListItem>>> GetCashAccountTransactionListItem(GetCashAccountTransactionListItemsParameters queryParameters);
    }
}