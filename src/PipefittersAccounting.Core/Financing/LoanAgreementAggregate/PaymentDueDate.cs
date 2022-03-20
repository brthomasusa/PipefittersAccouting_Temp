#pragma warning disable CS8618

using PipefittersAccounting.SharedKernel;

namespace PipefittersAccounting.Core.Financing.LoanAgreementAggregate
{
    public class PaymentDueDate : ValueObject
    {
        public DateTime Value { get; }

        protected PaymentDueDate() { }

        private PaymentDueDate(DateTime paymentDueDate)
            : this()
        {
            Value = paymentDueDate;
        }

        public static implicit operator DateTime(PaymentDueDate self) => self.Value;

        public static PaymentDueDate Create(DateTime paymentDueDate)
        {
            CheckValidity(paymentDueDate);
            return new PaymentDueDate(paymentDueDate);
        }

        private static void CheckValidity(DateTime value)
        {
            if (value == default)
            {
                throw new ArgumentNullException("The payment due date is required.");
            }
        }
    }
}