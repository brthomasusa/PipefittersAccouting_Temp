namespace PipefittersAccounting.Core.Financing.LoanAgreementAggregate.Components
{
    public class InstallmentPaymentAmountValidationHandler : Handler<LoanAmortizationSchedule>
    {
        private readonly decimal _loanAgreementAmount;
        private readonly decimal _annualInterestRate;

        public InstallmentPaymentAmountValidationHandler
        (
            decimal loanAmount,
            decimal interestRate
        )
        {
            _loanAgreementAmount = loanAmount;
            _annualInterestRate = interestRate;
        }

        public override void Handle(LoanAmortizationSchedule request)
        {
            decimal principalTotal = 0M;
            decimal totalInterestPaid = 0M;
            decimal totalPayments = 0M;

            var result = request.AmortizationSchedule.Values.ToList();

            result.ForEach(x => principalTotal += x.LoanPrincipalAmount);
            result.ForEach(x => totalInterestPaid += x.LoanInterestAmount);
            result.ForEach(x => totalPayments += x.EqualMonthlyInstallment);

            if (principalTotal != _loanAgreementAmount)
            {
                string errMsg = $"The total of principal repayments: ${principalTotal} does not match the loan agreement amount: ${_loanAgreementAmount}!";
                throw new InvalidOperationException(errMsg);
            }

            if (principalTotal + totalInterestPaid != totalPayments)
            {
                string errMsg = $"Total of Equal Monthly Installment (EMI) payments (${totalPayments}) must equal total of principal repaid (${principalTotal}) + total of interest paid ({totalInterestPaid})!";
                throw new InvalidOperationException(errMsg);
            }

            base.Handle(request);
        }


    }
}