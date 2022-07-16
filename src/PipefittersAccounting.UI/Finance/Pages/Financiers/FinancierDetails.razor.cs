using Microsoft.AspNetCore.Components;
using PipefittersAccounting.SharedModel.ReadModels;
using PipefittersAccounting.SharedModel.Readmodels.Financing;
using PipefittersAccounting.UI.Interfaces;
using PipefittersAccounting.UI.Utilities;

namespace PipefittersAccounting.UI.Finance.Pages.Financiers
{
    public partial class FinancierDetails
    {
        [Parameter] public Guid FinancierId { get; set; }
        [Parameter] public FinancierDetail? FinancierDetailModel { get; set; }
        [Inject] public IFinanciersHttpService? FinanciersService { get; set; }


    }
}