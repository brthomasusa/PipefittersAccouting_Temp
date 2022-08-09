#pragma warning disable CS8618

using PipefittersAccounting.SharedKernel;

namespace PipefittersAccounting.Core.CashManagement.CashAccountAggregate.ValueObjects
{
    public class CheckNumber : ValueObject
    {
        public string Value { get; }

        protected CheckNumber() { }

        public CheckNumber(string checkNumber)
            : this()
        {
            Value = checkNumber;
        }

        public static implicit operator string(CheckNumber self) => self.Value;

        public static CheckNumber Create(string checkNumber)
        {
            CheckValidity(checkNumber);
            return new CheckNumber(checkNumber);
        }

        private static void CheckValidity(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException("The check number used to pay for this transaction is required.", nameof(value));
            }

            if (value.Length > 25)
            {
                throw new ArgumentException("The check number maximum length is 25 characters.", nameof(value));
            }
        }
    }
}