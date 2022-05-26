#pragma warning disable CS8618

using PipefittersAccounting.SharedKernel;

namespace PipefittersAccounting.Core.Financing.LoanAgreementAggregate.ValueObjects
{
    public class EqualMonthlyInstallment
    {
        public decimal Value { get; }

        protected EqualMonthlyInstallment() { }

        private EqualMonthlyInstallment(decimal emi)
        {
            Value = emi;
        }

        public static implicit operator decimal(EqualMonthlyInstallment self) => self.Value;

        public static EqualMonthlyInstallment Create(decimal emi)
        {
            CheckValidity(emi);
            return new EqualMonthlyInstallment(emi);
        }

        private static void CheckValidity(decimal value)
        {
            if (value % 0.01M != 0)
            {
                throw new ArgumentException("The equal monthly installment can not have more than two decimal places");
            }

            if (value < 0)
            {
                throw new ArgumentException("The equal monthly installment of the loan installment can not be negative.");
            }
        }
    }
}