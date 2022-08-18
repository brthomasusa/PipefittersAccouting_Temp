using Microsoft.AspNetCore.Components;
using Blazorise;
using PipefittersAccounting.SharedModel.WriteModels.Financing;
using PipefittersAccounting.UI.Interfaces;
using PipefittersAccounting.UI.Utilities;

namespace PipefittersAccounting.UI.Finance.Components
{
    public partial class LoanInstallmentDataEntryDialog
    {
        private Validations? _validations;
        private bool _isLoading;
        private Modal? _modalRef;
        private LoanInstallmentWriteModel? _currentInstallment;
        private List<LoanInstallmentWriteModel> _installments = new();

        [Parameter] public bool ShowDialog { get; set; }
        [Parameter] public LoanAgreementWriteModel? LoanAgreement { get; set; }
        [Parameter] public EventCallback<string> OnDialogClosedHandler { get; set; }
        [Inject] public IMessageService? MessageService { get; set; }

        protected async override Task OnParametersSetAsync()
        {
            if (ShowDialog)
            {
                if (_modalRef is not null && LoanAgreement is not null)
                {
                    await _modalRef!.Show();
                    await InvokeAsync(StateHasChanged);
                }
            }
        }

        private async Task OnSave()
        {
            _isLoading = true;
            await Task.CompletedTask;
            _isLoading = false;

            // OperationResult<bool> result = await EmployeeService!.EditTimeCardInfo(TimeCardWriteModel!);

            // if (result.Success)
            // {
            //     await _modalRef!.Hide();
            //     await OnDialogCloseEventHandler.InvokeAsync("saved");
            // }
            // else
            // {
            //     await MessageService!.Error($"Error while updating timecard info: {result.NonSuccessMessage}", "Error");
            // }
        }

        private async Task CloseDialog()
        {
            await _modalRef!.Hide();
            await OnDialogClosedHandler.InvokeAsync("canceled");
            // await OnDialogCloseEventHandler.InvokeAsync("canceled");
        }
    }
}