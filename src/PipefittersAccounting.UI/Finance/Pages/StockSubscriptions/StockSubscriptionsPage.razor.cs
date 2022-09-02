using Microsoft.AspNetCore.Components;
using Blazorise;
using Blazorise.Snackbar;
using Fluxor;
using PipefittersAccounting.SharedModel.ReadModels;
using PipefittersAccounting.SharedModel.Readmodels.Financing;
using PipefittersAccounting.UI.Interfaces;
using PipefittersAccounting.UI.Utilities;
using PipefittersAccounting.UI.Store.State.StockSubscription;
using PipefittersAccounting.UI.Services.Fluxor;

namespace PipefittersAccounting.UI.Finance.Pages.StockSubscriptions
{
    public partial class StockSubscriptionsPage
    {
        private Guid _selectedStockId;
        private string _placeHolderTextForSearch = "Search by investor name";
        private string? _snackBarMessage;
        private Snackbar? _snackbar;
        private List<StockSubscriptionListItem>? _subscriptionList;
        private StockSubscriptionReadModel? _selectedSubscription;
        private MetaData? _metaData;
        private Func<int, int, Task>? _pagerChangedEventHandler;

        [Inject] private IState<StockSubscriptionState>? _stockSubscriptionState { get; set; }
        [Inject] private StateFacade? _facade { get; set; }

        [Inject] public IStockSubscriptionRepository? StockSubscriptionService { get; set; }
        [Inject] public IMessageService? MessageService { get; set; }
        [Inject] public NavigationManager? NavManager { get; set; }

        protected async override Task OnInitializedAsync()
        {
            _pagerChangedEventHandler = GetAllStockSubscriptions;
            await _pagerChangedEventHandler.Invoke(1, 10);
            await base.OnInitializedAsync();
        }

        private async Task GetAllStockSubscriptions(int pageNumber, int pageSize)
        {
            if (_stockSubscriptionState!.Value.CurrentSubscriptions is null)
            {
                _facade!.LoadStockSubscriptionsUnfiltered(pageNumber, pageSize);
            }

            await Task.CompletedTask;
        }

        private async Task GetAllStockSubscriptions(string investorName, int pageNumber, int pageSize)
        {
            _facade!.LoadStockSubscriptionsFiltered(investorName, pageNumber, pageSize);
            await Task.CompletedTask;
        }

        private void OnActionItemClicked(string action, Guid stockId)
        {
            NavManager!.NavigateTo
            (
                action switch
                {
                    "Edit" => $"Finance/Pages/LoanAgreements/LoanAgreementEditPage/{stockId}",
                    _ => throw new ArgumentOutOfRangeException(nameof(action), $"Unexpected menu item: {action}"),
                }
            );
        }

        private async Task GetStockSubscription(Guid stockId)
        {
            GetStockSubscriptionParameter queryParam = new() { StockId = stockId };

            OperationResult<StockSubscriptionReadModel> result =
                await StockSubscriptionService!.GetStockSubscriptionReadModel(queryParam);

            if (result.Success)
            {
                _selectedSubscription = result.Result;
                await InvokeAsync(StateHasChanged);
            }
            else
            {
                await MessageService!.Error($"Error while retrieving stock subscription: {result.NonSuccessMessage}", "Error");
            }
        }

        private async Task SearchChanged(string searchTerm) => await GetAllStockSubscriptions(searchTerm, 1, 10);

        private void ShowDetailDialog(Guid stockId) => _selectedStockId = stockId;
    }
}