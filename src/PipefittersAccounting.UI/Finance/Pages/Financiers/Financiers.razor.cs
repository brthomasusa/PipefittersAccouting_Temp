using Microsoft.AspNetCore.Components;
using PipefittersAccounting.SharedModel.ReadModels;
using PipefittersAccounting.SharedModel.Readmodels.Financing;
using PipefittersAccounting.UI.Interfaces;
using PipefittersAccounting.UI.Utilities;

namespace PipefittersAccounting.UI.Finance.Pages.Financiers
{
    public partial class Financiers
    {
        private string _placeHolderTextForSearch = "Search by financier's name";
        private GetFinanciers? _getFinanciersParameters;
        private GetFinanciersByName? _getFinanciersbyNameParameters;
        private List<FinancierListItems>? _financierList;
        private MetaData? _metaData;
        private Func<int, int, Task>? _pagerChangedEventHandler;

        [Inject] public IFinanciersHttpService? FinanciersService { get; set; }
        [Inject] public NavigationManager? NavManager { get; set; }

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
                logger!.LogError(result.NonSuccessMessage);
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
                logger!.LogError(result.NonSuccessMessage);
            }
        }

        private void OnActionItemClicked(string action, Guid financierId)
        {
            NavManager!.NavigateTo
            (
                action switch
                {
                    "Details" => $"Finance/Pages/Financiers/FinancierDetails/{financierId}",
                    "Edit" => $"Finance/Pages/Financiers/FinancierEdit/{financierId}",
                    "Delete" => $"Finance/Pages/Financiers/FinancierEdit/{financierId}",
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
    }
}