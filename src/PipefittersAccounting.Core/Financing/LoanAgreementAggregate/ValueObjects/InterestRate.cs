#pragma warning disable CS8618

using PipefittersAccounting.SharedKernel;

namespace PipefittersAccounting.Core.Financing.LoanAgreementAggregate.ValueObjects
{
    public class InterestRate : ValueObject
    {
        public decimal Value { get; }

        protected InterestRate() { }

        private InterestRate(decimal rate)
            : this()
        {
            Value = rate;
        }

        public static InterestRate Create(decimal rate)
        {
            CheckValidity(rate);
            return new InterestRate(rate);
        }

        public static implicit operator decimal(InterestRate self) => self.Value;

        private static void CheckValidity(decimal value)
        {
            if (value < 0)
            {
                throw new ArgumentException("The interest rate can not be negative.");
            }

            if (value >= 1M)
            {
                throw new ArgumentException("An interest rate of 100% is not allowed.");
            }
        }
    }
}