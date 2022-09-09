using Fluxor;
using PipefittersAccounting.UI.Store.UseCases.Finance.StockSubscription.GetStockSubscriptions.Actions;


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
    }
}