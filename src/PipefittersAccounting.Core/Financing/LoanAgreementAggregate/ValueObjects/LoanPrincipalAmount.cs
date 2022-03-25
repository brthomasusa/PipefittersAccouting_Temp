#pragma warning disable CS8618

using PipefittersAccounting.SharedKernel;

namespace PipefittersAccounting.Core.Financing.LoanAgreementAggregate.ValueObjects
{
    public class LoanPrincipalAmount : ValueObject
    {
        public decimal Value { get; }

        protected LoanPrincipalAmount() { }

        private LoanPrincipalAmount(decimal principalAmount)
            : this()
        {
            Value = principalAmount;
        }

        public static implicit operator decimal(LoanPrincipalAmount self) => self.Value;

        public static LoanPrincipalAmount Create(decimal principalAmount)
        {
            CheckValidity(principalAmount);
            return new LoanPrincipalAmount(principalAmount);
        }

        private static void CheckValidity(decimal value)
        {
            if (value < 0)
            {
                throw new ArgumentException("The principal amount of the loan installment can not negative.");
            }
        }
    }
}