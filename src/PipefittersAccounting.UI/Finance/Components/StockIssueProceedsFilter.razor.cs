using Microsoft.AspNetCore.Components;
using Fluxor;
using PipefittersAccounting.UI.Store.State.Finance.StockSubscription;

namespace PipefittersAccounting.UI.Finance.Components
{
    public partial class StockIssueProceedsFilter
    {
        private string? _selectedFilter;

        [Parameter] public EventCallback<string> FilterSetEventHandler { get; set; }
        [Inject] private IState<StockSubscriptionsState>? _stockSubscriptionState { get; set; }

        protected async override Task OnInitializedAsync()
        {
            _selectedFilter = _stockSubscriptionState!.Value.SubscriptionListFilter switch
            {
                "all" => "All ",
                "rcvd" => "Funds rcvd",
                "notrcvd" => "Funds not rcvd",
                _ => "Unknown filter"
            };

            await Task.CompletedTask;
        }

        private async Task OnFilterChanged(string filterName)
        {
            _selectedFilter = filterName switch
            {
                "all" => "All ",
                "rcvd" => "Funds rcvd",
                "notrcvd" => "Funds not rcvd",
                _ => "Unknown filter"
            };

            await FilterSetEventHandler.InvokeAsync(filterName);
        }
    }
}