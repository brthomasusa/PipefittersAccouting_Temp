#pragma warning disable CS8618

using PipefittersAccounting.SharedKernel;
using PipefittersAccounting.SharedKernel.Interfaces;
using PipefittersAccounting.SharedKernel.CommonValueObjects;
using PipefittersAccounting.Core.Financing.LoanAgreementAggregate.ValueObjects;

namespace PipefittersAccounting.Core.Financing.LoanAgreementAggregate
{
    public class LoanInstallment : Entity<Guid>
    {
        protected LoanInstallment() { }

        public LoanInstallment
        (
            LoanPaymentEconEvent evt,
            EntityGuidID loanID,
            InstallmentNumber installmentNumber,
            PaymentDueDate paymentDueDate,
            EqualMonthlyInstallment emi,
            LoanPrincipalAmount principalAmount,
            LoanInterestAmount interestAmount,
            LoanPrincipalRemaining remainPrincipal,
            EntityGuidID userId
        )
            : this()
        {
            if (evt is null)
            {
                throw new ArgumentNullException("A loan payment economic event is required.");
            }
            Id = evt.EconomicEvent.Id;

            LoanId = loanID ?? throw new ArgumentNullException("The loan agreement id can not be null.");
            InstallmentNumber = installmentNumber ?? throw new ArgumentNullException("The payment number is required.");
            PaymentDueDate = paymentDueDate ?? throw new ArgumentNullException("The payment due date is required.");
            EqualMonthlyInstallment = emi ?? throw new ArgumentNullException("The equal monthly installment of the loan installment is required.");
            LoanPrincipalAmount = principalAmount ?? throw new ArgumentNullException("The principal amount is required.");
            LoanInterestAmount = interestAmount ?? throw new ArgumentNullException("The interest amount is required.");
            LoanPrincipalRemaining = remainPrincipal ?? throw new ArgumentNullException("The balance remaining is required.");
            UserId = userId ?? throw new ArgumentNullException("The user Id is required.");

            CheckValidity();
        }

        public Guid LoanId { get; init; }

        public virtual InstallmentNumber InstallmentNumber { get; private set; }
        public void UpdateInstallmentNumber(InstallmentNumber value)
        {
            InstallmentNumber = value ?? throw new ArgumentNullException("The installment number is required.");
            UpdateLastModifiedDate();
            CheckValidity();
        }

        public virtual PaymentDueDate PaymentDueDate { get; private set; }
        public void UpdatePaymentDueDate(PaymentDueDate value)
        {
            PaymentDueDate = value ?? throw new ArgumentNullException("The payment due date is required.");
            UpdateLastModifiedDate();
            CheckValidity();
        }

        public virtual EqualMonthlyInstallment EqualMonthlyInstallment { get; private set; }
        public void UpdateEqualMonthlyInstallment(EqualMonthlyInstallment value)
        {
            EqualMonthlyInstallment = value ?? throw new ArgumentNullException("The equal monthly installment of the loan installment is required.");
            UpdateLastModifiedDate();
            CheckValidity();
        }

        public virtual LoanPrincipalAmount LoanPrincipalAmount { get; private set; }
        public void UpdateLoanPrincipalAmount(LoanPrincipalAmount value)
        {
            LoanPrincipalAmount = value ?? throw new ArgumentNullException("The loan principal amount is required.");
            UpdateLastModifiedDate();
            CheckValidity();
        }

        public virtual LoanInterestAmount LoanInterestAmount { get; private set; }
        public void UpdateLoanInterestAmount(LoanInterestAmount value)
        {
            LoanInterestAmount = value ?? throw new ArgumentNullException("The loan principal amount is required.");
            UpdateLastModifiedDate();
            CheckValidity();
        }

        public virtual LoanPrincipalRemaining LoanPrincipalRemaining { get; private set; }
        public void UpdateLoanPrincipalRemaining(LoanPrincipalRemaining value)
        {
            LoanPrincipalRemaining = value ?? throw new ArgumentNullException("The loan principal balance remaining is required.");
            UpdateLastModifiedDate();
            CheckValidity();
        }

        public Guid UserId { get; private set; }
        public void UpdateUserId(EntityGuidID value)
        {
            UserId = value ?? throw new ArgumentNullException("The User id can not be null.");
            UpdateLastModifiedDate();
        }
    }
}