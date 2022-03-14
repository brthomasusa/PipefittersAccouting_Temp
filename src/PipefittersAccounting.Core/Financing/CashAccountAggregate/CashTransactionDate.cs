#pragma warning disable CS8618

using PipefittersAccounting.SharedKernel;

namespace PipefittersAccounting.Core.Financing.CashAccountAggregate
{
    public class CashTransactionDate : ValueObject
    {
        public DateTime Value { get; }

        protected CashTransactionDate() { }

        public CashTransactionDate(DateTime transactionDate)
            : this()
        {
            Value = transactionDate;
        }

        public static implicit operator DateTime(CashTransactionDate self) => self.Value;

        public static CashTransactionDate Create(DateTime transactionDate)
        {
            CheckValidity(transactionDate);
            return new CashTransactionDate(transactionDate);
        }

        private static void CheckValidity(DateTime value)
        {
            if (value == default)
            {
                throw new ArgumentNullException("The cash account transaction date is required.", nameof(value));
            }
        }
    }
}