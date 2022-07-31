using Microsoft.AspNetCore.Components;
using Blazorise;
using PipefittersAccounting.SharedModel;
using PipefittersAccounting.SharedModel.ReadModels;
using PipefittersAccounting.SharedModel.Readmodels.HumanResources;
using PipefittersAccounting.SharedModel.WriteModels.HumanResources;
using PipefittersAccounting.UI.HumanResources.Validators;
using PipefittersAccounting.UI.Interfaces;
using PipefittersAccounting.UI.Utilities;

namespace PipefittersAccounting.UI.HumanResources.Pages
{
    public partial class EmployeeEditPage
    {
        private const string _returnUri = "HumanResouces/Pages/Employees";
        private string? _snackBarMessage;
        private EmployeeWriteModelValidator _modelValidator = new();
        private EmployeeWriteModel? _employeeWriteModel;

        [Parameter] public Guid EmployeeId { get; set; }
        [Inject] public IEmployeeHttpService? EmployeeService { get; set; }
        [Inject] public IMessageService? MessageService { get; set; }

        protected async override Task OnInitializedAsync()
        {
            GetEmployeeParameter queryParameters = new() { EmployeeID = EmployeeId };

            OperationResult<EmployeeDetail> result =
                await EmployeeService!.GetEmployeeDetails(queryParameters);

            if (result.Success)
            {
                _employeeWriteModel = result.Result.Map();
                Console.WriteLine(_employeeWriteModel.ToJson());
                StateHasChanged();
            }
            else
            {
                await MessageService!.Error($"Error while retrieving employee: {result.NonSuccessMessage}", "Error");
            }
        }
    }
}