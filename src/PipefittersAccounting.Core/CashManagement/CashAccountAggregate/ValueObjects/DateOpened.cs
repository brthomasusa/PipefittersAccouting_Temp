#pragma warning disable CS8618

using PipefittersAccounting.SharedKernel;

namespace PipefittersAccounting.Core.CashManagement.CashAccountAggregate.ValueObjects
{
    public class DateOpened : ValueObject
    {
        public DateTime Value { get; }

        protected DateOpened() { }

        private DateOpened(DateTime dateOpened)
            : this()
        {
            Value = dateOpened;
        }

        public static implicit operator DateTime(DateOpened self) => self.Value;

        public static DateOpened Create(DateTime dateOpened)
        {
            CheckValidity(dateOpened);
            return new DateOpened(dateOpened);
        }

        private static void CheckValidity(DateTime value)
        {
            if (value == default)
            {
                throw new ArgumentNullException("The account open date is required.", nameof(value));
            }
        }
    }
}