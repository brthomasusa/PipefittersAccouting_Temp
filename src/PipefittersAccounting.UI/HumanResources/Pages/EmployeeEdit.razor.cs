using Microsoft.AspNetCore.Components;
using PipefittersAccounting.SharedModel.ReadModels;
using PipefittersAccounting.SharedModel.Readmodels.HumanResources;
using PipefittersAccounting.UI.Interfaces;
using PipefittersAccounting.UI.Utilities;

namespace PipefittersAccounting.UI.HumanResources.Pages
{
    public partial class EmployeeEdit
    {
        [Parameter] public Guid EmployeeId { get; set; }
        [Parameter] public EmployeeDetail? EmployeeDetailModel { get; set; }
        [Inject] public IEmployeeHttpService? EmployeeService { get; set; }

        protected async override Task OnInitializedAsync()
        {
            GetEmployeeParameter queryParameters = new() { EmployeeID = EmployeeId };

            OperationResult<EmployeeDetail> result =
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