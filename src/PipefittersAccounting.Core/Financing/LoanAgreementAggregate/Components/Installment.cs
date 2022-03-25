using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PipefittersAccounting.Core.Financing.LoanAgreementAggregate.Components
{
    public record Installment
    (
        DateTime PaymentDueDate,
        decimal Payment,
        decimal Principal,
        decimal Interest,
        decimal TotalInterestPaid,
        decimal RemainingBalance
    );
}