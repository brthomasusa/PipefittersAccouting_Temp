using PipefittersAccounting.SharedKernel;
using PipefittersAccounting.SharedModel.WriteModels.Financing;

namespace PipefittersAccounting.Infrastructure.Interfaces.Financing
{
    public interface ILoanAgreementValidationService
    {
        Task<ValidationResult> IsValidCreateLoanAgreementInfo(LoanAgreementWriteModel writeModel);

        Task<ValidationResult> IsValidDeleteLoanAgreementInfo(LoanAgreementWriteModel writeModel);
    }
}