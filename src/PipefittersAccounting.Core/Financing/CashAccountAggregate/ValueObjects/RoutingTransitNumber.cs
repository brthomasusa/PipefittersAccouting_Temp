#pragma warning disable CS8618

using System.Text.RegularExpressions;
using PipefittersAccounting.SharedKernel;

namespace PipefittersAccounting.Core.Financing.CashAccountAggregate.ValueObjects
{
    public class RoutingTransitNumber : ValueObject
    {
        public string Value { get; }

        protected RoutingTransitNumber() { }

        private RoutingTransitNumber(string routingNumber)
            : this()
        {
            Value = routingNumber;
        }

        public static implicit operator string(RoutingTransitNumber self) => self.Value;

        public static RoutingTransitNumber Create(string routingNumber)
        {
            CheckValidity(routingNumber);
            return new RoutingTransitNumber(routingNumber);
        }

        private static void CheckValidity(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException("The bank routing number is required.", nameof(value));
            }

            //TODO Define regex for U.S. bank routing number
            // string buffer = value.Replace("-", "");
            // if (!Regex.IsMatch(buffer, rgRoutingNumber))
            // {
            //     throw new ArgumentException("Invalid bank routing (Transit ABA) number!", nameof(value));
            // }
        }
    }
}