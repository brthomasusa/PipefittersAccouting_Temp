using PipefittersAccounting.SharedModel.ReadModels;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedModel.Readmodels.CashManagement;
using PipefittersAccounting.SharedModel.Readmodels.Financing;

namespace PipefittersAccounting.Infrastructure.Interfaces.Financing
{
    public interface ILoanAgreementQueryService
    {
        Task<OperationResult<LoanAgreementReadModel>> GetLoanAgreementDetails(GetLoanAgreement queryParameters);
        Task<OperationResult<PagedList<LoanAgreementListItem>>> GetLoanAgreementListItems(GetLoanAgreements queryParameters);
        Task<OperationResult<PagedList<LoanAgreementListItem>>> GetLoanAgreementListItems(GetLoanAgreementByLoanNumber queryParameters);
        Task<OperationResult<Guid>> GetLoanIdOfDuplicationLoanAgreement(GetDuplicateLoanAgreement queryParameters);
        Task<OperationResult<decimal>> VerifyCashDepositForDebtIssueProceeds(ReceiptLoanProceedsValidationParams queryParameters);
        Task<OperationResult<Guid>> VerifyCreditorIsLinkedToLoanAgreement(ReceiptLoanProceedsValidationParams queryParameters);
    }
}