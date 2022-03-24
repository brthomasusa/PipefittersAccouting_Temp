#pragma warning disable CS8618

using PipefittersAccounting.SharedKernel;

namespace PipefittersAccounting.Core.Financing.LoanAgreementAggregate
{
    public class InstallmentNumber : ValueObject
    {
        public int Value { get; }

        protected InstallmentNumber() { }

        private InstallmentNumber(int paymentsPerYear)
            : this()
        {
            Value = paymentsPerYear;
        }

        public static implicit operator int(InstallmentNumber self) => self.Value;

        public static InstallmentNumber Create(int paymentNumber)
        {
            CheckValidity(paymentNumber);
            return new InstallmentNumber(paymentNumber);
        }

        private static void CheckValidity(int value)
        {
            if (value < 1)
            {
                throw new ArgumentOutOfRangeException("The installment number must be greater than or equal to one.");
            }
        }
    }
}