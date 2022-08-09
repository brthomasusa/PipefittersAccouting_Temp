using PipefittersAccounting.SharedModel.ReadModels;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedModel.Readmodels.CashManagement;
using PipefittersAccounting.SharedModel.Readmodels.Financing;

namespace PipefittersAccounting.Infrastructure.Interfaces.CashManagement
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
        Task<OperationResult<CreditorIsOwedThisLoanInstallmentValidationInfo>> GetCreditorIsOwedThisLoanInstallmentValidationInfo(GetLoanInstallmentInfoParameters queryParameters);
        Task<OperationResult<CashAccountTransactionDetail>> GetCashAccountTransactionDetail(GetCashAccountTransactionDetailParameters queryParameters);
        Task<OperationResult<PagedList<CashAccountTransactionListItem>>> GetCashAccountTransactionListItem(GetCashAccountTransactionListItemsParameters queryParameters); //GetInvestorIdForDividendDeclaration
        Task<OperationResult<Guid>> GetInvestorIdForStockSubscription(GetInvestorIdForStockSubscriptionParameter queryParameters);
        Task<OperationResult<Guid>> GetInvestorIdForDividendDeclaration(GetDividendDeclarationParameter queryParameters);
        Task<OperationResult<VerificationOfCashDepositStockIssueProceeds>> VerifyCashDepositOfStockIssueProceeds(GetStockSubscriptionParameter queryParameters);
        Task<OperationResult<DividendDeclarationDetails>> GetDividendDeclarationDetails(GetDividendDeclarationParameter queryParameters);
        Task<OperationResult<List<TimeCardPaymentInfo>>> GetTimeCardPaymentInfo(GetTimeCardPaymentInfoParameter queryParameters);
    }
}