using Microsoft.AspNetCore.Components;
using Blazorise;
using Blazorise.Snackbar;
using PipefittersAccounting.SharedModel.ReadModels;
using PipefittersAccounting.SharedModel.Readmodels.HumanResources;
using PipefittersAccounting.UI.Interfaces;
using PipefittersAccounting.UI.Utilities;

namespace PipefittersAccounting.UI.HumanResources.Pages
{
    public partial class Employees
    {
        private Guid _selectedEmployeeId;
        private string _placeHolderTextForSearch = "Search by employee's last name";
        private string? _snackarMessage;
        private Snackbar? _snackbar;
        private GetEmployeesParameters? _getEmployeesParameters;
        private GetEmployeesByLastNameParameters? _getEmployeesByLastNameParameters;
        private List<EmployeeListItem>? _employeeList;
        private MetaData? _metaData;
        private Func<int, int, Task>? _pagerChangedEventHandler;

        [Inject] public IEmployeeHttpService? EmployeeService { get; set; }
        [Inject] public IMessageService? MessageService { get; set; }
        [Inject] public NavigationManager? NavManager { get; set; }

        protected async override Task OnInitializedAsync()
        {
            _pagerChangedEventHandler = GetEmployees;
            await _pagerChangedEventHandler.Invoke(1, 5);
        }

        private async Task GetEmployees(int pageNumber, int pageSize)
        {
            _getEmployeesParameters = new() { Page = pageNumber, PageSize = pageSize };

            OperationResult<PagingResponse<EmployeeListItem>> result =
                await EmployeeService!.GetEmployeeListItems(_getEmployeesParameters);

            if (result.Success)
            {
                _employeeList = result.Result.Items;
                _metaData = result.Result.MetaData;
                StateHasChanged();
            }
            else
            {
                await MessageService!.Error($"Error while retrieving employees: {result.NonSuccessMessage}", "Error");
            }
        }

        private async Task GetEmployees(string? lastName, int pageNumber, int pageSize)
        {
            _getEmployeesByLastNameParameters = new() { LastName = lastName, Page = pageNumber, PageSize = pageSize };

            OperationResult<PagingResponse<EmployeeListItem>> result =
                await EmployeeService!.GetEmployeeListItems(_getEmployeesByLastNameParameters);

            if (result.Success)
            {
                _employeeList = result.Result.Items;
                _metaData = result.Result.MetaData;
                StateHasChanged();
            }
            else
            {
                await MessageService!.Error($"Error while retrieving employees: {result.NonSuccessMessage}", "Error");
            }
        }

        private void OnActionItemClicked(string action, Guid employeeId)
        {
            NavManager!.NavigateTo
            (
                action switch
                {
                    "Edit" => $"HumanResouces/Pages/EmployeeEditPage/{employeeId}",
                    _ => throw new ArgumentOutOfRangeException(nameof(action), $"Unexpected menu item: {action}"),
                }
            );
        }

        private async Task SearchChanged(string searchTerm) => await GetEmployees(searchTerm, 1, 5);

        private void ShowDetailDialog(Guid employeeId) => _selectedEmployeeId = employeeId;
    }
}