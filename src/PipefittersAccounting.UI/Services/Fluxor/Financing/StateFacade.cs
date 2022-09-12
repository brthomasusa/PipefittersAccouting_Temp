using Fluxor;
using PipefittersAccounting.SharedModel.WriteModels.Financing;
using PipefittersAccounting.UI.Store.UseCases.Finance.StockSubscription.GetStockSubscriptions.Actions;
using PipefittersAccounting.UI.Store.UseCases.Finance.StockSubscription.CreateStockSubscription.Actions;

namespace PipefittersAccounting.UI.Services.Fluxor.Financing
{
    public class StateFacade
    {
        private readonly IDispatcher? _dispatcher;

        public StateFacade(IDispatcher dispatcher) => _dispatcher = dispatcher;

        public void LoadStockSubscriptionsWithSearchTerm(string investorName, int pageNumber, int pageSize)
            => _dispatcher!.Dispatch(new LoadStockSubscriptionsWithSearchTermAction(investorName, pageNumber, pageSize));

        public void LoadStockSubscriptions(string filterName, int pageNumber, int pageSize)
            => _dispatcher!.Dispatch(new LoadStockSubscriptionsAction(filterName, pageNumber, pageSize));

        public void UpdateStockSubscriptionCreateModel(StockSubscriptionWriteModel subscription)
            => _dispatcher!.Dispatch(new CreateStockSubscriptionAction(subscription));
    }
}