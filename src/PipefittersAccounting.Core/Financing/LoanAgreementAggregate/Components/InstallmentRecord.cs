namespace PipefittersAccounting.Core.Financing.LoanAgreementAggregate.Components
{
    public record InstallmentRecord
    (
        int InstallmentNumber,
        DateTime PaymentDueDate,
        decimal Payment,
        decimal Principal,
        decimal Interest,
        decimal TotalInterestPaid,
        decimal RemainingBalance
    );
}