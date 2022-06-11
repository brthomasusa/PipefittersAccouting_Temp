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

        public List<LoanInstallmentWriteModel> AmortizationSchedule { get; set; }

        public Guid UserId { get; set; }
    }

    public class LoanInstallmentWriteModel : IWriteModel
    {
        public Guid LoanInstallmentId { get; set; }
        public Guid LoanId { get; set; }
        public int InstallmentNumber { get; set; }
        public DateTime PaymentDueDate { get; set; }
        public decimal EqualMonthlyInstallment { get; set; }
        public decimal LoanPrincipalAmount { get; set; }
        public decimal LoanInterestAmount { get; set; }
        public decimal LoanPrincipalRemaining { get; set; }
        public Guid UserId { get; set; }
    }
}