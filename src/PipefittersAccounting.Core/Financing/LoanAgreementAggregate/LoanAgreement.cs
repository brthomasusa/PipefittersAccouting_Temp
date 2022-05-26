#pragma warning disable CS8618

using PipefittersAccounting.SharedKernel;
using PipefittersAccounting.SharedKernel.CommonValueObjects;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedKernel.Interfaces;
using PipefittersAccounting.Core.Financing.LoanAgreementAggregate.Components;
using PipefittersAccounting.Core.Financing.LoanAgreementAggregate.ValueObjects;

namespace PipefittersAccounting.Core.Financing.LoanAgreementAggregate
{
    public class LoanAgreement : AggregateRoot<Guid>
    {
        private List<LoanInstallment> _loanAmortizationTable;

        protected LoanAgreement() => _loanAmortizationTable = new();

        public LoanAgreement
        (
            LoanAgreementEconEvent evt,
            EntityGuidID financierId,
            LoanAmount loanAmount,
            InterestRate interestRate,
            LoanDate loanDate,
            MaturityDate maturityDate,
            NumberOfInstallments numberOfInstallments,
            EntityGuidID userId,
            List<LoanInstallment> installments
        )
            : this()
        {
            if (evt is null)
            {
                throw new ArgumentNullException("A loan agreement economic event is required.");
            }
            Id = evt.EconomicEvent.Id;

            FinancierId = financierId ?? throw new ArgumentNullException("The financier Id is required.");
            LoanAmount = loanAmount ?? throw new ArgumentNullException("The loan amount for this loan agreement is required.");
            InterestRate = interestRate ?? throw new ArgumentNullException("The interest rate is required; if zero then pass in 0.");
            LoanDate = loanDate ?? throw new ArgumentNullException("The loan agreement date is required.");
            MaturityDate = maturityDate ?? throw new ArgumentNullException("The loan maturity date is required.");
            NumberOfInstallments = numberOfInstallments ?? throw new ArgumentNullException("The number of installments is required.");
            UserId = userId ?? throw new ArgumentNullException("The user Id is required.");

            if (installments is null)
            {
                throw new ArgumentNullException("A loan agreement must have a repayment schedule.");
            }
            AddLoanInstallmentPaymentSchedule(installments);

            CheckValidity();
        }

        public Guid FinancierId { get; private set; }

        public virtual LoanAmount LoanAmount { get; private set; }

        public virtual InterestRate InterestRate { get; private set; }

        public virtual LoanDate LoanDate { get; private set; }

        public virtual MaturityDate MaturityDate { get; private set; }

        public virtual NumberOfInstallments NumberOfInstallments { get; private set; }

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

        public void UpdateNumberOfInstallments(NumberOfInstallments value)
        {
            NumberOfInstallments = value ?? throw new ArgumentNullException("The number of installments is required.");
            UpdateLastModifiedDate();
            CheckValidity();
        }

        public void UpdateUserId(EntityGuidID value)
        {
            UserId = value ?? throw new ArgumentNullException("The User id can not be null.");
            UpdateLastModifiedDate();
        }

        public virtual IReadOnlyCollection<LoanInstallment> LoanAmortizationTable => _loanAmortizationTable.ToList();

        private void AddLoanInstallmentPaymentSchedule(List<LoanInstallment> installments)
        {
            DateTime firstPaymentDate = LoanDate.Value.AddMonths(1);

            OperationResult<LoanAmortizationSchedule> result =
                LoanAmortizationSchedule.Create(installments);

            if (result.Success)
            {
                InstallmentNumberValidationHandler handler = new(NumberOfInstallments);
                handler.SetNext(new InstallmentPaymentDateValidationHandler(firstPaymentDate, MaturityDate.Value))
                       .SetNext(new InstallmentPaymentAmountValidationHandler(LoanAmount));

                handler.Handle(result.Result);

                _loanAmortizationTable.AddRange(installments);
            }
            else
            {
                throw new ArgumentException(result.NonSuccessMessage);
            }
        }

        public void UpdateLoanInstallmentPaymentSchedule(List<LoanInstallment> installments)
        {
            if (installments is null)
            {
                throw new ArgumentNullException("A loan agreement must have a repayment schedule.");
            }

            DateTime firstPaymentDate = LoanDate.Value.AddMonths(1);

            OperationResult<LoanAmortizationSchedule> result =
                LoanAmortizationSchedule.Create(installments);

            if (result.Success)
            {
                InstallmentNumberValidationHandler handler = new(NumberOfInstallments);
                handler.SetNext(new InstallmentPaymentDateValidationHandler(firstPaymentDate, MaturityDate.Value))
                       .SetNext(new InstallmentPaymentAmountValidationHandler(LoanAmount));

                handler.Handle(result.Result);

                _loanAmortizationTable = new();
                _loanAmortizationTable.AddRange(installments);
            }
            else
            {
                throw new ArgumentException(result.NonSuccessMessage);
            }
        }

        protected override void CheckValidity()
        {
            if (DateTime.Compare(MaturityDate, LoanDate) < 0)
            {
                throw new ArgumentException("Loan maturity date must be greater than or equal to the loan date.");
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