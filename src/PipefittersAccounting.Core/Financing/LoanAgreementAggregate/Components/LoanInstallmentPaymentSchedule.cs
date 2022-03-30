using PipefittersAccounting.SharedKernel.Utilities;

namespace PipefittersAccounting.Core.Financing.LoanAgreementAggregate.Components
{
    public class LoanInstallmentPaymentSchedule
    {
        private SortedDictionary<int, Installment> _loanPaymentSchedule;

        private LoanInstallmentPaymentSchedule(List<Installment> installments)
        {
            _loanPaymentSchedule = new SortedDictionary<int, Installment>();
            installments.ForEach(item => _loanPaymentSchedule.Add(item.InstallmentNumber, item));
        }

        public static OperationResult<LoanInstallmentPaymentSchedule> Create(List<Installment> installments)
        {
            // Using Installment.InstallmentNumber as disctionary key so
            // must check for duplicates before attempting to add to dictionary.
            var duplicates = installments.GroupBy(x => x.InstallmentNumber)
                                           .Any(installment => installment.Count() > 1);

            if (duplicates)
            {
                string errMsg = "Found multiple installments with the same installment number.";
                return OperationResult<LoanInstallmentPaymentSchedule>.CreateFailure(errMsg);
            }

            return OperationResult<LoanInstallmentPaymentSchedule>.CreateSuccessResult(new LoanInstallmentPaymentSchedule(installments));
        }

        public SortedDictionary<int, Installment> RepaymentSchedule => _loanPaymentSchedule;
    }
}