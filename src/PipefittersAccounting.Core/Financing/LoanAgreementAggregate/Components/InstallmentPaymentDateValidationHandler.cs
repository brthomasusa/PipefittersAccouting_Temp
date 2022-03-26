namespace PipefittersAccounting.Core.Financing.LoanAgreementAggregate.Components
{
    public class InstallmentPaymentDateValidationHandler : Handler<LoanInstallmentPaymentSchedule>
    {
        private readonly DateTime _firstPaymentDate;
        private readonly DateTime _maturityDate;

        public InstallmentPaymentDateValidationHandler
        (
            DateTime firstPaymentDate,
            DateTime maturityDate
        )
        {
            _firstPaymentDate = firstPaymentDate;
            _maturityDate = maturityDate;
        }

        public override void Handle(LoanInstallmentPaymentSchedule request)
        {
            var result = request.RepaymentSchedule.Values
                .Where(i => i.PaymentDueDate < _firstPaymentDate || i.PaymentDueDate > _maturityDate)
                .ToList();

            if (result is not null && result.Count > 0)
            {
                string errMsg = $"Invalid installment payment dates(s) found! Valid installment payment dates are between loan agreement loan date and maturity date.";
                throw new InvalidOperationException(errMsg);
            }

            base.Handle(request);
        }
    }
}