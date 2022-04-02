using System.Collections.ObjectModel;
using PipefittersAccounting.Core.Financing.LoanAgreementAggregate;
using PipefittersAccounting.SharedKernel.Utilities;

namespace PipefittersAccounting.Core.Financing.LoanAgreementAggregate.Components
{
    public class LoanInstallmentPaymentSchedule
    {
        private SortedDictionary<int, LoanInstallment> _loanPaymentSchedule;

        private LoanInstallmentPaymentSchedule(List<LoanInstallment> installments)
        {
            _loanPaymentSchedule = new SortedDictionary<int, LoanInstallment>();
            installments.ForEach(item => _loanPaymentSchedule.Add(item.InstallmentNumber, item));
        }

        public static OperationResult<LoanInstallmentPaymentSchedule> Create(List<LoanInstallment> installments)
        {
            // Using LoanInstallment.InstallmentNumber as disctionary key so
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
        public ReadOnlyDictionary<int, LoanInstallment> RepaymentSchedule => new(_loanPaymentSchedule);
    }
}