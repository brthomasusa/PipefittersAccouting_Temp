using PipefittersAccounting.SharedModel.ReadModels;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedModel.Readmodels.Financing;

namespace PipefittersAccounting.Infrastructure.Interfaces.Financing
{
    public interface ICashAccountQueryService
    {
        Task<OperationResult<FinancierIdValidationModel>>
            GetFinancierIdValidationModel(FinancierIdValidationParams queryParameters);

        Task<OperationResult<CreditorHasLoanAgreeValidationModel>>
            GetCreditorHasLoanAgreeValidationModel(CreditorHasLoanAgreeValidationParams queryParameters);

        Task<OperationResult<DepositLoanProceedsValidationModel>>
            GetReceiptLoanProceedsValidationModel(ReceiptLoanProceedsValidationParams queryParameters);

        Task<OperationResult<DisburesementLoanPymtValidationModel>>
            GetDisburesementLoanPymtValidationModel(DisburesementLoanPymtValidationParams queryParameters);

        Task<OperationResult<CashAccountDetail>> GetCashAccountDetails(GetCashAccount queryParameters);

        Task<OperationResult<PagedList<CashAccountListItem>>> GetCashAccountListItems(GetCashAccounts queryParameters);
    }
}