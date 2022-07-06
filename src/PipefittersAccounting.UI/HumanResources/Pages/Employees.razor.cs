using Microsoft.AspNetCore.Components;
using PipefittersAccounting.SharedModel.ReadModels;
using PipefittersAccounting.SharedModel.Readmodels.HumanResources;
using PipefittersAccounting.UI.Interfaces;
using PipefittersAccounting.UI.Utilities;

namespace PipefittersAccounting.UI.HumanResources.Pages
{
    public partial class Employees
    {
        private GetEmployeesParameters? GetEmployeesParameters { get; set; }

        public List<EmployeeListItem>? EmployeeList { get; set; } = new();
        public MetaData? MetaData { get; set; }
        [Inject] public IEmployeeHttpService? employeeService { get; set; }

        protected async override Task OnInitializedAsync()
        {
            await GetEmployees(1, 5);
        }

        public async Task CurrentPageChangedHandler(int currentPage)
        {
            logger!.LogInformation($"Employees page has received new current page: {currentPage}");
            await GetEmployees(currentPage, 5);
        }

        private async Task GetEmployees(int page, int pageSize)
        {
            GetEmployeesParameters = new() { Page = page, PageSize = pageSize };

            OperationResult<PagingResponse<EmployeeListItem>> result =
                await employeeService!.GetEmployeeListItems(GetEmployeesParameters);

            if (result.Success)
            {
                EmployeeList = result.Result.Items;
                MetaData = result.Result.MetaData;
                StateHasChanged();
            }
            else
            {
                logger!.LogError(result.NonSuccessMessage);
            }
        }
    }
}