#pragma warning disable CS8602

using Microsoft.AspNetCore.Components;
using Blazorise;
using Blazorise.Snackbar;
using PipefittersAccounting.SharedModel.Readmodels.Financing;
using PipefittersAccounting.UI.Finance.Validators;
using PipefittersAccounting.SharedModel.WriteModels.Financing;
using PipefittersAccounting.UI.Interfaces;
using PipefittersAccounting.UI.Utilities;

namespace PipefittersAccounting.UI.Finance.Pages.Financiers
{
    public partial class FinancierCreatePage
    {
        private string? _snackBarMessage;
        private bool _isLoading;
        private Snackbar? _snackbar;
        private FinancierWriteModel? _financierDetailModel;
        private Validations? _validations;
        private FinancierWriteModelValidator _modelValidator = new();

        [Inject] public IFinanciersHttpService? FinanciersService { get; set; }
        [Inject] public IMessageService? MessageService { get; set; }

        protected override void OnInitialized()
        {
            _financierDetailModel = new()
            {
                Id = Guid.NewGuid(),
                UserId = new Guid("660bb318-649e-470d-9d2b-693bfb0b2744")
            };
        }

        protected async Task Save()
        {
            var result = await _modelValidator.ValidateAsync(_financierDetailModel!, CancellationToken.None);

            if (!await _validations!.ValidateAll())
                return;

            _financierDetailModel!.StateCode = _financierDetailModel!.StateCode.ToUpper();

            _isLoading = true;
            OperationResult<FinancierReadModel> createResult = await FinanciersService!.CreateFinancier(_financierDetailModel!);
            _isLoading = false;

            if (createResult.Success)
            {
                _snackBarMessage = $"Information for {_financierDetailModel!.FinancierName} was successfully created";
                await _snackbar!.Show();
            }
            else
            {
                await MessageService!.Error($"Error while creating: {createResult.NonSuccessMessage} info", "Error");
            }
        }
    }
}