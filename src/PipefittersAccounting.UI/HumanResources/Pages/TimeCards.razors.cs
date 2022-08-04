using Microsoft.AspNetCore.Components;
using Blazorise;
using PipefittersAccounting.SharedModel;
using PipefittersAccounting.SharedModel.Readmodels.HumanResources;
using PipefittersAccounting.SharedModel.WriteModels.HumanResources;
using PipefittersAccounting.UI.Interfaces;
using PipefittersAccounting.UI.Utilities;

namespace PipefittersAccounting.UI.HumanResources.Pages
{
    public partial class TimeCards
    {
        private bool _isLoading;
        private Modal? _modalRef;
        private Validations? _validations;
        private List<EmployeeManager>? _managers;
        private List<TimeCardWithPymtInfo>? _timeCardReadModels;
        private TimeCardWriteModel? _selectedTimeCard;

        [Inject] public IEmployeeHttpService? EmployeeService { get; set; }
        [Inject] public IMessageService? MessageService { get; set; }

        protected async override Task OnInitializedAsync()
        {
            await GetManagers();
            await base.OnInitializedAsync();
        }

        private async Task GetManagers()
        {
            OperationResult<List<EmployeeManager>> result = await EmployeeService!.GetEmployeeManagers();

            if (result.Success)
            {
                _managers = result.Result;
                await GetWorkerForManager(_managers.FirstOrDefault()!.ManagerId);
                await InvokeAsync(StateHasChanged);
            }
            else
            {
                await MessageService!.Error($"Error while retrieving list of managers: {result.NonSuccessMessage}", "Error");
            }
        }

        private async Task GetWorkerForManager(Guid managerId)
        {
            GetTimeCardsForManagerParameter queryParameters = new()
            {
                SupervisorId = managerId,
                PayPeriodEndDate = new DateTime(2022, 2, 28)
            };

            OperationResult<List<TimeCardWithPymtInfo>> result = await EmployeeService!.GetTimeCardsForManager(queryParameters);

            if (result.Success)
            {
                _timeCardReadModels = result.Result;
                await InvokeAsync(StateHasChanged);
            }
            else
            {
                await MessageService!.Error($"Error while retrieving list of timecards: {result.NonSuccessMessage}", "Error");
            }
        }

        private void CheckRegularHours(ValidatorEventArgs validationArgs)
        {
            bool isValid = ((int)validationArgs.Value >= 0 && (int)validationArgs.Value <= 168);

            validationArgs.Status = isValid ? ValidationStatus.Success : ValidationStatus.Error;

            if (validationArgs.Status == ValidationStatus.Error)
            {
                validationArgs.ErrorText = "Regular hours out of range.";
            }
        }
        private void CheckOvertimeHours(ValidatorEventArgs validationArgs)
        {
            bool isValid = ((int)validationArgs.Value >= 0 && (int)validationArgs.Value <= 168);

            validationArgs.Status = isValid ? ValidationStatus.Success : ValidationStatus.Error;

            if (validationArgs.Status == ValidationStatus.Error)
            {
                validationArgs.ErrorText = "Overtime hours out of range.";
            }
        }

        private async Task OnSelectedRowChanged(EmployeeManager mgr)
        {
            await GetWorkerForManager(mgr!.ManagerId);
        }

        private void OpenEditTimecardModal()
        {

            Console.WriteLine($"opening new applicant modal...");
        }

        private async Task ShowEditModal(TimeCardWithPymtInfo readModel)
        {
            _selectedTimeCard = readModel.Map();

            await InvokeAsync(StateHasChanged);

            await _modalRef!.Show();
        }

        private async Task HideEditModal(string action)
        {
            if (action == "save")
            {
                if (!await _validations!.ValidateAll())
                    return;
            }

            await _modalRef!.Hide();
        }
    }
}
