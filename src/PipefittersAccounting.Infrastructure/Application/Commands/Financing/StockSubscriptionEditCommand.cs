#pragma warning disable CS8604

using PipefittersAccounting.Core.Financing.StockSubscriptionAggregate;
using PipefittersAccounting.Core.Financing.StockSubscriptionAggregate.ValueObjects;
using PipefittersAccounting.Core.Interfaces.Financing;
using PipefittersAccounting.Infrastructure.Interfaces;
using PipefittersAccounting.Infrastructure.Interfaces.Financing;
using PipefittersAccounting.SharedModel.WriteModels.Financing;
using PipefittersAccounting.SharedKernel.CommonValueObjects;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedKernel;

namespace PipefittersAccounting.Infrastructure.Application.Commands.Financing
{
    public class StockSubscriptionEditCommand
    {
        public static async Task<OperationResult<bool>> Process
        (
            StockSubscriptionWriteModel model,
            IStockSubscriptionAggregateRepository repository,
            IStockSubscriptionValidationService validationService,
            IUnitOfWork uow
        )
        {
            ValidationResult validationResult = await validationService.IsValidEditStockSubscriptionInfo(model);

            if (validationResult.IsValid)
            {
                try
                {
                    OperationResult<StockSubscription> getResult = await repository.GetStockSubscriptionByIdAsync(model.StockId);
                    if (getResult.Success)
                    {
                        StockSubscription subscription = getResult.Result;
                        subscription.UpdateStockIssueDate(StockIssueDate.Create(model.StockIssueDate));
                        subscription.UpdateSharesIssured(SharesIssured.Create(model.SharesIssued));
                        subscription.UpdatePricePerShare(PricePerShare.Create(model.PricePerShare));
                        subscription.UpdateUserId(EntityGuidID.Create(model.UserId));

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