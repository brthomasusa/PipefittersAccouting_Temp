namespace PipefittersAccounting.UI.Sqlite
{
    public class LoanInstallmentModel
    {
        public int InstallmentNumber { get; set; }
        public DateTime PaymentDueDate { get; set; }
        public decimal PaymentAmount { get; set; }
        public decimal PrincipalPymtAmount { get; set; }
        public decimal InterestPymtAmount { get; set; }
        public decimal PrincipalRemaining { get; set; }
    }
}