using PipefittersAccounting.Infrastructure.Application.Queries.Financing.CashAccountAggregate;
using PipefittersAccounting.Infrastructure.Persistence.DatabaseContext;
using PipefittersAccounting.Infrastructure.Interfaces.Financing;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedModel.ReadModels;
using PipefittersAccounting.SharedModel.Readmodels.Financing;

namespace PipefittersAccounting.Infrastructure.Application.Services.Financing
{
    public class CashAccountQueryService : ICashAccountQueryService
    {
        private readonly DapperContext _dapperCtx;

        public CashAccountQueryService(DapperContext ctx) => _dapperCtx = ctx;

        public async Task<OperationResult<FinancierIdValidationModel>>
            GetFinancierIdValidationModel(FinancierIdValidationParams queryParameters)
                => await GetFinancierIdValidationModelQuery.Query(queryParameters, _dapperCtx);

        public async Task<OperationResult<CreditorHasLoanAgreeValidationModel>>
            GetCreditorHasLoanAgreeValidationModel(CreditorHasLoanAgreeValidationParams queryParameters)
                => await GetCreditorHasLoanAgreeValidationModelQuery.Query(queryParameters, _dapperCtx);

        public async Task<OperationResult<DepositLoanProceedsValidationModel>>
            GetReceiptLoanProceedsValidationModel(ReceiptLoanProceedsValidationParams queryParameters)
                => await GetReceiptLoanProceedsValidationModelQuery.Query(queryParameters, _dapperCtx);

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

        public async Task<OperationResult<ExternalAgentIdentificationInfo>> GetExternalAgentIdentificationInfo(ExternalAgentIdentificationParameters queryParameters)
            => await GetExternalAgentIdentificationInfoQuery.Query(queryParameters, _dapperCtx);

        public async Task<OperationResult<EconomicEventIdentificationInfo>> GetEconomicEventIdentificationInfo(EconomicEventIdentificationParameters queryParameters)
            => await GetEconomicEventIdentificationInfoQuery.Query(queryParameters, _dapperCtx);

        public async Task<OperationResult<CreditorIssuedLoanAgreementValidationInfo>> GetCreditorIssuedLoanAgreementValidationInfo(CreditorIssuedLoanAgreementValidationParameters queryParameters)
            => await GetCreditorIssuedLoanAgreementValidationInfoQuery.Query(queryParameters, _dapperCtx);

        public async Task<OperationResult<CashReceiptOfDebtIssueProceedsInfo>> GetCashReceiptOfDebtIssueProceedsInfo(CashReceiptOfDebtIssueProceedsParameters queryParameters)
            => await GetCashReceiptOfDebtIssueProceedsInfoQuery.Query(queryParameters, _dapperCtx);

        public async Task<OperationResult<CashDisbursementForLoanInstallmentPaymentInfo>> GetCashDisbursementForLoanInstallmentPaymentInfo(GetLoanInstallmentInfoParameters queryParameters)
            => await GetCashDisbursementForLoanInstallmentPaymentInfoQuery.Query(queryParameters, _dapperCtx);

        public async Task<OperationResult<FinancierToLoanInstallmentValidationInfo>> GetFinancierToLoanInstallmentValidationInfo(GetLoanInstallmentInfoParameters queryParameters)
            => await GetFinancierToLoanInstallmentValidationInfoQuery.Query(queryParameters, _dapperCtx);
    }
}