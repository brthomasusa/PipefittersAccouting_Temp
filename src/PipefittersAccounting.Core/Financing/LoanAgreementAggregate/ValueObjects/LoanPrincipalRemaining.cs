#pragma warning disable CS8618

using PipefittersAccounting.SharedKernel;

namespace PipefittersAccounting.Core.Financing.LoanAgreementAggregate.ValueObjects
{
    public class LoanPrincipalRemaining : ValueObject
    {
        public decimal Value { get; }

        protected LoanPrincipalRemaining() { }

        private LoanPrincipalRemaining(decimal principalRemainingAmount)
            : this()
        {
            Value = principalRemainingAmount;
        }

        public static implicit operator decimal(LoanPrincipalRemaining self) => self.Value;

        public static LoanPrincipalRemaining Create(decimal principalRemainingAmount)
        {
            CheckValidity(principalRemainingAmount);
            return new LoanPrincipalRemaining(principalRemainingAmount);
        }

        private static void CheckValidity(decimal value)
        {
            if (value % 0.01M != 0)
            {
                throw new ArgumentException("The balance remaining amount can not have more than two decimal places");
            }

            if (value < 0)
            {
                throw new ArgumentException("The loan principal remaining amount can not be negative.", nameof(value));
            }
        }
    }
}