using PipefittersAccounting.Infrastructure.Application.Queries.CashManagement;
using PipefittersAccounting.Infrastructure.Persistence.DatabaseContext;
using PipefittersAccounting.Infrastructure.Interfaces.CashManagement;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedModel.ReadModels;
using PipefittersAccounting.SharedModel.Readmodels.CashManagement;
using PipefittersAccounting.SharedModel.Readmodels.Financing;

namespace PipefittersAccounting.Infrastructure.Application.Services.CashManagement
{
    public class CashAccountQueryService : ICashAccountQueryService
    {
        private readonly DapperContext _dapperCtx;

        public CashAccountQueryService(DapperContext ctx) => _dapperCtx = ctx;


        public async Task<OperationResult<CashAccountDetail>> GetCashAccountDetails(GetCashAccount queryParameters)
            => await GetCashAccountDetailsQuery.Query(queryParameters, _dapperCtx);

        public async Task<OperationResult<CashAccountReadModel>> GetCashAccountReadModel(GetCashAccount queryParameters)
            => await GetCashAccountReadModelQuery.Query(queryParameters, _dapperCtx);

        public async Task<OperationResult<CashAccountReadModel>> GetCashAccountWithAccountName(GetCashAccountWithAccountName queryParameters)
            => await GetCashAccountWithAccountNameQuery.Query(queryParameters, _dapperCtx);

        public async Task<OperationResult<CashAccountReadModel>> GetCashAccountWithAccountNumber(GetCashAccountWithAccountNumber queryParameters)
            => await GetCashAccountWithAccountNumberQuery.Query(queryParameters, _dapperCtx);

        public async Task<OperationResult<PagedList<CashAccountListItem>>> GetCashAccountListItems(GetCashAccounts queryParameters)
            => await GetCashAccountListItemQuery.Query(queryParameters, _dapperCtx);

        public async Task<OperationResult<int>> GetNumberOfCashAccountTransactions(GetCashAccount queryParameters)
            => await GetNumberOfCashAccountTransactionsQuery.Query(queryParameters, _dapperCtx);

        public async Task<OperationResult<CreditorIssuedLoanAgreementValidationInfo>> GetCreditorIssuedLoanAgreementValidationInfo(CreditorLoanAgreementValidationParameters queryParameters)
            => await GetCreditorIssuedLoanAgreementValidationInfoQuery.Query(queryParameters, _dapperCtx);

        public async Task<OperationResult<CashReceiptOfDebtIssueProceedsInfo>> GetCashReceiptOfDebtIssueProceedsInfo(CreditorLoanAgreementValidationParameters queryParameters)
            => await GetCashReceiptOfDebtIssueProceedsInfoQuery.Query(queryParameters, _dapperCtx);

        public async Task<OperationResult<CashDisbursementForLoanInstallmentPaymentInfo>> GetCashDisbursementForLoanInstallmentPaymentInfo(GetLoanInstallmentInfoParameters queryParameters)
            => await GetCashDisbursementForLoanInstallmentPaymentInfoQuery.Query(queryParameters, _dapperCtx);

        public async Task<OperationResult<CreditorIsOwedThisLoanInstallmentValidationInfo>> GetCreditorIsOwedThisLoanInstallmentValidationInfo(GetLoanInstallmentInfoParameters queryParameters)
            => await GetFinancierToLoanInstallmentValidationInfoQuery.Query(queryParameters, _dapperCtx);

        public async Task<OperationResult<CashAccountTransactionDetail>> GetCashAccountTransactionDetail(GetCashAccountTransactionDetailParameters queryParameters)
            => await GetCashAccountTransactionDetailsQuery.Query(queryParameters, _dapperCtx);

        public async Task<OperationResult<PagedList<CashAccountTransactionListItem>>> GetCashAccountTransactionListItem(GetCashAccountTransactionListItemsParameters queryParameters)
            => await GetCashAccountTransactionListItemQuery.Query(queryParameters, _dapperCtx);

        public async Task<OperationResult<Guid>> GetInvestorIdForStockSubscription(GetInvestorIdForStockSubscriptionParameter queryParameters)
            => await GetInvestorIdForStockSubscriptionQuery.Query(queryParameters, _dapperCtx);

        public async Task<OperationResult<Guid>> GetInvestorIdForDividendDeclaration(GetDividendDeclarationParameter queryParameters)
            => await GetInvestorIdForDividendDeclarationQuery.Query(queryParameters, _dapperCtx);

        public async Task<OperationResult<VerificationOfCashDepositStockIssueProceeds>> VerifyCashDepositOfStockIssueProceeds(GetStockSubscriptionParameter queryParameters)
            => await VerifyCashDepositOfStockIssueProceedsQuery.Query(queryParameters, _dapperCtx);

        public async Task<OperationResult<DividendDeclarationDetails>> GetDividendDeclarationDetails(GetDividendDeclarationParameter queryParameters)
            => await GetDividendDeclarationDetailsQuery.Query(queryParameters, _dapperCtx);

        public async Task<OperationResult<List<TimeCardPaymentInfo>>> GetTimeCardPaymentInfo(GetTimeCardPaymentInfoParameter queryParameters)
            => await GetTimeCardPaymentInfoQuery.Query(queryParameters, _dapperCtx);
    }
}