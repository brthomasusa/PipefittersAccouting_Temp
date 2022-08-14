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
        public bool _showDeleteDialog;
        private Snackbar? _snackbar;
        private string? _snackBarMessage;
        private List<EmployeeManager>? _managers;
        private List<TimeCardWithPymtInfo>? _timeCardReadModels;
        private TimeCardWriteModel? _selectedTimeCardWriteModel;
        private TimeCardWithPymtInfo? _selectedTimeCardReadModel;

        [Inject] public IEmployeeRepository? EmployeeService { get; set; }
        [Inject] public IMessageService? MessageService { get; set; }

        protected async override Task OnInitializedAsync()
        {
            await GetTimeCardsForPayPeriod();
            await GetManagers();
            await base.OnInitializedAsync();
        }

        private async Task GetTimeCardsForPayPeriod()
        {
            GetTimeCardsForPayPeriodParameter queryParameters = new()
            {
                UserId = new Guid("660bb318-649e-470d-9d2b-693bfb0b2744")
            };

            OperationResult<List<TimeCardWithPymtInfo>> result = await EmployeeService!.GetTimeCardsForPayPeriod(queryParameters);

            if (!result.Success)
            {
                await MessageService!.Error($"Error while retrieving list of managers: {result.NonSuccessMessage}", "Error");
            }
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
            _showDeleteDialog = false;

            GetTimeCardsForManagerParameter queryParameters = new()
            {
                SupervisorId = managerId,
                PayPeriodEndDate = new DateTime(2022, 2, 28)
            };

            OperationResult<List<TimeCardWithPymtInfo>> result = await EmployeeService!.GetTimeCardsForManager(queryParameters);

            if (result.Success)
            {
                _timeCardReadModels = result.Result;
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

            _showEditDialog = false;
        }

        private async Task ShowDeleteModal(TimeCardWithPymtInfo readModel)
        {
            _selectedTimeCardReadModel = readModel;
            _showDeleteDialog = true;
            await InvokeAsync(StateHasChanged);
        }

        private async Task OnDeleteDialogClosed(string action)
        {
            if (action == "deleted")
            {
                _snackBarMessage = $"Timecard information was successfully deleted.";
                await GetWorkerTimeCardsForManager(_selectedTimeCardReadModel!.SupervisorId);
                await _snackbar!.Show();
            }

            _showDeleteDialog = false;
        }
    }
}
