#pragma warning disable CS8602


using Microsoft.AspNetCore.Components;
using Blazorise;
using Blazorise.Snackbar;
using FluentValidation;
using PipefittersAccounting.UI.Utilities;

namespace PipefittersAccounting.UI.Finance.Components
{
    public partial class DataEntryForm<TWriteModel>  // 
    {
        private bool _isLoading;
        private Snackbar? _snackbar;
        private Validations? _validations;

        [CascadingParameter(Name = "WriteModelValidator")]
        private AbstractValidator<TWriteModel>? ModelValidator { get; set; }

        [Parameter] public string? ReturnUri { get; set; }
        [Parameter] public string? FormTitle { get; set; }
        [Parameter] public string? SnackBarMessage { get; set; }
        [Parameter] public RenderFragment? DataEntryFieldsTemplate { get; set; }
        [Parameter] public TWriteModel? WriteModel { get; set; }
        [Parameter] public Func<Task<OperationResult<bool>>>? SaveClickedEventHandler { get; set; }
        [Inject] public IMessageService? MessageService { get; set; }

        public async Task OnSave()
        {
            var result = await ModelValidator.ValidateAsync(WriteModel!, CancellationToken.None);

            if (!await _validations!.ValidateAll())
                return;

            _isLoading = true;
            OperationResult<bool> saveResult = await SaveClickedEventHandler.Invoke();
            _isLoading = false;

            if (saveResult.Success)
            {
                await _snackbar!.Show();
            }
            else
            {
                await MessageService!.Error($"Error while saving info: {saveResult.NonSuccessMessage}", "Error");
            }
        }
    }
}