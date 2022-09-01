using Fluxor;
using PipefittersAccounting.UI.Store.UseCases.StockSubscription.GetStockSubscriptions.Actions.LoadStockSubscriptions;


namespace PipefittersAccounting.UI.Services.Fluxor
{
    public class StateFacade
    {
        private readonly IDispatcher? _dispatcher;

        public StateFacade(IDispatcher dispatcher) => _dispatcher = dispatcher;

        public void LoadStockSubscriptions()
        {
            Console.WriteLine("StateFacade.LoadStockSubscriptions called...");
            _dispatcher!.Dispatch(new LoadStockSubscriptionAction());
        }
    }
}