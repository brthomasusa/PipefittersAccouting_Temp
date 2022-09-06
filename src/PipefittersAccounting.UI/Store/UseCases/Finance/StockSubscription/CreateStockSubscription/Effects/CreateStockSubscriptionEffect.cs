using Fluxor;
using Blazorise;
using PipefittersAccounting.UI.Store.UseCases.Finance.StockSubscription.CreateStockSubscription.Actions;
using PipefittersAccounting.SharedModel.Readmodels.Financing;
using PipefittersAccounting.UI.Interfaces;
using PipefittersAccounting.UI.Utilities;

namespace PipefittersAccounting.UI.Store.UseCases.Finance.StockSubscription.CreateStockSubscription.Effects
{
    public class CreateStockSubscriptionEffect : Effect<CreateStockSubscriptionAction>
    {
        private IStockSubscriptionRepository? _stockSubscriptionService;
        private IMessageService? _messageService;

        public CreateStockSubscriptionEffect(IStockSubscriptionRepository repo, IMessageService messageSvc)
            => (_stockSubscriptionService, _messageService) = (repo, messageSvc);

        public override async Task HandleAsync(CreateStockSubscriptionAction action, IDispatcher dispatcher)
        {
            OperationResult<StockSubscriptionReadModel> createResult
                = await _stockSubscriptionService!.CreateStockSubscription(action.StockSubscriptionWriteModel);

            if (createResult.Success)
            {
                GetStockSubscriptionListItem parameter = new() { Page = 1, PageSize = 10 };

                OperationResult<PagingResponse<StockSubscriptionListItem>> result =
                    await _stockSubscriptionService!.GetStockSubscriptionListItems(parameter);

                if (result.Success)
                {
                    dispatcher.Dispatch(new CreateStockSubscriptionSuccessAction(result.Result!, createResult.Result));
                }
                else
                {
                    dispatcher.Dispatch(new CreateStockSubscriptionFailureAction(result.NonSuccessMessage));
                }
            }
            else
            {
                dispatcher.Dispatch(new CreateStockSubscriptionFailureAction(createResult.NonSuccessMessage));
            }
        }

    }
}