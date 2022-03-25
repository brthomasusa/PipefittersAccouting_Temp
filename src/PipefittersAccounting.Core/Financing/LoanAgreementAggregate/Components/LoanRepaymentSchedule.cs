#pragma warning disable CS8604

using System.Collections.Generic;
using System.Linq;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.Core.Financing.LoanAgreementAggregate.ValueObjects;

namespace PipefittersAccounting.Core.Financing.LoanAgreementAggregate
{
    public class LoanRepaymentSchedule
    {
        private readonly int _installments;
        private readonly DateTime _loanDate;
        private readonly DateTime _maturityDate;
        private readonly decimal _loanAmount;

        private SortedDictionary<int, LoanInstallment> _loanPaymentSchedule;

        protected LoanRepaymentSchedule(
            LoanDate loanDate,
            MaturityDate maturityDate,
            LoanAmount loanAmount
        )
        {
            _loanDate = DateTime.Parse(loanDate.Value.ToString());
            _maturityDate = DateTime.Parse(maturityDate.Value.ToString());
            _installments = MonthDiff(_loanDate, _maturityDate) + 1;
            _loanAmount = loanAmount;
            _loanPaymentSchedule = new SortedDictionary<int, LoanInstallment>();
        }

        public static LoanRepaymentSchedule Create(LoanDate loanDate, MaturityDate maturityDate, LoanAmount loanAmount)
            => new(loanDate, maturityDate, loanAmount);

        public OperationResult<bool> AddLoanInstallment(LoanInstallment installment)
        {
            try
            {
                if (installment.InstallmentNumber > _installments)
                {
                    string errMsg = $"This installment number ({installment.InstallmentNumber}) is greater the total installments ({_installments})!";
                    return OperationResult<bool>.CreateFailure(errMsg);
                }

                if (installment.PaymentDueDate < _loanDate || installment.PaymentDueDate > _maturityDate)
                {
                    string errMsg = $"The payment due date must be between {_loanDate} and {_maturityDate}.";
                    return OperationResult<bool>.CreateFailure(errMsg);
                }

                if (GetTotalRepaidPrincipal() + installment.LoanPrincipalAmount > _loanAmount)
                {
                    string errMsg = "Adding this installment would cause the repaid principal amount to exceed the loan amount.";
                    return OperationResult<bool>.CreateFailure(errMsg);
                }

                _loanPaymentSchedule.Add(installment.InstallmentNumber, installment);
                return OperationResult<bool>.CreateSuccessResult(true);
            }
            catch (Exception ex)
            {
                return OperationResult<bool>.CreateFailure(ex.Message);
            }
        }

        public bool IsValidLoanRepaymentSchedule()
        {
            return true;
        }

        private int MonthDiff(DateTime startDate, DateTime endDate)
        {
            int monthsApart = 12 * (startDate.Year - endDate.Year) + startDate.Month - endDate.Month;
            return Math.Abs(monthsApart);
        }

        private decimal GetTotalRepaidPrincipal()
        {
            decimal returnValue = 0M;

            var result = _loanPaymentSchedule.Values.ToList();
            result.ForEach(x => returnValue += x.LoanPrincipalAmount);

            return returnValue;
        }
    }
}