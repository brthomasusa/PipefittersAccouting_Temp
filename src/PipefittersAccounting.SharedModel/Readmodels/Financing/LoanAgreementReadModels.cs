using PipefittersAccounting.SharedModel.Interfaces;

namespace PipefittersAccounting.SharedModel.Readmodels.Financing
{
    public class LoanAgreementReadModel
    {
        public Guid LoanId { get; set; }
        public string? LoanNumber { get; set; }
        public Guid FinancierId { get; set; }
        public decimal LoanAmount { get; set; }
        public decimal InterestRate { get; set; }
        public DateTime LoanDate { get; set; }
        public DateTime MaturityDate { get; set; }
        public int NumberOfInstallments { get; set; }
        public string? FinancierName { get; set; }
        public string? AddressLine1 { get; set; }
        public string? AddressLine2 { get; set; }
        public string? CityStateZipcode { get; set; }
        public string? Telephone { get; set; }
        public string? PointOfContact { get; set; }
        public List<LoanInstallmentReadModel>? LoanInstallmentDetailsList { get; set; } = new();
        public Guid UserId { get; set; }
    }

    public class LoanAgreementListItem
    {
        public Guid LoanId { get; set; }
        public string? LoanNumber { get; set; }
        public string? FinancierName { get; set; }
        public decimal LoanAmount { get; set; }
        public decimal InterestRate { get; set; }
        public DateTime LoanDate { get; set; }
        public DateTime MaturityDate { get; set; }
        public int NumberOfInstallments { get; set; }
        public DateTime LoanProceedsReceived { get; set; }
        public decimal LoanProceedsAmount { get; set; }
    }

    public class LoanInstallmentReadModel
    {
        public Guid LoanInstallmentId { get; set; }
        public Guid LoanId { get; set; }
        public int InstallmentNumber { get; set; }
        public DateTime PaymentDueDate { get; set; }
        public decimal EqualMonthlyInstallment { get; set; }
        public decimal PrincipalAmount { get; set; }
        public decimal InterestAmount { get; set; }
        public decimal PrincipalRemaining { get; set; }
        public DateTime DatePaid { get; set; }
        public decimal AmountPaid { get; set; }
        public Guid UserId { get; set; }
    }

    public class LoanInstallmentListItem
    {
        public Guid LoanInstallmentId { get; set; }
        public int InstallmentNumber { get; set; }
        public DateTime PaymentDueDate { get; set; }
        public decimal EqualMonthlyInstallment { get; set; }
        public decimal PrincipalAmount { get; set; }
        public decimal InterestAmount { get; set; }
        public decimal PrincipalRemaining { get; set; }
        public DateTime DatePaid { get; set; }
        public decimal AmountPaid { get; set; }
    }
}