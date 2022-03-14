#pragma warning disable CS8618

using PipefittersAccounting.SharedKernel;

namespace PipefittersAccounting.Core.Financing.LoanAgreementAggregate
{
    public class MaturityDate : ValueObject
    {
        public DateTime Value { get; }

        protected MaturityDate() { }

        private MaturityDate(DateTime maturityDate)
            : this()
        {
            Value = maturityDate;
        }

        public static implicit operator DateTime(MaturityDate self) => self.Value;

        public static MaturityDate Create(DateTime maturityDate)
        {
            CheckValidity(maturityDate);
            return new MaturityDate(maturityDate);
        }

        private static void CheckValidity(DateTime value)
        {
            if (value == default)
            {
                throw new ArgumentNullException("The loan maturity date is required.", nameof(value));
            }
        }
    }
}