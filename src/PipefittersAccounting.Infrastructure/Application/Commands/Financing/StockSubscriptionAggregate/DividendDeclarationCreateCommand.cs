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
    public class DividendDeclarationCreateCommand
    {
        public static async Task<OperationResult<bool>> Process
        (
            DividendDeclarationWriteModel model,
            IStockSubscriptionAggregateRepository repository,
            IStockSubscriptionValidationService validationService,
            IUnitOfWork uow
        )
        {
            if (model.DividendId == Guid.Empty)
                model.DividendId = Guid.NewGuid();

            ValidationResult validationResult = await validationService.IsValidCreateDividendDeclarationInfo(model);

            if (validationResult.IsValid)
            {
                try
                {
                    OperationResult<StockSubscription> getResult = await repository.GetStockSubscriptionByIdAsync(model.StockId);
                    if (getResult.Success)
                    {
                        StockSubscription subscription = getResult.Result;
                        subscription.AddDividendDeclaration(model.DividendId,
                                                            model.DividendDeclarationDate,
                                                            model.DividendPerShare,
                                                            model.UserId);

                        repository.UpdateStockSubscription(subscription);
                        await uow.Commit();

                        return OperationResult<bool>.CreateSuccessResult(true);
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
            else
            {
                return OperationResult<bool>.CreateFailure(validationResult.Messages[0]);
            }
        }
    }
}