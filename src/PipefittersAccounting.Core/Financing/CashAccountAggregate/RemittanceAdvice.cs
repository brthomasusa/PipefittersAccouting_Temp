#pragma warning disable CS8618

using PipefittersAccounting.SharedKernel;

namespace PipefittersAccounting.Core.Financing.CashAccountAggregate
{
    public class RemittanceAdvice : ValueObject
    {
        public string Value { get; }

        protected RemittanceAdvice() { }

        public RemittanceAdvice(string RemittanceAdvice)
            : this()
        {
            Value = RemittanceAdvice;
        }

        public static implicit operator string(RemittanceAdvice self) => self.Value;

        public static RemittanceAdvice Create(string RemittanceAdvice)
        {
            CheckValidity(RemittanceAdvice);
            return new RemittanceAdvice(RemittanceAdvice);
        }

        private static void CheckValidity(string value)
        {
            if (value.Length > 50)
            {
                throw new ArgumentException("The RemittanceAdvice maximum length is 50 characters.", nameof(value));
            }
        }
    }
}