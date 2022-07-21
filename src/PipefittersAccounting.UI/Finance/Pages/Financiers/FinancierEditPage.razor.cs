#pragma warning disable CS8602

using Microsoft.AspNetCore.Components;
using PipefittersAccounting.UI.Components.Common;

namespace PipefittersAccounting.UI.Finance.Pages.Financiers
{
    public partial class FinancierEditPage
    {
        private EditMode _formEditMode = EditMode.Updating;
        private string _returnUri = "Finance/Pages/Financiers/FinanciersListPage";

        [Parameter] public Guid FinancierId { get; set; }
    }
}
