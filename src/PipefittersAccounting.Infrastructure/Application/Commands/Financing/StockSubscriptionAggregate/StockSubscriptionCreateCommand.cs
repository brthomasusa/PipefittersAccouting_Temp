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

namespace PipefittersAccounting.Infrastructure.Application.Commands.Financing.StockSubscriptionAggregate
{
    public class StockSubscriptionCreateCommand
    {
        public static async Task<OperationResult<bool>> Process
        (
            StockSubscriptionWriteModel model,
            IStockSubscriptionAggregateRepository repository,
            IStockSubscriptionValidationService validationService,
            IUnitOfWork uow
        )
        {
            if (model.StockId == Guid.Empty)
                model.StockId = Guid.NewGuid();

            ValidationResult validationResult = await validationService.IsValidCreateStockSubscriptionInfo(model);

            if (validationResult.IsValid)
            {
                try
                {
                    StockSubscription subscription = new
                    (
                        EntityGuidID.Create(model.StockId),
                        EntityGuidID.Create(model.FinancierId),
                        StockIssueDate.Create(model.StockIssueDate),
                        SharesIssured.Create(model.SharesIssued),
                        PricePerShare.Create(model.PricePerShare),
                        EntityGuidID.Create(model.UserId)
                    );

                    await repository.AddStockSubscriptionAsync(subscription);
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