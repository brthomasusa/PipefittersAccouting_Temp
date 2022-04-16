
namespace PipefittersAccounting.SharedModel.Readmodels.Financing
{
    public class FinancierIdValidationParams
    {
        public Guid FinancierId { get; set; }
    }

    public class ReceiptLoanProceedsValidationParams
    {
        public Guid FinancierId { get; set; }
        public Guid LoanId { get; set; }
    }

    public class CreditorHasLoanAgreeValidationParams
    {
        public Guid FinancierId { get; set; }
        public Guid LoanId { get; set; }
    }

    public class DisburesementLoanPymtValidationParams
    {
        public Guid LoanInstallmentId { get; set; }
    }
}