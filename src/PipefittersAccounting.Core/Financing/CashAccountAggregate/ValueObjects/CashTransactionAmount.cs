#pragma warning disable CS8618

using PipefittersAccounting.SharedKernel;

namespace PipefittersAccounting.Core.Financing.CashAccountAggregate.ValueObjects
{
    public class CashTransactionAmount : ValueObject
    {
        public decimal Value { get; }

        protected CashTransactionAmount() { }

        private CashTransactionAmount(decimal transactionAmount)
            : this()
        {
            Value = transactionAmount;
        }

        public static implicit operator decimal(CashTransactionAmount self) => self.Value;

        public static CashTransactionAmount Create(decimal transactionAmount)
        {
            CheckValidity(transactionAmount);
            return new CashTransactionAmount(transactionAmount);
        }

        private static void CheckValidity(decimal value)
        {
            if (value % 0.01M != 0)
            {
                throw new ArgumentException("The cash transaction amount can not have more than two decimal places");
            }

            if (value <= 0)
            {
                throw new ArgumentException("The cash transaction amount must be greater than $0.00.", nameof(value));
            }
        }
    }
}