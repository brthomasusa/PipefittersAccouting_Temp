#pragma warning disable CS8618

using PipefittersAccounting.SharedModel.Interfaces;

namespace PipefittersAccounting.SharedModel.WriteModels.Financing
{
    public class LoanAgreementWriteModel : IWriteModel
    {
        public Guid LoanId { get; set; }
        public Guid FinancierId { get; set; }
        public decimal LoanAmount { get; set; }
        public decimal InterestRate { get; set; }
        public DateTime LoanDate { get; set; }
        public DateTime MaturityDate { get; set; }
        public int NumberOfInstallments { get; set; }
        public List<LoanInstallmentWriteModel> AmortizationSchedule { get; set; } = new();
        public Guid UserId { get; set; }
    }

    public class LoanInstallmentWriteModel : IWriteModel
    {
        public Guid LoanInstallmentId { get; set; }
        public Guid LoanId { get; set; }
        public int InstallmentNumber { get; set; }
        public DateTime PaymentDueDate { get; set; }
        public decimal PaymentAmount { get; set; }
        public decimal PrincipalPymtAmount { get; set; }
        public decimal InterestPymtAmount { get; set; }
        public decimal PrincipalRemaining { get; set; }
        public Guid UserId { get; set; }
    }
}