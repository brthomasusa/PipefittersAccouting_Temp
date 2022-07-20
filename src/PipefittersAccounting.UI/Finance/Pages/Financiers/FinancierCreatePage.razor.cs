using Microsoft.AspNetCore.Components;
using PipefittersAccounting.SharedModel.ReadModels;
using PipefittersAccounting.SharedModel.Readmodels.Financing;
using PipefittersAccounting.SharedModel.WriteModels;
using PipefittersAccounting.SharedModel.WriteModels.Financing;
using PipefittersAccounting.UI.Interfaces;
using PipefittersAccounting.UI.Utilities;

namespace PipefittersAccounting.UI.Finance.Pages.Financiers
{
    public partial class FinancierCreatePage
    {
        [Parameter] public FinancierWriteModel? FinancierWriteModel { get; set; }
        [Inject] public IFinanciersHttpService? FinanciersService { get; set; }
    }
}