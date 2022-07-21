#pragma warning disable CS8602
using Microsoft.AspNetCore.Components;
using Blazorise;
using Blazorise.Snackbar;
using PipefittersAccounting.SharedModel;
using PipefittersAccounting.SharedModel.Readmodels.Financing;
using PipefittersAccounting.UI.Finance.Validators;
using PipefittersAccounting.SharedModel.WriteModels.Financing;
using PipefittersAccounting.UI.Interfaces;
using PipefittersAccounting.UI.Utilities;

namespace PipefittersAccounting.UI.Finance.Pages.Financiers
{
    public partial class FinancierEditPage
    {
        private string? _snackBarMessage;
        private bool _isLoading;
        private Snackbar? _snackbar;
        private FinancierWriteModel? _financierDetailModel;
        private Validations? _validations;
        private FinancierWriteModelValidator _modelValidator = new();

        [Parameter] public Guid FinancierId { get; set; }
        [Inject] public IFinanciersHttpService? FinanciersService { get; set; }
        [Inject] public IMessageService? MessageService { get; set; }

        protected async override Task OnInitializedAsync()
        {
            await GetFinancier();
        }

        private async Task GetFinancier()
        {
            GetFinancier getFinancierParameters = new() { FinancierId = FinancierId };

            OperationResult<FinancierReadModel> result =
                await FinanciersService!.GetFinancierDetails(getFinancierParameters);

            if (result.Success)
            {
                _financierDetailModel = result.Result.Map();
                _financierDetailModel.UserId = new Guid("660bb318-649e-470d-9d2b-693bfb0b2744");
                StateHasChanged();
            }
            else
            {
                logger!.LogError(result.NonSuccessMessage);
            }
        }

        protected async Task Save()
        {
            var result = await _modelValidator.ValidateAsync(_financierDetailModel!, CancellationToken.None);

            if (!await _validations!.ValidateAll())
                return;

            _financierDetailModel!.StateCode = _financierDetailModel!.StateCode.ToUpper();

            _isLoading = true;
            OperationResult<bool> editResult = await FinanciersService!.EditFinancier(_financierDetailModel!);
            _isLoading = false;

            if (editResult.Success)
            {
                _snackBarMessage = $"Information for {_financierDetailModel!.FinancierName} was successfully updated";
                await _snackbar!.Show();
            }
            else
            {
                await MessageService!.Error($"Error while updating: {editResult.NonSuccessMessage} info", "Error");
            }
        }
    }
}
