namespace PipefittersAccounting.Core.Financing.LoanAgreementAggregate.Components
{
    public class InstallmentNumberValidationHandler : Handler<LoanInstallmentPaymentSchedule>
    {
        private readonly int _numberOfPayments;

        public InstallmentNumberValidationHandler(int payments) => _numberOfPayments = payments;

        public override void Handle(LoanInstallmentPaymentSchedule request)
        {
            // Ensure correct number of installments
            if (request.RepaymentSchedule.Count < _numberOfPayments)
            {
                string errMsg = $"The repayment schedule only contains {request.RepaymentSchedule.Count} installments. It should contain {_numberOfPayments}";
                throw new InvalidOperationException(errMsg);
            }

            if (request.RepaymentSchedule.Count > _numberOfPayments)
            {
                string errMsg = $"The repayment schedule contains too many ({request.RepaymentSchedule.Count}) installments. It should contain {_numberOfPayments}";
                throw new InvalidOperationException(errMsg);
            }


            var dictKeys = request.RepaymentSchedule.Keys.ToList();

            // Dictionary keys must be numbered 1 throught numberOfPayments
            if (dictKeys.First() != 1 || dictKeys.Last() != _numberOfPayments)
            {
                throw new InvalidOperationException("The numbering of the dictionary is out of order.");
            }

            // Ensure installment numbers are numbered sequencially from one to number of payments
            foreach (KeyValuePair<int, LoanInstallment> entry in request.RepaymentSchedule)
            {
                if (entry.Key != entry.Value.InstallmentNumber)
                {
                    string errMsg = $"The installment number for installment number {entry.Key} should equal {entry.Key} but actually equals {entry.Value.InstallmentNumber}.";
                    throw new InvalidOperationException(errMsg);
                }
            }

            // var result = request.RepaymentSchedule.Values
            //     .Where(i => i.InstallmentNumber < 1 || i.InstallmentNumber > _numberOfPayments)
            //     .ToList();

            // if (result is not null && result.Count > 0)
            // {
            //     string errMsg = $"Invalid installment number(s) found! Valid installment numbers are between 1 and {_numberOfPayments}.";
            //     throw new InvalidOperationException(errMsg);
            // }

            base.Handle(request);
        }
    }
}