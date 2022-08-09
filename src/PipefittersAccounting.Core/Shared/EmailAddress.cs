#pragma warning disable CS8618

using System.Net.Mail;
using PipefittersAccounting.SharedKernel;

namespace PipefittersAccounting.Core.Shared
{
    public class EmailAddress : ValueObject
    {
        public string Value { get; }

        protected EmailAddress() { }

        private EmailAddress(string email)
            : this()
        {
            Value = email;
        }

        public static implicit operator string(EmailAddress self) => self.Value;

        public static EmailAddress Create(string value)
        {
            CheckValidity(value);
            return new EmailAddress(value);
        }

        private static void CheckValidity(string value)
        {
            try
            {
                MailAddress emailAddress = new MailAddress(value);
            }
            catch (FormatException)
            {
                throw new ArgumentException("Invalid email address!", nameof(value));
            }
        }
    }
}