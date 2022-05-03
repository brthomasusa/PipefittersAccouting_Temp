using PipefittersAccounting.Core.Financing.FinancierAggregate;
using PipefittersAccounting.Infrastructure.Interfaces;
using PipefittersAccounting.Core.Interfaces.Financing;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedModel.WriteModels.Financing;

namespace PipefittersAccounting.Infrastructure.Application.Commands.Financing
{
    public class FinancierDeleteCommand
    {
        public static async Task<OperationResult<bool>> Execute
        (
            DeleteFinancierInfo model,
            IFinancierAggregateRepository repo,
            IUnitOfWork unitOfWork
        )
        {
            try
            {
                OperationResult<bool> result = await repo.Exists(model.Id);

                if (!result.Success)
                {
                    string errMsg = $"Delete failed, a financier with id: {model.Id} could not be found!";
                    return OperationResult<bool>.CreateFailure(errMsg);
                }

                OperationResult<Financier> getResult = await repo.GetByIdAsync(model.Id);

                if (getResult.Success)
                {
                    Financier financier = getResult.Result;

                    OperationResult<bool> deleteResult = repo.Delete(financier);

                    if (deleteResult.Success)
                    {
                        await unitOfWork.Commit();
                        return OperationResult<bool>.CreateSuccessResult(true);
                    }
                    else
                    {
                        return OperationResult<bool>.CreateFailure(deleteResult.NonSuccessMessage);
                    }
                }
                else
                {
                    return OperationResult<bool>.CreateFailure(getResult.NonSuccessMessage);
                }
            }
            catch (Exception ex)
            {
                return OperationResult<bool>.CreateFailure(ex.Message);
            }
        }
    }
}