#pragma warning disable CS8602

using Microsoft.AspNetCore.Components;
using Blazorise;
using PipefittersAccounting.SharedModel;
using PipefittersAccounting.SharedModel.Readmodels.Financing;
using PipefittersAccounting.SharedModel.WriteModels.Financing;
using PipefittersAccounting.UI.Finance.Validators;
using PipefittersAccounting.UI.Interfaces;
using PipefittersAccounting.UI.Utilities;

namespace PipefittersAccounting.UI.Finance.Pages.Financiers
{
    public partial class FinancierEditPage
    {
        private string? _returnUri;
        private string? _snackBarMessage;
        private FinancierWriteModel? _financierDetailModel;
        private FinancierWriteModelValidator _modelValidator = new();

        [Parameter] public Guid FinancierId { get; set; }
        [Inject] public IFinanciersHttpService? FinanciersService { get; set; }
        [Inject] public IMessageService? MessageService { get; set; }

        protected async override Task OnInitializedAsync()
        {
            _returnUri = "Finance/Pages/Financiers/FinanciersListPage";
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
                await MessageService!.Error($"Error while retrieving financier: {result.NonSuccessMessage}", "Error");
            }
        }

        private async Task<OperationResult<bool>> Update()
        {
            _financierDetailModel!.StateCode = _financierDetailModel!.StateCode.ToUpper();
            OperationResult<bool> updateResult = await FinanciersService!.EditFinancier(_financierDetailModel!);

            if (updateResult.Success)
            {
                _snackBarMessage = $"Information for {_financierDetailModel!.FinancierName} was successfully updated.";
                await InvokeAsync(StateHasChanged);
                return OperationResult<bool>.CreateSuccessResult(true);
            }
            else
            {
                return OperationResult<bool>.CreateFailure(updateResult.NonSuccessMessage);
            }
        }
    }
}

