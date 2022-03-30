#pragma warning disable CS8618

using PipefittersAccounting.SharedKernel;

namespace PipefittersAccounting.Core.Financing.LoanAgreementAggregate.ValueObjects
{
    public class NumberOfInstallments : ValueObject
    {
        public int Value { get; }

        protected NumberOfInstallments() { }

        private NumberOfInstallments(int paymentsPerYear)
            : this()
        {
            Value = paymentsPerYear;
        }

        public static implicit operator int(NumberOfInstallments self) => self.Value;

        public static NumberOfInstallments Create(int paymentsPerYear)
        {
            CheckValidity(paymentsPerYear);
            return new NumberOfInstallments(paymentsPerYear);
        }

        private static void CheckValidity(int value)
        {
            if (value < 1)
            {
                throw new ArgumentOutOfRangeException("The number of installments must be greater than 1.", nameof(value));
            }
        }
    }
}