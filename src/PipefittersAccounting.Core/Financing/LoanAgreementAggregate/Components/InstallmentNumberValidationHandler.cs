namespace PipefittersAccounting.Core.Financing.LoanAgreementAggregate.Components
{
    public class InstallmentNumberValidationHandler : Handler<LoanInstallmentPaymentSchedule>
    {
        private readonly int _numberOfPayments;

        public InstallmentNumberValidationHandler(int payments) => _numberOfPayments = payments;

        public override void Handle(LoanInstallmentPaymentSchedule request)
        {
            if (request.RepaymentSchedule.Count < _numberOfPayments)
            {
                string errMsg = $"The repayment schedule only contains {request.RepaymentSchedule.Count} installments. It should contain {_numberOfPayments}";
                throw new InvalidOperationException(errMsg);
            }

            var result = request.RepaymentSchedule.Values
                .Where(i => i.InstallmentNumber < 1 || i.InstallmentNumber > _numberOfPayments)
                .ToList();

            if (result is not null && result.Count > 0)
            {
                string errMsg = $"Invalid installment number(s) found! Valid installment numbers are between 1 and the number of payments in the loan agreement.";
                throw new InvalidOperationException(errMsg);
            }

            base.Handle(request);
        }
    }
}