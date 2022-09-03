using Fluxor;
using PipefittersAccounting.UI.Store.UseCases.Finance.StockSubscription.GetStockSubscriptions.Actions;


namespace PipefittersAccounting.UI.Services.Fluxor.Financing
{
    public class StateFacade
    {
        private readonly IDispatcher? _dispatcher;

        public StateFacade(IDispatcher dispatcher) => _dispatcher = dispatcher;

        public void LoadStockSubscriptionsUnfiltered(int pageNumber, int pageSize)
            => _dispatcher!.Dispatch(new LoadStockSubscriptionsUnfilteredAction(pageNumber, pageSize));

        public void LoadStockSubscriptionsFiltered(string investorName, int pageNumber, int pageSize)
            => _dispatcher!.Dispatch(new LoadStockSubscriptionsFilteredAction(investorName, pageNumber, pageSize));
    }
}