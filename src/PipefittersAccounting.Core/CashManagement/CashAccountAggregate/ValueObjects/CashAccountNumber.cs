#pragma warning disable CS8618

using PipefittersAccounting.SharedKernel;

namespace PipefittersAccounting.Core.CashManagement.CashAccountAggregate.ValueObjects
{
    public class CashAccountNumber : ValueObject
    {
        public string Value { get; }

        protected CashAccountNumber() { }

        private CashAccountNumber(string acctNumber)
            : this()
        {
            Value = acctNumber;
        }

        public static implicit operator string(CashAccountNumber self) => self.Value;

        public static CashAccountNumber Create(string acctNumber)
        {
            CheckValidity(acctNumber);
            return new CashAccountNumber(acctNumber);
        }

        private static void CheckValidity(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException("The cash account number is required.", nameof(value));
            }

            if (value.Length > 50)
            {
                throw new ArgumentException("The cash account number maximum length is 50 characters.", nameof(value));
            }
        }
    }
}