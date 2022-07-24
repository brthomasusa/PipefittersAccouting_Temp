#pragma warning disable CS8602

using Microsoft.AspNetCore.Components;
using PipefittersAccounting.SharedModel.Readmodels.Financing;
using PipefittersAccounting.SharedModel.WriteModels.Financing;
using PipefittersAccounting.UI.Finance.Validators;
using PipefittersAccounting.UI.Interfaces;
using PipefittersAccounting.UI.Utilities;

namespace PipefittersAccounting.UI.Finance.Pages.Financiers
{
    public partial class FinancierCreatePage
    {
        private string _returnUri = "Finance/Pages/Financiers/FinanciersListPage";
        private string? _snackBarMessage;
        private FinancierWriteModel? _financierDetailModel;
        private FinancierWriteModelValidator _modelValidator = new();

        [Inject] public IFinanciersHttpService? FinanciersService { get; set; }

        protected override void OnInitialized()
        {
            _financierDetailModel = new()
            {
                Id = Guid.NewGuid(),
                UserId = new Guid("660bb318-649e-470d-9d2b-693bfb0b2744")
            };
        }

        private async Task<OperationResult<bool>> Create()
        {
            _financierDetailModel!.StateCode = _financierDetailModel!.StateCode.ToUpper();
            OperationResult<FinancierReadModel> createResult = await FinanciersService!.CreateFinancier(_financierDetailModel!);

            if (createResult.Success)
            {
                _snackBarMessage = $"Information for {_financierDetailModel!.FinancierName} was successfully created.";
                await InvokeAsync(StateHasChanged);
                return OperationResult<bool>.CreateSuccessResult(true);
            }
            else
            {
                return OperationResult<bool>.CreateFailure(createResult.NonSuccessMessage);
            }
        }
    }
}