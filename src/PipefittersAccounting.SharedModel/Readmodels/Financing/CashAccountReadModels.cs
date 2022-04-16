
namespace PipefittersAccounting.SharedModel.Readmodels.Financing
{
    public class FinancierIdValidationModel
    {
        public Guid FinancierId { get; set; }
        public string? FinancierName { get; set; }
    }

    public class CreditorHasLoanAgreeValidationModel
    {
        public string? FinancierName { get; set; }
        public decimal LoanAmount { get; set; }
    }

    public class ReceiptLoanProceedsValidationModel
    {
        public Guid FinancierId { get; set; }
        public Guid LoanId { get; set; }
        public string? FinancierName { get; set; }
        public decimal LoanAmount { get; set; }
        public DateTime DateReceived { get; set; }
        public decimal AmountReceived { get; set; }
    }

    public class DisburesementLoanPymtValidationModel
    {
        public Guid FinancierId { get; set; }
        public Guid LoanId { get; set; }
        public Guid LoanInstallmentId { get; set; }
        public int InstallmentNumber { get; set; }
        public DateTime PaymentDueDate { get; set; }
        public decimal EqualMonthlyInstallment { get; set; }
        public DateTime DatePaid { get; set; }
        public decimal AmountPaid { get; set; }
    }
}