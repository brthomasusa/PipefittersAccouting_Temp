namespace PipefittersAccounting.SharedModel.Readmodels.Financing
{
    public class GetLoanAgreement
    {
        public Guid LoanId { get; set; }
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

    public class GetLoanAgreementLookup { }
}