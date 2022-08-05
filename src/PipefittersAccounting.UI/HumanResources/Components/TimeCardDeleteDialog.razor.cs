using Microsoft.AspNetCore.Components;
using Blazorise;
using PipefittersAccounting.SharedModel.Readmodels.HumanResources;

namespace PipefittersAccounting.UI.HumanResources.Components
{
    public partial class TimeCardDeleteDialog
    {
        private bool _isLoading;
        private Modal? _modalRef;

        [Parameter] public TimeCardWithPymtInfo? TimeCardReadModel { get; set; }
        [Parameter] public EventCallback<string> HandleDialogClose { get; set; }

        protected async override Task OnParametersSetAsync()
        {
            if (TimeCardReadModel is not null)
            {
                if (_modalRef is not null)
                {
                    await _modalRef!.Show();
                }
            }
        }

        private async Task CloseDialog(string action)
        {
            if (action.Equals("save"))
            {
                _isLoading = true;
                await HandleDialogClose.InvokeAsync(action);
                _isLoading = false;
                await _modalRef!.Hide();
            }
            else
            {
                await _modalRef!.Hide();
            }
        }
    }
}