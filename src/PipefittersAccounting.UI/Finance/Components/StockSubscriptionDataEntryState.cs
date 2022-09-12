using PipefittersAccounting.SharedModel.Readmodels.Financing;
using PipefittersAccounting.SharedModel.WriteModels.Financing;

namespace PipefittersAccounting.UI.Finance.Components
{
    public class StockSubscriptionDataEntryState
    {
        public StockSubscriptionWriteModel SubscriptionWriteModel { get; set; } = new();
        public List<FinancierLookup> Financiers { get; set; } = new();
    }
}