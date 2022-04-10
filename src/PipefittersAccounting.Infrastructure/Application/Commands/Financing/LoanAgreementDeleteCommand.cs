using PipefittersAccounting.Core.Financing.LoanAgreementAggregate;
using PipefittersAccounting.Core.Financing.LoanAgreementAggregate.ValueObjects;
using PipefittersAccounting.Infrastructure.Interfaces;
using PipefittersAccounting.Core.Interfaces.Financing;
using PipefittersAccounting.SharedKernel.CommonValueObjects;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedModel.WriteModels.Financing;

namespace PipefittersAccounting.Infrastructure.Application.Commands.Financing
{
    public class LoanAgreementDeleteCommand
    {
        public static async Task<OperationResult<bool>> Execute
        (
            DeleteLoanAgreementInfo model,
            ILoanAgreementAggregateRepository repo,
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

                OperationResult<LoanAgreement> result = await repo.GetByIdAsync(model.LoanId);

                if (!result.Success)
                {
                    return OperationResult<bool>.CreateFailure(result.NonSuccessMessage);
                }

                repo.Delete(result.Result);
                await unitOfWork.Commit();

                return OperationResult<bool>.CreateSuccessResult(true);
            }
            catch (Exception ex)
            {
                return OperationResult<bool>.CreateFailure(ex.Message);
            }
        }
    }
}