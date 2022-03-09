using PipefittersAccounting.Core.Financing.FinancierAggregate;
using PipefittersAccounting.Infrastructure.Interfaces;
using PipefittersAccounting.Infrastructure.Interfaces.Financing;
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
                if (await repo.Exists(model.Id) == false)
                {
                    string errMsg = $"Delete failed, a financier with id: {model.Id} could not be found!";
                    return OperationResult<bool>.CreateFailure(errMsg);
                }

                Financier financier = await repo.GetByIdAsync(model.Id);

                repo.Delete(financier);
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