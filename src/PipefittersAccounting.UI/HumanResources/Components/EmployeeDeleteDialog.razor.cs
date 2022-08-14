using Microsoft.AspNetCore.Components;
using Blazorise;
using PipefittersAccounting.SharedModel;
using PipefittersAccounting.SharedModel.Readmodels.HumanResources;
using PipefittersAccounting.UI.Interfaces;
using PipefittersAccounting.UI.Utilities;

namespace PipefittersAccounting.UI.HumanResources.Components
{
    public partial class EmployeeDeleteDialog
    {
        private bool _isLoading;
        private Modal? _modalRef;
        [Parameter] public bool ShowDialog { get; set; }
        [Parameter] public EmployeeDetail? EmployeeReadModel { get; set; }
        [Parameter] public EventCallback<string> OnDialogCloseEventHandler { get; set; }
        [Inject] public IEmployeeRepository? EmployeeService { get; set; }
        [Inject] public IMessageService? MessageService { get; set; }

        protected async override Task OnParametersSetAsync()
        {
            if (EmployeeReadModel is not null && ShowDialog)
            {
                if (_modalRef is not null)
                {
                    await _modalRef!.Show();
                }
            }
        }

        private async Task OnDelete()
        {
            _isLoading = true;
            OperationResult<bool> result = await EmployeeService!.DeleteEmployeeInfo(EmployeeReadModel!.Map());

            if (result.Success)
            {
                await _modalRef!.Hide();
                await OnDialogCloseEventHandler.InvokeAsync("deleted");
            }
            else
            {
                await MessageService!.Error($"Error while deleting employee info: {result.NonSuccessMessage}", "Error");
            }

            _isLoading = false;
        }

        private async Task CloseDialog()
        {
            await _modalRef!.Hide();
            await OnDialogCloseEventHandler.InvokeAsync("canceled");
        }

        private string ConvertIsActiveToString()
            => EmployeeReadModel!.IsActive ? "Active" : "Inactive";

        private string ConvertIsSupervisorToString()
            => EmployeeReadModel!.IsSupervisor ? "Yes" : "No";

        private string HideEmptyDate()
            => EmployeeReadModel!.LastModifiedDate != default ? EmployeeReadModel!.LastModifiedDate.ToShortDateString() : "";

        private string ConvertCurrencyToString()
            => string.Format("{0:C}", EmployeeReadModel!.PayRate);
    }
}