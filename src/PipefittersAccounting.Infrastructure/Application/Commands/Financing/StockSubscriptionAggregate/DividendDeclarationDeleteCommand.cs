#pragma warning disable CS8604

using PipefittersAccounting.Core.Financing.StockSubscriptionAggregate;
using PipefittersAccounting.Core.Interfaces.Financing;
using PipefittersAccounting.Infrastructure.Interfaces;
using PipefittersAccounting.Infrastructure.Interfaces.Financing;
using PipefittersAccounting.SharedModel.WriteModels.Financing;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedKernel;

namespace PipefittersAccounting.Infrastructure.Application.Commands.Financing.StockSubscriptionAggregate
{
    public class DividendDeclarationDeleteCommand
    {
        public static async Task<OperationResult<bool>> Process
        (
            DividendDeclarationWriteModel model,
            IStockSubscriptionAggregateRepository repository,
            IStockSubscriptionValidationService validationService,
            IUnitOfWork uow
        )
        {
            ValidationResult validationResult = await validationService.IsValidDeleteDividendDeclarationInfo(model);

            if (validationResult.IsValid)
            {
                try
                {
                    OperationResult<bool> result = await repository.DeleteDividendDeclarationAsync(model.DividendId);

                    if (result.Success)
                    {
                        await uow.Commit();
                        return OperationResult<bool>.CreateSuccessResult(true);
                    }
                    else
                    {
                        return OperationResult<bool>.CreateFailure(result.NonSuccessMessage);
                    }
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