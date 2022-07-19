using Microsoft.AspNetCore.Components;
using Blazorise;
using PipefittersAccounting.SharedModel;
using PipefittersAccounting.SharedModel.Readmodels.Financing;
using PipefittersAccounting.UI.Validators.Financing;
using PipefittersAccounting.SharedModel.WriteModels.Financing;
using PipefittersAccounting.UI.Interfaces;
using PipefittersAccounting.UI.Utilities;

namespace PipefittersAccounting.UI.Finance.Pages.Financiers
{
    public partial class FinancierEdit
    {
        private FinancierWriteModel? _financierDetailModel;
        private Validations? _validations;
        private FinancierWriteModelValidator _modelValidator = new();

        [Parameter] public Guid FinancierId { get; set; }
        [Inject] public IFinanciersHttpService? FinanciersService { get; set; }

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

            Console.WriteLine("Validated: " + result.IsValid);

            if (!await _validations!.ValidateAll())
                return;

            //call a service ....
        }
    }
}
