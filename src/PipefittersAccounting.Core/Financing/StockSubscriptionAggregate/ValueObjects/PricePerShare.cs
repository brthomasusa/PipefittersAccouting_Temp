using PipefittersAccounting.SharedKernel;

namespace PipefittersAccounting.Core.Financing.StockSubscriptionAggregate.ValueObjects
{
    public class PricePerShare : ValueObject
    {
        public decimal Value { get; }

        protected PricePerShare() { }

        private PricePerShare(decimal pricePerShare)
            : this()
        {
            Value = pricePerShare;
        }

        public static implicit operator decimal(PricePerShare self) => self.Value;

        public static PricePerShare Create(decimal pricePerShare)
        {
            CheckValidity(pricePerShare);
            return new PricePerShare(pricePerShare);
        }

        private static void CheckValidity(decimal value)
        {
            if (value % 0.01M != 0)
            {
                throw new ArgumentException("The price-per-share amount can not have more than two decimal places");
            }

            if (value <= 0)
            {
                throw new ArgumentException("The price per share must be greater than $0.00.", nameof(value));
            }
        }
    }
}