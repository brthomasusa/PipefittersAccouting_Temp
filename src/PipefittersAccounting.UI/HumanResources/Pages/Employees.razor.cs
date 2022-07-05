using Microsoft.AspNetCore.Components;
using PipefittersAccounting.SharedModel.ReadModels;
using PipefittersAccounting.SharedModel.Readmodels.HumanResources;
using PipefittersAccounting.UI.Interfaces;
using PipefittersAccounting.UI.Utilities;

namespace PipefittersAccounting.UI.HumanResources.Pages
{
    public partial class Employees
    {
        public List<EmployeeListItem>? EmployeeList { get; set; } = new();

        [Inject] public IEmployeeHttpService? employeeService { get; set; }

        protected async override Task OnInitializedAsync()
        {
            GetEmployeesParameters parameters = new() { Page = 1, PageSize = 13 };
            OperationResult<PagingResponse<EmployeeListItem>> result = await employeeService!.GetEmployeeListItems(parameters);

            if (result.Success)
            {
                EmployeeList = result.Result.Items;
            }
            else
            {
                logger!.LogError(result.NonSuccessMessage);
            }
        }
    }
}