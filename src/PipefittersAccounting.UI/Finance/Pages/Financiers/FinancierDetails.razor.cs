using Microsoft.AspNetCore.Components;
using PipefittersAccounting.SharedModel.Readmodels.Financing;
using PipefittersAccounting.UI.Interfaces;
using PipefittersAccounting.UI.Utilities;

namespace PipefittersAccounting.UI.Finance.Pages.Financiers
{
    public partial class FinancierDetails
    {
        private FinancierDetail? _financierDetailModel;
        private string _pageTitle = "Financier Details";
        private string? _formTitle;

        [Parameter] public Guid FinancierId { get; set; }
        [Inject] public IFinanciersHttpService? FinanciersService { get; set; }

        protected async override Task OnInitializedAsync()
        {
            await GetFinancier();
            _formTitle = _financierDetailModel!.FinancierName;
        }

        private async Task GetFinancier()
        {
            GetFinancier getFinancierParameters = new() { FinancierId = FinancierId };

            OperationResult<FinancierDetail> result =
                await FinanciersService!.GetFinancierDetails(getFinancierParameters);

            if (result.Success)
            {
                _financierDetailModel = result.Result;
                StateHasChanged();
            }
            else
            {
                logger!.LogError(result.NonSuccessMessage);
            }
        }

        private string ConvertIsActiveToString()
            => _financierDetailModel!.IsActive ? "Active" : "Inactive";
    }
}