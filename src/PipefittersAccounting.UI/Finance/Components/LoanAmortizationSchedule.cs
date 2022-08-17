using System.Collections.ObjectModel;
using PipefittersAccounting.SharedModel.WriteModels.Financing;
using PipefittersAccounting.UI.Utilities;

namespace PipefittersAccounting.UI.Finance.Components
{
    public class LoanAmortizationSchedule
    {
        private SortedDictionary<int, LoanInstallmentWriteModel> _loanPaymentSchedule;

        private LoanAmortizationSchedule(List<LoanInstallmentWriteModel> installments)
        {
            _loanPaymentSchedule = new SortedDictionary<int, LoanInstallmentWriteModel>();
            installments.ForEach(item => _loanPaymentSchedule.Add(item.InstallmentNumber, item));
        }

        public static OperationResult<LoanAmortizationSchedule> Create(List<LoanInstallmentWriteModel> installments)
        {
            // Using LoanInstallment.InstallmentNumber as disctionary key means we
            // must check for duplicates before attempting to add to dictionary.
            var duplicates = installments.GroupBy(x => x.InstallmentNumber)
                                         .Any(installment => installment.Count() > 1);

            if (duplicates)
            {
                string errMsg = "Found multiple installments with the same installment number.";
                return OperationResult<LoanAmortizationSchedule>.CreateFailure(errMsg);
            }

            return OperationResult<LoanAmortizationSchedule>.CreateSuccessResult(new LoanAmortizationSchedule(installments));
        }

        public ReadOnlyDictionary<int, LoanInstallmentWriteModel> AmortizationSchedule => new(_loanPaymentSchedule);
    }
}