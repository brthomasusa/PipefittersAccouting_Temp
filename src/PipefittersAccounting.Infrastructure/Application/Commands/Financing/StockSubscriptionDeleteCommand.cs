#pragma warning disable CS8604

using PipefittersAccounting.Core.Interfaces.Financing;
using PipefittersAccounting.Infrastructure.Interfaces;
using PipefittersAccounting.Infrastructure.Interfaces.Financing;
using PipefittersAccounting.SharedModel.WriteModels.Financing;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedKernel;

namespace PipefittersAccounting.Infrastructure.Application.Commands.Financing
{
    public class StockSubscriptionDeleteCommand
    {
        public static async Task<OperationResult<bool>> Process
        (
            StockSubscriptionWriteModel model,
            IStockSubscriptionAggregateRepository repository,
            IStockSubscriptionValidationService validationService,
            IUnitOfWork uow
        )
        {
            ValidationResult validationResult = await validationService.IsValidDeleteStockSubscriptionInfo(model);

            if (validationResult.IsValid)
            {
                try
                {
                    await repository.DeleteStockSubscriptionAsync(model.StockId);
                    await uow.Commit();

                    return OperationResult<bool>.CreateSuccessResult(true);
                }
                catch (Exception ex)
                {
                    return OperationResult<bool>.CreateFailure(ex.Message);
                }
            }
            else
            {
                return OperationResult<bool>.CreateFailure(validationResult.Messages[0]);
            }
        }
    }
}