using Microsoft.AspNetCore.Components;
using PipefittersAccounting.SharedModel.Readmodels.HumanResources;
using PipefittersAccounting.UI.Interfaces;
using PipefittersAccounting.UI.Utilities;

namespace PipefittersAccounting.UI.HumanResources.Pages
{
    public partial class EmployeeDetails
    {
        [Parameter] public Guid EmployeeId { get; set; }
        [Parameter] public EmployeeReadModel? EmployeeDetailModel { get; set; }
        [Inject] public IEmployeeRepository? EmployeeService { get; set; }

        protected async override Task OnInitializedAsync()
        {
            GetEmployeeParameter queryParameters = new() { EmployeeID = EmployeeId };

            OperationResult<EmployeeReadModel> result =
                await EmployeeService!.GetEmployeeDetails(queryParameters);

            if (result.Success)
            {
                EmployeeDetailModel = result.Result;
                StateHasChanged();
            }
            else
            {
                logger!.LogError(result.NonSuccessMessage);
            }
        }
    }
}