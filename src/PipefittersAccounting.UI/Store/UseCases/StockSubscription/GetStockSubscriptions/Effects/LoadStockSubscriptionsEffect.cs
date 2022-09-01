using Fluxor;
using Blazorise;
using PipefittersAccounting.UI.Store.UseCases.StockSubscription.GetStockSubscriptions.Actions.LoadStockSubscriptions;
using PipefittersAccounting.SharedModel.Readmodels.Financing;
using PipefittersAccounting.UI.Interfaces;
using PipefittersAccounting.UI.Utilities;

namespace PipefittersAccounting.UI.Store.UseCases.StockSubscription.GetStockSubscriptions.Effects
{
    public class LoadStockSubscriptionsEffect : Effect<LoadStockSubscriptionAction>
    {
        private IStockSubscriptionRepository? _stockSubscriptionService;
        private IMessageService? _messageService;

        public LoadStockSubscriptionsEffect(IStockSubscriptionRepository repo, IMessageService messageSvc)
            => (_stockSubscriptionService, _messageService) = (repo, messageSvc);

        public override async Task HandleAsync(LoadStockSubscriptionAction action, IDispatcher dispatcher)
        {
            try
            {
                Console.WriteLine("LoadStockSubscriptionsEffect.HandleAsync called...");

                GetStockSubscriptionListItem parameter = new() { Page = 1, PageSize = 10 };

                OperationResult<PagingResponse<StockSubscriptionListItem>> result =
                    await _stockSubscriptionService!.GetStockSubscriptionListItems(parameter);

                if (result.Success)
                {
                    Console.WriteLine($"LoadStockSubscriptionsEffect.HandleAsync called..., {result.Result!.Items!.Count} stock subscriptions retrieved.");
                    dispatcher.Dispatch(new LoadStockSubscriptionSuccessAction(result.Result!));
                }
                else
                {
                    dispatcher.Dispatch(new LoadStockSubscriptionFailureAction(result.NonSuccessMessage));
                }
            }
            catch (Exception e)
            {
                dispatcher.Dispatch(new LoadStockSubscriptionFailureAction(e.Message));
            }

        }
    }
}