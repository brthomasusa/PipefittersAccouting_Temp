using System.Collections.ObjectModel;

namespace PipefittersAccounting.Core.Financing.LoanAgreementAggregate.Components
{
    public class LoanAmortizationCalculator
    {
        private readonly decimal _monthlyInterestRate;
        private readonly decimal _loanPrincipalAmount;
        private readonly DateTime _firstPymtDate;
        private readonly DateTime _maturityDate;
        private readonly decimal _equalMonthlyPymt;
        private readonly int _numberOfPymts;
        private List<InstallmentRecord> _pymtSchedule;

        protected LoanAmortizationCalculator
        (
            decimal annualInterestRate,
            decimal loanPrincipalAmount,
            DateTime firstPymtDate,
            DateTime maturityDate
        )
        {
            _monthlyInterestRate = annualInterestRate / 12;
            _loanPrincipalAmount = loanPrincipalAmount;
            _firstPymtDate = firstPymtDate;
            _maturityDate = maturityDate;
            _numberOfPymts = CalcNumberOfPayments(_firstPymtDate, _maturityDate);
            _equalMonthlyPymt =
                EqualMonthlyInstallmentCalculator.CalcEqualMonthlyInstallment(_numberOfPymts,
                                                                              (double)annualInterestRate,
                                                                              _loanPrincipalAmount);
            _pymtSchedule = new();
            CalcRepaymentSchedule();
        }

        public ReadOnlyCollection<InstallmentRecord> RepaymentSchedule => new(_pymtSchedule);

        public static LoanAmortizationCalculator Create
        (
            decimal annualInterestRate,
            decimal loanPrincipalAmount,
            DateTime firstPymtDate,
            DateTime maturityDate
        )
        {
            return new(annualInterestRate, loanPrincipalAmount, firstPymtDate, maturityDate);
        }

        private void CalcRepaymentSchedule()
        {
            decimal remainingLoanBalance = _loanPrincipalAmount;
            decimal accumulatedInterest = 0M;
            DateTime pymtDueDate = _firstPymtDate;

            for (int counter = 0; counter < _numberOfPymts; counter++)
            {
                decimal interestPymt = _monthlyInterestRate * remainingLoanBalance;
                decimal principalPymt = _equalMonthlyPymt - interestPymt;
                remainingLoanBalance = remainingLoanBalance - principalPymt;
                accumulatedInterest += interestPymt;

                _pymtSchedule.Add
                (
                    new InstallmentRecord(InstallmentNumber: counter + 1,
                                    PaymentDueDate: pymtDueDate,
                                    Payment: _equalMonthlyPymt,
                                    Principal: Decimal.Round(principalPymt, 2),
                                    Interest: Decimal.Round(interestPymt, 2),
                                    TotalInterestPaid: accumulatedInterest,
                                    RemainingBalance: Decimal.Round(remainingLoanBalance, 2))
                );

                pymtDueDate = _firstPymtDate.AddMonths(counter + 1);
            }
        }

        private int CalcNumberOfPayments(DateTime startDate, DateTime endDate)
        {
            int monthsApart = 12 * (startDate.Year - endDate.Year) + startDate.Month - endDate.Month;
            return Math.Abs(monthsApart);
        }
    }
}