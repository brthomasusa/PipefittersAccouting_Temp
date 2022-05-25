using PipefittersAccounting.SharedKernel;

namespace PipefittersAccounting.Core.Financing.StockSubscriptionAggregate.ValueObjects
{
    public class SharesIssured
    {
        public int Value { get; }

        protected SharesIssured() { }

        private SharesIssured(int sharesIssued)
            : this()
        {
            Value = sharesIssued;
        }

        public static implicit operator int(SharesIssured self) => self.Value;

        public static SharesIssured Create(int sharesIssued)
        {
            CheckValidity(sharesIssued);
            return new SharesIssured(sharesIssued);
        }

        private static void CheckValidity(int value)
        {
            if (value < 1)
            {
                throw new ArgumentOutOfRangeException("Shares issued must be greater than or equal to one.", nameof(value));
            }
        }
    }
}