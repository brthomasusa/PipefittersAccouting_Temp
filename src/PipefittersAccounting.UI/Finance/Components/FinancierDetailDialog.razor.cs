using Microsoft.AspNetCore.Components;
using Blazorise;
using PipefittersAccounting.SharedModel.Readmodels.Financing;
using PipefittersAccounting.UI.Interfaces;
using PipefittersAccounting.UI.Utilities;

namespace PipefittersAccounting.UI.Finance.Components
{
    public partial class FinancierDetailDialog
    {
        private Modal? _detailModalRef;
        private FinancierReadModel? _financier;

        [Parameter] public Guid FinancierId { get; set; }
        [Inject] public IFinanciersHttpService? FinanciersService { get; set; }
        [Inject] public IMessageService? MessageService { get; set; }

        protected async override Task OnParametersSetAsync()
        {
            if (FinancierId != default)
            {
                await GetFinancier();
                await InvokeAsync(StateHasChanged);

                if (_detailModalRef is not null)
                {
                    await _detailModalRef!.Show();
                }
            }
        }

        private async Task GetFinancier()
        {
            GetFinancier getFinancierParameters = new() { FinancierId = this.FinancierId };

            OperationResult<FinancierReadModel> result =
                await FinanciersService!.GetFinancierDetails(getFinancierParameters);

            if (result.Success)
            {
                _financier = result.Result;
            }
            else
            {
                await MessageService!.Error($"Error while retrieving financier details: {result.NonSuccessMessage}", "Error");
            }
        }

        private async Task CloseDialog() => await _detailModalRef!.Hide();

        private string ConvertIsActiveToString()
            => _financier!.IsActive ? "Active" : "Inactive";
    }
}