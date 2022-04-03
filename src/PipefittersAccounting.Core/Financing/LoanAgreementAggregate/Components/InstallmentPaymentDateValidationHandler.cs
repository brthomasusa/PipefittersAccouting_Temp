#pragma warning disable CS8625

namespace PipefittersAccounting.Core.Financing.LoanAgreementAggregate.Components
{
    public class InstallmentPaymentDateValidationHandler : Handler<LoanAmortizationSchedule>
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

        public override void Handle(LoanAmortizationSchedule request)
        {
            // Ensure all payment dates have been set to a valid date, not a default date
            var defaultDateResult = request.AmortizationSchedule.Values
                .Where(i => i.PaymentDueDate == default)
                .ToList();

            if (defaultDateResult is not null && defaultDateResult.Count > 0)
            {
                string errMsg = "One or more payment dates were found to have a default date value, please correct with an actual date value.";
                throw new InvalidOperationException(errMsg);
            }

            // Ensure all payment dates are withing the loan date and maturity date range
            var result = request.AmortizationSchedule.Values
                .Where(i => i.PaymentDueDate < _firstPaymentDate || i.PaymentDueDate > _maturityDate)
                .ToList();

            if (result is not null && result.Count > 0)
            {
                string errMsg = "Invalid installment payment dates(s) found! Valid installment payment dates are between loan agreement loan date and maturity date.";
                throw new InvalidOperationException(errMsg);
            }

            // Starting with the 2nd payment date, each payment date must be greater the previous payment date
            var dateOrderResult = request.AmortizationSchedule.Values.ToList();
            DateTime comparisonDate = dateOrderResult[0].PaymentDueDate.Value.AddDays(-1);

            for (int ctr = 0; ctr < dateOrderResult.Count; ctr++)
            {
                if (dateOrderResult[ctr].PaymentDueDate <= comparisonDate)
                {
                    string errMsg = $"The payment due date for installment {ctr + 1} can not be less than or equal the payment date for installment {ctr}.";
                    throw new InvalidOperationException(errMsg);
                }
                comparisonDate = dateOrderResult[ctr].PaymentDueDate;
            }

            base.Handle(request);
        }
    }
}