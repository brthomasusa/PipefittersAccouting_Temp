using Fluxor;
using Blazorise;
using PipefittersAccounting.UI.Store.UseCases.Finance.StockSubscription.GetStockSubscriptions.Actions;
using PipefittersAccounting.SharedModel.Readmodels.Financing;
using PipefittersAccounting.UI.Interfaces;
using PipefittersAccounting.UI.Utilities;

namespace PipefittersAccounting.UI.Store.UseCases.Finance.StockSubscription.GetStockSubscriptions.Effects
{
    public class LoadStockSubscriptionsWithSearchTermEffect : Effect<LoadStockSubscriptionsWithSearchTermAction>
    {
        private IStockSubscriptionRepository? _stockSubscriptionService;
        private IMessageService? _messageService;

        public LoadStockSubscriptionsWithSearchTermEffect(IStockSubscriptionRepository repo, IMessageService messageSvc)
            => (_stockSubscriptionService, _messageService) = (repo, messageSvc);

        public override async Task HandleAsync(LoadStockSubscriptionsWithSearchTermAction action, IDispatcher dispatcher)
        {
            try
            {
                GetStockSubscriptionListItemByInvestorName parameter =
                    new()
                    {
                        InvestorName = action.SearchTerm,
                        Page = action.PageNumber,
                        PageSize = action.PageSize
                    };

                OperationResult<PagingResponse<StockSubscriptionListItem>> result =
                    await _stockSubscriptionService!.GetStockSubscriptionListItems(parameter);

                if (result.Success)
                {
                    dispatcher.Dispatch(new LoadStockSubscriptionsWithSearchTermSuccessAction(result.Result!));
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