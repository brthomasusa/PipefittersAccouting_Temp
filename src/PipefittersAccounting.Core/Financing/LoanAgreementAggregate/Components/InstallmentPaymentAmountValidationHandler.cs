namespace PipefittersAccounting.Core.Financing.LoanAgreementAggregate.Components
{
    public class InstallmentPaymentAmountValidationHandler : Handler<LoanInstallmentPaymentSchedule>
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

        public override void Handle(LoanInstallmentPaymentSchedule request)
        {
            decimal principalTotal = 0M;
            decimal totalInterestPaid = 0M;
            decimal totalPayments = 0M;

            var result = request.RepaymentSchedule.Values.ToList();

            result.ForEach(x => principalTotal += x.Principal);
            result.ForEach(x => totalInterestPaid += x.Interest);
            result.ForEach(x => totalPayments += x.Payment);

            if (principalTotal != _loanAgreementAmount)
            {
                string errMsg = $"The total of principal repayments: ${principalTotal} does not match the loan agreement amount: ${_loanAgreementAmount}!";
                throw new InvalidOperationException(errMsg);
            }

            if (principalTotal + totalInterestPaid != totalPayments)
            {
                string errMsg = $"Total Equal Monthly Installment (EMI) must equal total principal repaid (${principalTotal}) + total interest paid ({totalInterestPaid})!";
                throw new InvalidOperationException(errMsg);
            }

            base.Handle(request);
        }


    }
}