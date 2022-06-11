using PipefittersAccounting.Core.Financing.LoanAgreementAggregate;
using PipefittersAccounting.Core.Financing.LoanAgreementAggregate.ValueObjects;
using PipefittersAccounting.Core.Interfaces.Financing;
using PipefittersAccounting.Infrastructure.Interfaces;
using PipefittersAccounting.Infrastructure.Interfaces.Financing;
using PipefittersAccounting.SharedKernel.CommonValueObjects;
using PipefittersAccounting.SharedKernel;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedModel.WriteModels.Financing;

namespace PipefittersAccounting.Infrastructure.Application.Commands.Financing.LoanAgreementAggregate
{
    public class LoanAgreementDeleteCommand
    {
        public static async Task<OperationResult<bool>> Process
        (
            LoanAgreementWriteModel model,
            ILoanAgreementAggregateRepository repo,
            ILoanAgreementValidationService validationService,
            IUnitOfWork unitOfWork
        )
        {
            try
            {
                OperationResult<bool> exist = await repo.Exists(model.LoanId);
                if (!exist.Result)
                {
                    string errMsg = $"Delete loan agreement failed! Unable to locate a loan agreement with this Id: {model.LoanId}!";
                    return OperationResult<bool>.CreateFailure(errMsg);
                }

                ValidationResult validationResult = await validationService.IsValidDeleteLoanAgreementInfo(model);

                if (validationResult.IsValid)
                {
                    OperationResult<LoanAgreement> result = await repo.GetByIdAsync(model.LoanId);

                    if (!result.Success)
                    {
                        return OperationResult<bool>.CreateFailure(result.NonSuccessMessage);
                    }

                    repo.Delete(result.Result);
                    await unitOfWork.Commit();

                    return OperationResult<bool>.CreateSuccessResult(true);
                }
                else
                {
                    return OperationResult<bool>.CreateFailure(validationResult.Messages[0]);
                }
            }
            catch (Exception ex)
            {
                return OperationResult<bool>.CreateFailure(ex.Message);
            }
        }
    }
}