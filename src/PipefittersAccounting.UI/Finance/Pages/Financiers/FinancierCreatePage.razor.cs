#pragma warning disable CS8602

using PipefittersAccounting.UI.Components.Common;

namespace PipefittersAccounting.UI.Finance.Pages.Financiers
{
    public partial class FinancierCreatePage
    {
        private EditMode _formEditMode = EditMode.Creating;
        private string _returnUri = "Finance/Pages/Financiers/FinanciersListPage";
    }
}