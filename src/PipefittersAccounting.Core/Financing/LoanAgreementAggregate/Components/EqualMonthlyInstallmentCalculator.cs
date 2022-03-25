using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PipefittersAccounting.Core.Financing.LoanAgreementAggregate.Components
{
    public class EqualMonthlyInstallmentCalculator
    {
        public static decimal CalcEqualMonthlyInstallment
        (
            int numberOfPymts,
            double annualInterestRate,
            decimal totalPrincipalAmt
        )
        {
            double monthlyInterestRate = annualInterestRate / 12;
            var varDividend = monthlyInterestRate * (Math.Pow(1 + monthlyInterestRate, numberOfPymts));
            var varDivisor = Math.Pow(1 + monthlyInterestRate, numberOfPymts) - 1;
            var varQuotient = (decimal)varDividend / (decimal)varDivisor;

            return totalPrincipalAmt * varQuotient;
        }
    }
}