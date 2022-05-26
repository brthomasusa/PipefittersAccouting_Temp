#pragma warning disable CS8618

using PipefittersAccounting.SharedKernel;

namespace PipefittersAccounting.Core.Financing.LoanAgreementAggregate.ValueObjects
{
    public class LoanInterestAmount : ValueObject
    {
        public decimal Value { get; }

        protected LoanInterestAmount() { }

        private LoanInterestAmount(decimal interestAmount)
            : this()
        {
            Value = interestAmount;
        }

        public static implicit operator decimal(LoanInterestAmount self) => self.Value;

        public static LoanInterestAmount Create(decimal interestAmount)
        {
            CheckValidity(interestAmount);
            return new LoanInterestAmount(interestAmount);
        }

        private static void CheckValidity(decimal value)
        {
            if (value % 0.01M != 0)
            {
                throw new ArgumentException("The interest amount can not have more than two decimal places");
            }

            if (value < 0)
            {
                throw new ArgumentException("The loan interest amount can not be negative.");
            }
        }
    }
}