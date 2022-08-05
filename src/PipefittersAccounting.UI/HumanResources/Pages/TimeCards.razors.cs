using Microsoft.AspNetCore.Components;
using Blazorise;
using Blazorise.Snackbar;
using PipefittersAccounting.SharedModel;
using PipefittersAccounting.SharedModel.Readmodels.HumanResources;
using PipefittersAccounting.SharedModel.WriteModels.HumanResources;
using PipefittersAccounting.UI.Interfaces;
using PipefittersAccounting.UI.Utilities;

namespace PipefittersAccounting.UI.HumanResources.Pages
{
    public partial class TimeCards
    {
        public bool _showEditDialog;
        private Snackbar? _snackbar;
        private string? _snackBarMessage;
        private List<EmployeeManager>? _managers;
        private List<TimeCardWithPymtInfo>? _timeCardReadModels;
        private TimeCardWriteModel? _selectedTimeCardWriteModel;
        private TimeCardWithPymtInfo? _selectedTimeCardReadModel;

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
                await GetWorkerTimeCardsForManager(_managers.FirstOrDefault()!.ManagerId);
                await InvokeAsync(StateHasChanged);
            }
            else
            {
                await MessageService!.Error($"Error while retrieving list of managers: {result.NonSuccessMessage}", "Error");
            }
        }

        private async Task GetWorkerTimeCardsForManager(Guid managerId)
        {
            _showEditDialog = false;

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

        private async Task OnSelectedRowChanged(EmployeeManager mgr)
        {
            await GetWorkerTimeCardsForManager(mgr!.ManagerId);
        }

        private async Task ShowEditModal(TimeCardWithPymtInfo readModel)
        {
            _selectedTimeCardWriteModel = readModel.Map();
            _showEditDialog = true;
            await InvokeAsync(StateHasChanged);
        }

        private async Task OnEditDialogClosed(string action)
        {
            if (action.Equals("saved"))
            {
                _snackBarMessage = $"Timecard information was successfully updated.";
                await GetWorkerTimeCardsForManager(_selectedTimeCardWriteModel!.SupervisorId);
                await _snackbar!.Show();
            }
        }

        private async Task ShowDeleteModal(TimeCardWithPymtInfo readModel)
        {
            _selectedTimeCardReadModel = readModel;
            await InvokeAsync(StateHasChanged);
        }

        private async Task HideDeleteModal(string action)
        {
            if (action == "save")
            {
                OperationResult<bool> result = await EmployeeService!.EditTimeCardInfo(_selectedTimeCardWriteModel!);

                if (result.Success)
                {
                    _snackBarMessage = $"Timecard information was successfully updated.";
                    await GetWorkerTimeCardsForManager(_selectedTimeCardWriteModel!.SupervisorId);
                    await _snackbar!.Show();
                    await InvokeAsync(StateHasChanged);
                }
                else
                {
                    await MessageService!.Error($"Error while deleting timecard info: {result.NonSuccessMessage}", "Error");
                }
            }
        }
    }
}
