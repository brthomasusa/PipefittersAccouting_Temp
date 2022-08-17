namespace PipefittersAccounting.UI.Finance.Components
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