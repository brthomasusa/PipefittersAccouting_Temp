using Fluxor;
using Blazorise;
using PipefittersAccounting.UI.Store.UseCases.Finance.StockSubscription.GetStockSubscriptions.Actions;
using PipefittersAccounting.SharedModel.Readmodels.Financing;
using PipefittersAccounting.UI.Interfaces;
using PipefittersAccounting.UI.Utilities;

namespace PipefittersAccounting.UI.Store.UseCases.Finance.StockSubscription.GetStockSubscriptions.Effects
{
    public class LoadStockSubscriptionsEffect : Effect<LoadStockSubscriptionsAction>
    {
        private IStockSubscriptionRepository? _stockSubscriptionService;
        private IMessageService? _messageService;

        public LoadStockSubscriptionsEffect(IStockSubscriptionRepository repo, IMessageService messageSvc)
            => (_stockSubscriptionService, _messageService) = (repo, messageSvc);

        public override async Task HandleAsync(LoadStockSubscriptionsAction action, IDispatcher dispatcher)
        {
            switch (action.filterName)
            {
                case "all":
                    await GetSubscriptionsProceedsAll(action, dispatcher);
                    break;
                case "rcvd":
                    await GetSubscriptionsProceedsReceived(action, dispatcher);
                    break;
                case "notrcvd":
                    await GetSubscriptionsProceedsNotReceived(action, dispatcher);
                    break;
                default:
                    await _messageService!.Error($"Unknown stock subscription filter type: {action.filterName}", "System Error");
                    break;
            }
        }

        private async Task GetSubscriptionsProceedsAll
        (
            LoadStockSubscriptionsAction action,
            IDispatcher dispatcher
        )
        {
            try
            {
                GetStockSubscriptionListItem parameter =
                    new()
                    {
                        Page = action.PageNumber,
                        PageSize = action.PageSize
                    };

                OperationResult<PagingResponse<StockSubscriptionListItem>> result =
                    await _stockSubscriptionService!.GetStockSubscriptionListItems(parameter);

                if (result.Success)
                {
                    dispatcher.Dispatch(new LoadStockSubscriptionsSuccessAction(result.Result!, action.filterName, action.PageSize));
                }
                else
                {
                    dispatcher.Dispatch(new LoadStockSubscriptionsFailureAction(result.NonSuccessMessage));
                }
            }
            catch (Exception e)
            {
                dispatcher.Dispatch(new LoadStockSubscriptionsFailureAction(e.Message));
            }
        }

        private async Task GetSubscriptionsProceedsReceived
        (
            LoadStockSubscriptionsAction action,
            IDispatcher dispatcher
        )
        {
            try
            {
                GetStockSubscriptionListItem parameter =
                    new()
                    {
                        Page = action.PageNumber,
                        PageSize = action.PageSize
                    };

                OperationResult<PagingResponse<StockSubscriptionListItem>> result =
                    await _stockSubscriptionService!.GetStockSubscriptionListItemsFundsRcvd(parameter);

                if (result.Success)
                {
                    dispatcher.Dispatch(new LoadStockSubscriptionsSuccessAction(result.Result!, action.filterName, action.PageSize));
                }
                else
                {
                    dispatcher.Dispatch(new LoadStockSubscriptionsFailureAction(result.NonSuccessMessage));
                }
            }
            catch (Exception e)
            {
                dispatcher.Dispatch(new LoadStockSubscriptionsFailureAction(e.Message));
            }
        }

        private async Task GetSubscriptionsProceedsNotReceived
        (
            LoadStockSubscriptionsAction action,
            IDispatcher dispatcher
        )
        {
            try
            {
                GetStockSubscriptionListItem parameter =
                    new()
                    {
                        Page = action.PageNumber,
                        PageSize = action.PageSize
                    };

                OperationResult<PagingResponse<StockSubscriptionListItem>> result =
                    await _stockSubscriptionService!.GetStockSubscriptionListItemsFundsNotRcvd(parameter);

                if (result.Success)
                {
                    dispatcher.Dispatch(new LoadStockSubscriptionsSuccessAction(result.Result!, action.filterName, action.PageSize));
                }
                else
                {
                    dispatcher.Dispatch(new LoadStockSubscriptionsFailureAction(result.NonSuccessMessage));
                }
            }
            catch (Exception e)
            {
                dispatcher.Dispatch(new LoadStockSubscriptionsFailureAction(e.Message));
            }
        }
    }
}