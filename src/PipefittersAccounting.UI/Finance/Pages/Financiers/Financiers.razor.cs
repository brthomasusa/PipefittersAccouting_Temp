using Microsoft.AspNetCore.Components;
using PipefittersAccounting.SharedModel.ReadModels;
using PipefittersAccounting.SharedModel.Readmodels.Financing;
using PipefittersAccounting.UI.Interfaces;
using PipefittersAccounting.UI.Utilities;

namespace PipefittersAccounting.UI.Finance.Pages.Financiers
{
    public partial class Financiers
    {
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

        private void OnMenuItemClicked(string itemName, Guid employeeId)
        {
            NavManager!.NavigateTo
            (
            itemName switch
            {
                "ShowDetails" => $"HumanResouces/Pages/EmployeeDetails/{employeeId}",
                "EditEmployee" => $"HumanResouces/Pages/EmployeeEdit/{employeeId}",
                "DeleteEmployee" => $"HumanResouces/Pages/EmployeeDetails/{employeeId}",
                _ => throw new ArgumentOutOfRangeException(nameof(itemName), $"Unexpected menu item: {itemName}"),
            }
            );
        }
    }
}