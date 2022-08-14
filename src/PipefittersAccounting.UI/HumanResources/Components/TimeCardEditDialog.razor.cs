using Microsoft.AspNetCore.Components;
using Blazorise;
using PipefittersAccounting.SharedModel.WriteModels.HumanResources;
using PipefittersAccounting.UI.Interfaces;
using PipefittersAccounting.UI.Utilities;

namespace PipefittersAccounting.UI.HumanResources.Components
{
    public partial class TimeCardEditDialog
    {
        private bool _isLoading;
        private Modal? _modalRef;
        private Validations? _validations;

        [Parameter] public bool ShowDialog { get; set; }
        [Parameter] public TimeCardWriteModel? TimeCardWriteModel { get; set; }
        [Parameter] public EventCallback<string> OnDialogCloseEventHandler { get; set; }
        [Inject] public IEmployeeRepository? EmployeeService { get; set; }
        [Inject] public IMessageService? MessageService { get; set; }

        protected async override Task OnParametersSetAsync()
        {
            if (TimeCardWriteModel is not null && ShowDialog)
            {
                if (_modalRef is not null)
                {
                    await _modalRef!.Show();
                }
            }
        }

        private async Task OnSave()
        {
            if (!await _validations!.ValidateAll())
                return;

            _isLoading = true;
            OperationResult<bool> result = await EmployeeService!.EditTimeCardInfo(TimeCardWriteModel!);

            if (result.Success)
            {
                await _modalRef!.Hide();
                await OnDialogCloseEventHandler.InvokeAsync("saved");
            }
            else
            {
                await MessageService!.Error($"Error while updating timecard info: {result.NonSuccessMessage}", "Error");
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