namespace PipefittersAccounting.SharedModel.Readmodels.Financing
{
    public class GetLoanAgreement
    {
        public Guid LoanId { get; set; }
    }

    public class GetLoanAgreementByLoanNumber
    {
        public string? LoanNumber { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
    }

    public class GetLoanAgreements
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
    }

    public class GetLoanAgreementInstallments
    {
        public Guid LoanId { get; set; }
    }

    public class GetDuplicateLoanAgreement
    {
        public Guid FinancierId { get; set; }

        public decimal LoanAmount { get; set; }

        public decimal InterestRate { get; set; }

        public DateTime LoanDate { get; set; }

        public DateTime MaturityDate { get; set; }
    }

    public class GetLoanAgreementLookup { }
}