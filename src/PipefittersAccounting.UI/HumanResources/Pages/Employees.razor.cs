using Microsoft.AspNetCore.Components;
using PipefittersAccounting.SharedModel.ReadModels;
using PipefittersAccounting.SharedModel.Readmodels.HumanResources;
using PipefittersAccounting.UI.Interfaces;
using PipefittersAccounting.UI.Utilities;

namespace PipefittersAccounting.UI.HumanResources.Pages
{
    public partial class Employees
    {
        private GetEmployeesParameters? _getEmployeesParameters;
        private List<EmployeeListItem>? _employeeList;
        private MetaData? _metaData;
        private Func<int, int, Task>? _pagerChangedEventHandler;

        [Inject] public IEmployeeHttpService? employeeService { get; set; }

        protected async override Task OnInitializedAsync()
        {
            _pagerChangedEventHandler = GetEmployees;
            await _pagerChangedEventHandler.Invoke(1, 5);
        }

        private async Task GetEmployees(int pageNumber, int pageSize)
        {
            _getEmployeesParameters = new() { Page = pageNumber, PageSize = pageSize };

            OperationResult<PagingResponse<EmployeeListItem>> result =
                await employeeService!.GetEmployeeListItems(_getEmployeesParameters);

            if (result.Success)
            {
                _employeeList = result.Result.Items;
                _metaData = result.Result.MetaData;
                StateHasChanged();
            }
            else
            {
                logger!.LogError(result.NonSuccessMessage);
            }
        }
    }
}