using System;
using PipefittersAccounting.SharedKernel;

namespace PipefittersAccounting.Core.Financing.StockSubscriptionAggregate.ValueObjects
{
    public class StockIssueDate : ValueObject
    {
        public DateTime Value { get; }

        protected StockIssueDate() { }

        private StockIssueDate(DateTime stockIssueDate)
            : this()
        {
            Value = stockIssueDate;
        }

        public static implicit operator DateTime(StockIssueDate self) => self.Value;

        public static StockIssueDate Create(DateTime stockIssueDate)
        {
            CheckValidity(stockIssueDate);
            return new StockIssueDate(stockIssueDate);
        }

        private static void CheckValidity(DateTime value)
        {
            if (value == default)
            {
                throw new ArgumentNullException("The stock issue date is required.", nameof(value));
            }
        }
    }
}