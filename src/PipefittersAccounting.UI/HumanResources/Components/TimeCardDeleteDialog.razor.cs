using Microsoft.AspNetCore.Components;
using Blazorise;
using PipefittersAccounting.SharedModel;
using PipefittersAccounting.SharedModel.Readmodels.HumanResources;
using PipefittersAccounting.UI.Interfaces;
using PipefittersAccounting.UI.Utilities;

namespace PipefittersAccounting.UI.HumanResources.Components
{
    public partial class TimeCardDeleteDialog
    {
        private bool _isLoading;
        private Modal? _modalRef;

        [Parameter] public bool ShowDialog { get; set; }
        [Parameter] public TimeCardWithPymtInfo? TimeCardReadModel { get; set; }
        [Parameter] public EventCallback<string> OnDialogCloseEventHandler { get; set; }
        [Inject] public IEmployeeHttpService? EmployeeService { get; set; }
        [Inject] public IMessageService? MessageService { get; set; }

        protected async override Task OnParametersSetAsync()
        {
            if (TimeCardReadModel is not null && ShowDialog)
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
            OperationResult<bool> result = await EmployeeService!.DeleteTimeCardInfo(TimeCardReadModel!.Map());

            if (result.Success)
            {
                await _modalRef!.Hide();
                await OnDialogCloseEventHandler.InvokeAsync("deleted");
            }
            else
            {
                await MessageService!.Error($"Error while deleting timecard info: {result.NonSuccessMessage}", "Error");
            }

            _isLoading = false;
        }

        private async Task CloseDialog()
        {
            await _modalRef!.Hide();
            await OnDialogCloseEventHandler.InvokeAsync("canceled");
        }
    }
}