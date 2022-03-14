#pragma warning disable CS8618

using PipefittersAccounting.Core.Shared;
using PipefittersAccounting.Core.Financing.FinancierAggregate;
using PipefittersAccounting.SharedKernel;
using PipefittersAccounting.SharedKernel.CommonValueObjects;
using PipefittersAccounting.SharedKernel.Interfaces;

namespace PipefittersAccounting.Core.Financing.LoanAgreementAggregate
{
    public class LoanAgreement : AggregateRoot<Guid>, IAggregateRoot
    {
        private List<LoanPayment> _loanPayments;

        protected LoanAgreement() { }

        public LoanAgreement
        (
            LoanAgreementEconEvt evt,
            Financier financier,
            LoanAmount loanAmount,
            InterestRate interestRate,
            LoanDate loanDate,
            MaturityDate maturityDate,
            PaymentsPerYear paymentsPerYear,
            EntityGuidID userId
        )
            : this()
        {
            if (evt is null)
            {
                throw new ArgumentNullException("A loan agreement economic event is required.");
            }
            Id = evt.EconomicEvent.Id;
            EconomicEvent = evt.EconomicEvent;

            Financier = financier ?? throw new ArgumentNullException("The creditor who signed this loan agreement is required.");
            LoanAmount = loanAmount ?? throw new ArgumentNullException("The loan amount for this loan agreement is required.");
            InterestRate = interestRate ?? throw new ArgumentNullException("The interest rate is required; if zero then pass in 0.");
            LoanDate = loanDate ?? throw new ArgumentNullException("The loan agreement date is required.");
            MaturityDate = maturityDate ?? throw new ArgumentNullException("The loan maturity date is required.");
            PaymentsPerYear = paymentsPerYear ?? throw new ArgumentNullException("The number of loan payments per year is required.");
            UserId = userId ?? throw new ArgumentNullException("The user Id is required.");

            CheckValidity();
            _loanPayments = new List<LoanPayment>();
        }

        public virtual EconomicEvent EconomicEvent { get; private set; }

        public virtual Financier Financier { get; private set; }

        public virtual LoanAmount LoanAmount { get; private set; }

        public virtual InterestRate InterestRate { get; private set; }

        public virtual LoanDate LoanDate { get; private set; }

        public virtual MaturityDate MaturityDate { get; private set; }

        public virtual PaymentsPerYear PaymentsPerYear { get; private set; }

        public Guid UserId { get; private set; }

        public void UpdateLoanAmount(LoanAmount value)
        {
            LoanAmount = value ?? throw new ArgumentNullException("The loan amount for this loan agreement is required.");
            UpdateLastModifiedDate();
            CheckValidity();
        }

        public void UpdateInterestRate(InterestRate value)
        {
            InterestRate = value ?? throw new ArgumentNullException("The interest rate is required; if zero then pass in 0.");
            UpdateLastModifiedDate();
            CheckValidity();
        }

        public void UpdateLoanDate(LoanDate value)
        {
            LoanDate = value ?? throw new ArgumentNullException("The loan agreement date is required.");
            UpdateLastModifiedDate();
            CheckValidity();
        }

        public void UpdateMaturityDate(MaturityDate value)
        {
            MaturityDate = value ?? throw new ArgumentNullException("The loan maturity date is required.");
            UpdateLastModifiedDate();
            CheckValidity();
        }

        public void UpdatePaymentsPerYear(PaymentsPerYear value)
        {
            PaymentsPerYear = value ?? throw new ArgumentNullException("The number of loan payments per year is required.");
            UpdateLastModifiedDate();
            CheckValidity();
        }

        public void UpdateUserId(EntityGuidID value)
        {
            UserId = value ?? throw new ArgumentNullException("The User id can not be null.");
            UpdateLastModifiedDate();
        }

        public virtual IReadOnlyList<LoanPayment> LoanPaymentSchedule => _loanPayments.ToList();

        protected override void CheckValidity()
        {
            if (EconomicEvent.EventType is not EventTypeEnum.LoanAgreementCashReceipt)
            {
                throw new ArgumentException("Invalid EconomicEvent type; it must be 'EventType.CashReceiptFromLoanAgreement'.");
            }

            if (DateTime.Compare(MaturityDate, LoanDate) < 0)
            {
                throw new ArgumentException("Loan maturity date must be greater than or equal to the loan date.");
            }
        }
    }
}