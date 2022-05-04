
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

    public class DepositLoanProceedsValidationModel
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

    public class CashAccountListItem
    {
        public Guid CashAccountId { get; set; }
        public string? AccountNumber { get; set; }
        public string? BankName { get; set; }
        public string? AccountType { get; set; }
        public string? AccountName { get; set; }
        public string? Routing { get; set; }
        public decimal Inflows { get; set; }
        public decimal Outflows { get; set; }
        public decimal Balance { get; set; }
    }

    public class CashAccountDetail
    {
        public Guid CashAccountId { get; set; }
        public string? AccountNumber { get; set; }
        public string? BankName { get; set; }
        public string? AccountType { get; set; }
        public string? AccountName { get; set; }
        public string? Routing { get; set; }
        public decimal Inflows { get; set; }
        public decimal Outflows { get; set; }
        public decimal Balance { get; set; }
    }
}