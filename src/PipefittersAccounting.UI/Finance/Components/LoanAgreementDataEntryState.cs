using PipefittersAccounting.SharedModel;
using PipefittersAccounting.SharedModel.ReadModels;
using PipefittersAccounting.SharedModel.Readmodels.Financing;
using PipefittersAccounting.SharedModel.WriteModels.Financing;

namespace PipefittersAccounting.UI.Finance.Components
{
    public class LoanAgreementDataEntryState
    {
        public LoanAgreementWriteModel LoanWriteModel { get; set; } = new();
        public List<FinancierLookup> Financiers { get; set; } = new();
    }
}