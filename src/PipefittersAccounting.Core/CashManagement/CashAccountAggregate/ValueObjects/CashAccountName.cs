#pragma warning disable CS8618

using PipefittersAccounting.SharedKernel;

namespace PipefittersAccounting.Core.CashManagement.CashAccountAggregate.ValueObjects
{
    public class CashAccountName : ValueObject
    {
        public string Value { get; }

        protected CashAccountName() { }

        private CashAccountName(string acctName)
            : this()
        {
            Value = acctName;
        }

        public static implicit operator string(CashAccountName self) => self.Value;

        public static CashAccountName Create(string acctName)
        {
            CheckValidity(acctName);
            return new CashAccountName(acctName);
        }

        private static void CheckValidity(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException("The cash account name is required.", nameof(value));
            }

            if (value.Length > 50)
            {
                throw new ArgumentException("The cash account name maximum length is 50 characters.", nameof(value));
            }
        }
    }
}