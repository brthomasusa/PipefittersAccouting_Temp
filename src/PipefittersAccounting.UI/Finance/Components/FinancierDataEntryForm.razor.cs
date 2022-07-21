#pragma warning disable CS8602

using Microsoft.AspNetCore.Components;
using Blazorise;
using Blazorise.Snackbar;
using PipefittersAccounting.SharedModel;
using PipefittersAccounting.SharedModel.Readmodels.Financing;
using PipefittersAccounting.SharedModel.WriteModels.Financing;
using PipefittersAccounting.UI.Components.Common;
using PipefittersAccounting.UI.Finance.Validators;
using PipefittersAccounting.UI.Interfaces;
using PipefittersAccounting.UI.Utilities;

namespace PipefittersAccounting.UI.Finance.Components
{
    public partial class FinancierDataEntryForm
    {
        private string? _snackBarMessage;
        private bool _isLoading;
        private Snackbar? _snackbar;
        private FinancierWriteModel? _financierDetailModel;
        private Validations? _validations;
        private FinancierWriteModelValidator _modelValidator = new();

        [Parameter] public string? ReturnUri { get; set; }
        [Parameter] public EditMode FormEditMode { get; set; }
        [Parameter] public Guid FinancierId { get; set; }
        [Inject] public IFinanciersHttpService? FinanciersService { get; set; }
        [Inject] public IMessageService? MessageService { get; set; }

        protected async override Task OnInitializedAsync()
        {
            if (FormEditMode == EditMode.Creating)
            {
                _financierDetailModel = new()
                {
                    Id = Guid.NewGuid(),
                    UserId = new Guid("660bb318-649e-470d-9d2b-693bfb0b2744")
                };
            }
            else if (FormEditMode == EditMode.Updating)
            {
                await GetFinancier();
            }
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

        private async Task Save()
        {
            var result = await _modelValidator.ValidateAsync(_financierDetailModel!, CancellationToken.None);

            if (!await _validations!.ValidateAll())
                return;

            _financierDetailModel!.StateCode = _financierDetailModel!.StateCode.ToUpper();

            if (FormEditMode == EditMode.Creating)
            {
                await Create();
            }
            else if (FormEditMode == EditMode.Updating)
            {
                await Update();
            }
        }

        private async Task Create()
        {
            _isLoading = true;
            OperationResult<FinancierReadModel> createResult = await FinanciersService!.CreateFinancier(_financierDetailModel!);
            _isLoading = false;

            if (createResult.Success)
            {
                _snackBarMessage = $"Information for {_financierDetailModel!.FinancierName} was successfully created.";
                await _snackbar!.Show();
            }
            else
            {
                await MessageService!.Error($"Error while creating: {createResult.NonSuccessMessage} info", "Error");
            }
        }

        private async Task Update()
        {
            _isLoading = true;
            OperationResult<bool> editResult = await FinanciersService!.EditFinancier(_financierDetailModel!);
            _isLoading = false;

            if (editResult.Success)
            {
                _snackBarMessage = $"Information for {_financierDetailModel!.FinancierName} was successfully updated.";
                await _snackbar!.Show();
            }
            else
            {
                await MessageService!.Error($"Error while creating: {editResult.NonSuccessMessage} info", "Error");
            }
        }
    }
}