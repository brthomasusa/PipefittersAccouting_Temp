namespace PipefittersAccounting.Core.Financing.LoanAgreementAggregate.Components
{
    public class LoanInstallmentPaymentSchedule
    {
        private SortedDictionary<int, Installment> _loanPaymentSchedule;

        public LoanInstallmentPaymentSchedule
        (
            SortedDictionary<int, Installment> loanPaymentSchedule
        )
        => _loanPaymentSchedule = loanPaymentSchedule;

        public SortedDictionary<int, Installment> RepaymentSchedule => _loanPaymentSchedule;
    }
}