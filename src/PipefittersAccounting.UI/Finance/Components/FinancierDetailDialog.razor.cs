using Microsoft.AspNetCore.Components;
using Blazorise;
using PipefittersAccounting.SharedModel.ReadModels;
using PipefittersAccounting.SharedModel.Readmodels.Financing;
using PipefittersAccounting.UI.Interfaces;
using PipefittersAccounting.UI.Utilities;

namespace PipefittersAccounting.UI.Finance.Components
{
    public partial class FinancierDetailDialog
    {
        private Modal? _detailModalRef;

        [Parameter] public FinancierReadModel? Financier { get; set; }

        protected override void OnParametersSet()
        {
            if (Financier is not null && _detailModalRef is not null)
            {
                _detailModalRef!.Show();
            }
        }

        private async Task CloseDialog() => await _detailModalRef!.Hide();

        private string ConvertIsActiveToString()
            => Financier!.IsActive ? "Active" : "Inactive";
    }
}