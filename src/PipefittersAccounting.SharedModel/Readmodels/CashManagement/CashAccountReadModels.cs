namespace PipefittersAccounting.SharedModel.Readmodels.CashManagement
{
    public class DepositLoanProceedsValidationModel
    {
        public Guid FinancierId { get; set; }
        public Guid LoanId { get; set; }
        public string FinancierName { get; set; } = default!;
        public decimal LoanAmount { get; set; }
        public DateTime DateReceived { get; set; }
        public decimal AmountReceived { get; set; }
    }

    public class CashAccountListItem
    {
        public Guid CashAccountId { get; set; }
        public string AccountType { get; set; } = default!;
        public string BankName { get; set; } = default!;
        public string AccountName { get; set; } = default!;
        public string AccountNumber { get; set; } = default!;
        public string RoutingTransitNumber { get; set; } = default!;
        public DateTime DateOpened { get; set; }
        public Guid UserId { get; set; }
        public decimal Inflow { get; set; }
        public decimal Outflow { get; set; }
        public decimal Balance { get; set; }
    }

    public class CashAccountDetail
    {
        public Guid CashAccountId { get; set; }
        public string? AccountType { get; set; }
        public string? BankName { get; set; }
        public string? AccountName { get; set; }
        public string? AccountNumber { get; set; }
        public string? RoutingTransitNumber { get; set; }
        public DateTime DateOpened { get; set; }
        public Guid UserId { get; set; }
        public decimal Inflow { get; set; }
        public decimal Outflow { get; set; }
        public decimal Balance { get; set; }
    }

    public class CashAccountReadModel
    {
        public Guid CashAccountId { get; set; }
        public int CashAccountTypeId { get; set; }
        public string? BankName { get; set; }
        public string? AccountName { get; set; }
        public string? AccountNumber { get; set; }
        public string? RoutingTransitNumber { get; set; }
        public DateTime DateOpened { get; set; }
        public Guid UserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastModifiedDate { get; set; }
    }

    public class CreditorIssuedLoanAgreementValidationInfo
    {
        public Guid FinancierId { get; set; }
        public string? FinancierName { get; set; }
        public Guid LoanId { get; set; }
        public decimal LoanAmount { get; set; }
    }

    public class CashReceiptOfDebtIssueProceedsInfo
    {
        public Guid FinancierId { get; set; }
        public Guid LoanId { get; set; }
        public string? FinancierName { get; set; }
        public DateTime LoanDate { get; set; }
        public DateTime MaturityDate { get; set; }
        public decimal LoanAmount { get; set; }
        public DateTime? DateReceived { get; set; }
        public decimal AmountReceived { get; set; }
    }

    public class CreditorIsOwedThisLoanInstallmentValidationInfo
    {
        public Guid FinancierId { get; set; }
        public Guid LoanId { get; set; }
        public Guid LoanInstallmentId { get; set; }
    }

    public class CashDisbursementForLoanInstallmentPaymentInfo
    {
        public Guid FinancierId { get; set; }
        public Guid LoanId { get; set; }
        public Guid LoanInstallmentId { get; set; }
        public DateTime LoanDate { get; set; }
        public DateTime MaturityDate { get; set; }
        public decimal EqualMonthlyInstallment { get; set; }
        public DateTime DatePaid { get; set; }
        public decimal AmountPaid { get; set; }
    }

    public class CashAccountTransactionDetail
    {
        public int CashTransactionId { get; set; }
        public DateTime CashAcctTransactionDate { get; set; }
        public decimal CashAcctTransactionAmount { get; set; }
        public Guid AgentId { get; set; }
        public int AgentTypeId { get; set; }
        public string? AgentName { get; set; }
        public string? AgentTypeName { get; set; }
        public Guid EventId { get; set; }
        public string? CashTransactionTypeName { get; set; }
        public string? CheckNumber { get; set; }
        public string? RemittanceAdvice { get; set; }
        public Guid UserId { get; set; }
        public string? UserFullName { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastModifiedDate { get; set; }
    }

    public class CashAccountTransactionListItem
    {
        public int CashTransactionId { get; set; }
        public DateTime CashAcctTransactionDate { get; set; }
        public decimal CashAcctTransactionAmount { get; set; }
        public Guid AgentId { get; set; }
        public int AgentTypeId { get; set; }
        public string? AgentName { get; set; }
        public string? AgentTypeName { get; set; }
        public string? CashTransactionTypeName { get; set; }
    }

    public class TimeCardPaymentInfo
    {
        public Guid TimeCardId { get; set; }
        public Guid EmployeeId { get; set; }
        public string? EmployeeName { get; set; }
        public DateTime PayPeriodEnded { get; set; }
        public decimal NetPay { get; set; }
    }
}
