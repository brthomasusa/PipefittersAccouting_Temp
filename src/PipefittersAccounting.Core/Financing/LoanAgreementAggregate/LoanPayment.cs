#pragma warning disable CS8618

using PipefittersAccounting.Core.Shared;
using PipefittersAccounting.SharedKernel;
using PipefittersAccounting.SharedKernel.CommonValueObjects;
using PipefittersAccounting.SharedKernel.Interfaces;

namespace PipefittersAccounting.Core.Financing.LoanAgreementAggregate
{
    public class LoanPayment : Entity<Guid>
    {
        protected LoanPayment() { }

        public LoanPayment
        (
            LoanPaymentEconEvt evt,
            LoanAgreement loanAgreement,
            PaymentNumber paymentNumber,
            PaymentDueDate paymentDueDate,
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
            EconomicEvent = evt.EconomicEvent;

            LoanAgreement = loanAgreement ?? throw new ArgumentNullException("The loan agreement is required.");
            PaymentNumber = paymentNumber ?? throw new ArgumentNullException("The payment number is required.");
            PaymentDueDate = paymentDueDate ?? throw new ArgumentNullException("The payment due date is required.");
            LoanPrincipalAmount = principalAmount ?? throw new ArgumentNullException("The principal amount is required.");
            LoanInterestAmount = interestAmount ?? throw new ArgumentNullException("The interest amount is required.");
            LoanPrincipalRemaining = remainPrincipal ?? throw new ArgumentNullException("The balance remaining is required.");
            UserId = userId ?? throw new ArgumentNullException("The user Id is required.");

            CheckValidity();
        }

        public virtual EconomicEvent EconomicEvent { get; private set; }

        public virtual LoanAgreement LoanAgreement { get; private set; }

        public virtual PaymentNumber PaymentNumber { get; private set; }
        public void UpdatePaymentNumber(PaymentNumber value)
        {
            PaymentNumber = value ?? throw new ArgumentNullException("The payment number is required.");
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

        protected override void CheckValidity()
        {
            if (EconomicEvent.EventType is not EventTypeEnum.LoanPaymentCashDisbursement)
            {
                throw new ArgumentException("Invalid EconomicEvent type; it must be 'EventType.CashDisbursementForLoanPayment'.");
            }

            if (PaymentNumber > (MonthDiff(LoanAgreement.LoanDate, LoanAgreement.MaturityDate)))
            {
                throw new ArgumentOutOfRangeException("Payment number can not be greater than the length (in months) of the loan agreement.", nameof(PaymentNumber));
            }

        }

        private int MonthDiff(DateTime startDate, DateTime endDate)
        {
            int m1;
            int m2;
            if (startDate < endDate)
            {
                m1 = (endDate.Month - startDate.Month);         //for years
                m2 = (endDate.Year - startDate.Year) * 12;      //for months
            }
            else
            {
                m1 = (startDate.Month - endDate.Month);//for years
                m2 = (startDate.Year - endDate.Year) * 12; //for months
            }

            return m1 + m2;
        }
    }
}