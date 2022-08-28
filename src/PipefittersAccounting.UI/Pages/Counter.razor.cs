using Fluxor;
using Microsoft.AspNetCore.Components;
using PipefittersAccounting.UI.Store;
using PipefittersAccounting.UI.Store.CounterUseCase;

namespace PipefittersAccounting.UI.Pages
{
    public partial class Counter
    {
        [Inject] private IState<CounterState>? CounterState { get; set; }
        [Inject] public IDispatcher? Dispatcher { get; set; }

        private void IncrementCounter()
        {
            var action = new IncrementCounterAction();
            Dispatcher!.Dispatch(action);
        }
    }
}