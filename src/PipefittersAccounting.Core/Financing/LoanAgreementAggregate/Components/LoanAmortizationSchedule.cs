using System.Collections.ObjectModel;
using PipefittersAccounting.Core.Financing.LoanAgreementAggregate;
using PipefittersAccounting.SharedKernel.Utilities;

namespace PipefittersAccounting.Core.Financing.LoanAgreementAggregate.Components
{
    public class LoanAmortizationSchedule
    {
        private SortedDictionary<int, LoanInstallment> _loanPaymentSchedule;

        private LoanAmortizationSchedule(List<LoanInstallment> installments)
        {
            _loanPaymentSchedule = new SortedDictionary<int, LoanInstallment>();
            installments.ForEach(item => _loanPaymentSchedule.Add(item.InstallmentNumber, item));
        }

        public static OperationResult<LoanAmortizationSchedule> Create(List<LoanInstallment> installments)
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

        public ReadOnlyDictionary<int, LoanInstallment> AmortizationSchedule => new(_loanPaymentSchedule);
    }
}