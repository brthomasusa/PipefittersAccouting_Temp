#pragma warning disable CS8604

using Microsoft.AspNetCore.Components;
using Blazorise;
using PipefittersAccounting.SharedModel.Readmodels.HumanResources;
using PipefittersAccounting.UI.HumanResources.Components;
using PipefittersAccounting.UI.Interfaces;
using PipefittersAccounting.UI.Utilities;

namespace PipefittersAccounting.UI.HumanResources.Pages
{
    public partial class EmployeeCreate
    {
        private const string _returnUri = "HumanResouces/Pages/Employees";
        private const string _formTitle = "Create Employee Info";
        private string? _snackBarMessage;
        private EmployeeDataEntryState _state = new();

        [Inject] public IEmployeeRepository? EmployeeService { get; set; }
        [Inject] public IMessageService? MessageService { get; set; }

        protected async override Task OnInitializedAsync()
        {
            _state.EmployeeWriteModel = new()
            {
                EmployeeId = Guid.NewGuid(),
                StateCode = "",
                MaritalStatus = "",
                IsActive = true
            };

            await GetManagers();
            await GetEmployeeTypes();
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
            _state.EmployeeWriteModel!.MaritalStatus = _state.EmployeeWriteModel!.MaritalStatus.ToUpper();

            OperationResult<EmployeeDetail> result = await EmployeeService!.CreateEmployeeInfo(_state.EmployeeWriteModel!);

            if (result.Success)
            {
                _snackBarMessage = $"Information for {result.Result.EmployeeFullName} was successfully created.";
                await InvokeAsync(StateHasChanged);
                return OperationResult<bool>.CreateSuccessResult(true);
            }
            else
            {
                await MessageService!.Error($"Error while creating employee: {result.NonSuccessMessage}", "Error");
                return OperationResult<bool>.CreateFailure(result.NonSuccessMessage);
            }
        }
    }
}