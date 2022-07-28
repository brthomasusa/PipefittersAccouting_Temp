using Microsoft.AspNetCore.Components;
using Blazorise;
using Blazorise.Snackbar;
using PipefittersAccounting.SharedModel.ReadModels;
using PipefittersAccounting.SharedModel.Readmodels.Financing;
using PipefittersAccounting.SharedModel.WriteModels.Financing;
using PipefittersAccounting.UI.Interfaces;
using PipefittersAccounting.UI.Utilities;

namespace PipefittersAccounting.UI.Finance.Pages.Financiers
{
    public partial class FinanciersListPage
    {
        private FinancierListItems? _financierToDelete;
        private FinancierReadModel? _financierDetailModel;
        private bool _isLoading;
        private Modal? _deleteModalRef;
        private string? _modalTitle;
        private string? _modalMessage;
        private string? _snackarMessage;
        private Snackbar? _snackbar;
        private string _placeHolderTextForSearch = "Search by financier's name";
        private GetFinanciers? _getFinanciersParameters;
        private GetFinanciersByName? _getFinanciersbyNameParameters;
        private List<FinancierListItems>? _financierList;
        private MetaData? _metaData;
        private Func<int, int, Task>? _pagerChangedEventHandler;

        [Inject] public IFinanciersHttpService? FinanciersService { get; set; }
        [Inject] public NavigationManager? NavManager { get; set; }
        [Inject] public IMessageService? MessageService { get; set; }

        protected async override Task OnInitializedAsync()
        {
            _pagerChangedEventHandler = GetFinanciers;
            await _pagerChangedEventHandler.Invoke(1, 5);
        }

        private async Task GetFinanciers(int pageNumber, int pageSize)
        {
            _getFinanciersParameters = new() { Page = pageNumber, PageSize = pageSize };

            OperationResult<PagingResponse<FinancierListItems>> result =
                await FinanciersService!.GetFinancierListItems(_getFinanciersParameters);

            if (result.Success)
            {
                _financierList = result.Result.Items;
                _metaData = result.Result.MetaData;
                StateHasChanged();
            }
            else
            {
                await MessageService!.Error($"Error while deleting info: {result.NonSuccessMessage}", "Error");
            }
        }

        private async Task GetFinanciers(string? name, int pageNumber, int pageSize)
        {
            _getFinanciersbyNameParameters = new() { Name = name!, Page = pageNumber, PageSize = pageSize };

            OperationResult<PagingResponse<FinancierListItems>> result =
                await FinanciersService!.GetFinancierListItems(_getFinanciersbyNameParameters);

            if (result.Success)
            {
                _financierList = result.Result.Items;
                _metaData = result.Result.MetaData;
                StateHasChanged();
            }
            else
            {
                await MessageService!.Error($"Error while deleting info: {result.NonSuccessMessage}", "Error");
            }
        }

        private void OnActionItemClicked(string action, Guid financierId)
        {
            NavManager!.NavigateTo
            (
                action switch
                {
                    // "Details" => $"Finance/Pages/Financiers/FinancierDetailsPage/{financierId}",
                    "Edit" => $"Finance/Pages/Financiers/FinancierEditPage/{financierId}",
                    "Delete" => $"Finance/Pages/Financiers/FinancierEditPage/{financierId}",
                    _ => throw new ArgumentOutOfRangeException(nameof(action), $"Unexpected menu item: {action}"),
                }
            );
        }

        public async Task SearchChanged(string searchCriteria)
        {
            if (searchCriteria is not null)
            {
                await GetFinanciers(searchCriteria, 1, 5);
            }
        }

        private Task ShowDeleteModal(Guid financierId)
        {
            _financierToDelete = _financierList!.Find(fin => fin.FinancierId == financierId);

            _modalMessage = $"Delete financier {_financierToDelete!.FinancierName}?";
            _modalTitle = "Financier delete confirmation";

            return _deleteModalRef!.Show();
        }

        private async Task HideDeleteModal(string action)
        {
            if (action == "delete")
                await DeleteFinancier();

            await _deleteModalRef!.Hide();
        }

        private async Task DeleteFinancier()
        {
            FinancierWriteModel writeModel = new()
            {
                Id = _financierToDelete!.FinancierId,
                FinancierName = _financierToDelete!.FinancierName!
            };

            _isLoading = true;
            OperationResult<bool> result = await FinanciersService!.DeleteFinancier(writeModel);
            _isLoading = false;

            if (result.Success)
            {
                await GetFinanciers(1, 5);
                _snackarMessage = $"Information for financier {_financierToDelete!.FinancierName} was successfully deleted.";
                await InvokeAsync(StateHasChanged);
                await _snackbar!.Show();
            }
            else
            {
                await MessageService!.Error($"Error while deleting info: {result.NonSuccessMessage}", "Error");
            }
        }

        private async Task ShowDetailDialog(Guid financierId)
        {
            GetFinancier getFinancierParameters = new() { FinancierId = financierId };

            OperationResult<FinancierReadModel> result =
                await FinanciersService!.GetFinancierDetails(getFinancierParameters);

            if (result.Success)
            {
                _financierDetailModel = result.Result;
                await InvokeAsync(StateHasChanged);
            }
            else
            {
                await MessageService!.Error($"Error while deleting info: {result.NonSuccessMessage}", "Error");
            }
        }
    }
}