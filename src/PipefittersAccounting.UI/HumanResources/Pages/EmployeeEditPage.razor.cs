using Microsoft.AspNetCore.Components;
using Blazorise;
using PipefittersAccounting.SharedModel;
using PipefittersAccounting.SharedModel.Readmodels.HumanResources;
using PipefittersAccounting.UI.HumanResources.Components;
using PipefittersAccounting.UI.Interfaces;
using PipefittersAccounting.UI.Utilities;

namespace PipefittersAccounting.UI.HumanResources.Pages
{
    public partial class EmployeeEditPage
    {
        private const string _returnUri = "HumanResouces/Pages/Employees";
        private const string _formTitle = "Edit Employee Info";
        private string? _snackBarMessage;
        private EmployeeDataEntryState _state = new();

        [Parameter] public Guid EmployeeId { get; set; }
        [Inject] public IEmployeeHttpService? EmployeeService { get; set; }
        [Inject] public IMessageService? MessageService { get; set; }

        protected async override Task OnInitializedAsync()
        {
            await GetEmployee();
        }

        private async Task GetEmployee()
        {
            GetEmployeeParameter queryParameters = new() { EmployeeID = EmployeeId };

            OperationResult<EmployeeDetail> result =
                await EmployeeService!.GetEmployeeDetails(queryParameters);

            if (result.Success)
            {
                await GetManagers();
                await GetEmployeeTypes();

                _state.EmployeeWriteModel = result.Result.Map();

                await InvokeAsync(StateHasChanged);
            }
            else
            {
                await MessageService!.Error($"Error while retrieving employee: {result.NonSuccessMessage}", "Error");
            }
        }

        private async Task GetManagers()
        {
            OperationResult<List<EmployeeManager>> result = await EmployeeService!.GetEmployeeManagers();

            if (result.Success)
            {
                _state.Managers = result.Result;
            }
            else
            {
                await MessageService!.Error($"Error while retrieving list of managers: {result.NonSuccessMessage}", "Error");
            }
        }

        private async Task GetEmployeeTypes()
        {
            OperationResult<List<EmployeeTypes>> result = await EmployeeService!.GetEmployeeTypes();

            if (result.Success)
            {
                _state.EmployeeTypes = result.Result;
            }
            else
            {
                await MessageService!.Error($"Error while retrieving employee types: {result.NonSuccessMessage}", "Error");
            }
        }

        private async Task<OperationResult<bool>> Save()
        {
            _state.EmployeeWriteModel!.StateCode = _state.EmployeeWriteModel!.StateCode.ToUpper();
            OperationResult<bool> updateResult = await EmployeeService!.EditEmployeeInfo(_state.EmployeeWriteModel!);

            if (updateResult.Success)
            {
                string fullName = $"{_state.EmployeeWriteModel!.FirstName} {_state.EmployeeWriteModel!.LastName} ";
                _snackBarMessage = $"Information for {fullName} was successfully updated.";
                await InvokeAsync(StateHasChanged);
                return OperationResult<bool>.CreateSuccessResult(true);
            }
            else
            {
                await MessageService!.Error($"Error while retrieving employee: {updateResult.NonSuccessMessage}", "Error");
                return OperationResult<bool>.CreateFailure(updateResult.NonSuccessMessage);
            }
        }

        void ValidateMaritalStatus(ValidatorEventArgs e)
        {
            var maritalStatus = Convert.ToString(e.Value);

            bool isValid = (maritalStatus!.ToUpper() == "M" || maritalStatus!.ToUpper() == "S");

            e.Status = string.IsNullOrEmpty(maritalStatus) ? ValidationStatus.None :
                       isValid ? ValidationStatus.Success : ValidationStatus.Error;
        }
    }
}