using Microsoft.AspNetCore.Components;
using Blazorise;
using PipefittersAccounting.SharedModel.Readmodels.HumanResources;
using PipefittersAccounting.UI.Interfaces;
using PipefittersAccounting.UI.Utilities;

namespace PipefittersAccounting.UI.HumanResources.Components
{
    public partial class EmployeeDetailDialog
    {
        private Modal? _detailModalRef;
        private EmployeeDetail? _employee;
        private string selectedTab = "generalInfo";

        [Parameter] public Guid EmployeeId { get; set; }
        [Inject] public IEmployeeHttpService? EmployeeService { get; set; }
        [Inject] public IMessageService? MessageService { get; set; }

        protected async override Task OnParametersSetAsync()
        {
            if (EmployeeId != default)
            {
                await GetEmployee();
                await InvokeAsync(StateHasChanged);

                if (_detailModalRef is not null)
                {
                    await _detailModalRef!.Show();
                }
            }
        }

        private async Task GetEmployee()
        {
            GetEmployeeParameter getEmployeeParameters = new() { EmployeeID = this.EmployeeId };

            OperationResult<EmployeeDetail> result =
                await EmployeeService!.GetEmployeeDetails(getEmployeeParameters);

            if (result.Success)
            {
                _employee = result.Result;
            }
            else
            {
                await MessageService!.Error($"Error while retrieving employee details: {result.NonSuccessMessage}", "Error");
            }
        }

        private Task OnSelectedTabChanged(string name)
        {
            selectedTab = name;

            return Task.CompletedTask;
        }

        private async Task CloseDialog() => await _detailModalRef!.Hide();

        private string ConvertIsActiveToString()
            => _employee!.IsActive ? "Active" : "Inactive";

        private string ConvertIsSupervisorToString()
            => _employee!.IsSupervisor ? "Yes" : "No";

        private string HideEmptyDate()
            => _employee!.LastModifiedDate != default ? _employee!.LastModifiedDate.ToShortDateString() : "";

        private string ConvertCurrencyToString()
            => string.Format("{0:C}", _employee!.PayRate);
    }
}