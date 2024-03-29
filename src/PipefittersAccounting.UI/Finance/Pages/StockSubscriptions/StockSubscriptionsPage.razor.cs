using Microsoft.AspNetCore.Components;
using Blazorise;
using Blazorise.Snackbar;
using Fluxor;

using PipefittersAccounting.SharedModel.Readmodels.Financing;
using PipefittersAccounting.UI.Interfaces;
using PipefittersAccounting.UI.Utilities;
using PipefittersAccounting.UI.Store.State.Finance.StockSubscription;
using PipefittersAccounting.UI.Services.Fluxor.Financing;

namespace PipefittersAccounting.UI.Finance.Pages.StockSubscriptions
{
    public partial class StockSubscriptionsPage
    {
        private string _placeHolderTextForSearch = "Search by investor name";
        private string? _snackBarMessage;
        private Snackbar? _snackbar;
        private Guid _selectedStockId;
        private StockSubscriptionReadModel? _selectedSubscription;
        private Func<int, int, Task> _pagerChangedEventHandler => GetAllStockSubscriptions;

        [Inject] private IState<StockSubscriptionsState>? _stockSubscriptionState { get; set; }
        [Inject] private StateFacade? _facade { get; set; }

        [Inject] public IStockSubscriptionRepository? StockSubscriptionService { get; set; }
        [Inject] public IMessageService? MessageService { get; set; }
        [Inject] public NavigationManager? NavManager { get; set; }

        protected async override Task OnInitializedAsync()
        {
            if (_stockSubscriptionState!.Value.StockSubscriptionList is null)
            {
                _facade!.LoadStockSubscriptions
                    (
                        _stockSubscriptionState!.Value.SubscriptionListFilter,
                        _stockSubscriptionState!.Value.PageNumber,
                        _stockSubscriptionState!.Value.PageSize
                    );
            }

            await base.OnInitializedAsync();
        }

        private async Task GetAllStockSubscriptions(int pageNumber, int pageSize)
        {
            _facade!.LoadStockSubscriptions
                (
                    _stockSubscriptionState!.Value.SubscriptionListFilter,
                    pageNumber,
                    pageSize
                );

            await Task.CompletedTask;
        }

        private async Task GetStockSubscriptionsBySearchTerm(string investorName, int pageNumber, int pageSize)
        {
            _facade!.LoadStockSubscriptionsWithSearchTerm
            (
                investorName,
                pageNumber,
                pageSize
            );

            await Task.CompletedTask;
        }

        private async Task GetFilteredStockSubscriptions(string filterName)
        {
            _facade!.LoadStockSubscriptions
            (
                filterName,
                1,
                _stockSubscriptionState!.Value.PageSize
            );

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

        private async Task SearchChanged(string searchTerm)
            => await GetStockSubscriptionsBySearchTerm(searchTerm, 1, _stockSubscriptionState!.Value.PageSize);

        private async Task FilterChanged(string filterName)
            => await GetFilteredStockSubscriptions(filterName);

        private void ShowDetailDialog(Guid stockId) => _selectedStockId = stockId;

        private string ConvertEmptyDate(DateTime date)
            => date == default ? string.Empty : date.ToShortDateString();
    }
}