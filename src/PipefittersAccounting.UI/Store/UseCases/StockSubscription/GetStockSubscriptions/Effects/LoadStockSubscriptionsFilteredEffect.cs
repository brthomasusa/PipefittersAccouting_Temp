using Fluxor;
using Blazorise;
using PipefittersAccounting.UI.Store.UseCases.StockSubscription.GetStockSubscriptions.Actions;
using PipefittersAccounting.SharedModel.Readmodels.Financing;
using PipefittersAccounting.UI.Interfaces;
using PipefittersAccounting.UI.Utilities;

namespace PipefittersAccounting.UI.Store.UseCases.StockSubscription.GetStockSubscriptions.Effects
{
    public class LoadStockSubscriptionsFilteredEffect : Effect<LoadStockSubscriptionsFilteredAction>
    {
        private IStockSubscriptionRepository? _stockSubscriptionService;
        private IMessageService? _messageService;

        public LoadStockSubscriptionsFilteredEffect(IStockSubscriptionRepository repo, IMessageService messageSvc)
            => (_stockSubscriptionService, _messageService) = (repo, messageSvc);

        public override async Task HandleAsync(LoadStockSubscriptionsFilteredAction action, IDispatcher dispatcher)
        {
            try
            {
                GetStockSubscriptionListItemByInvestorName parameter =
                    new()
                    {
                        InvestorName = action.InvestorName,
                        Page = action.PageNumber,
                        PageSize = action.PageSize
                    };

                OperationResult<PagingResponse<StockSubscriptionListItem>> result =
                    await _stockSubscriptionService!.GetStockSubscriptionListItems(parameter);

                if (result.Success)
                {
                    dispatcher.Dispatch(new LoadStockSubscriptionsSuccessAction(result.Result!));
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