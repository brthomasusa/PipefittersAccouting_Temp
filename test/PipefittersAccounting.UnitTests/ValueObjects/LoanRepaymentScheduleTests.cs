#pragma warning disable CS8600
#pragma warning disable CS8602
#pragma warning disable CS8604
#pragma warning disable CS8625

using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using PipefittersAccounting.SharedKernel;
using PipefittersAccounting.SharedKernel.CommonValueObjects;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.Core.Financing.LoanAgreementAggregate;
using PipefittersAccounting.Core.Financing.LoanAgreementAggregate.ValueObjects;
using PipefittersAccounting.Core.Financing.LoanAgreementAggregate.Components;

namespace PipefittersAccounting.UnitTests.ValueObjects
{
    public class LoanRepaymentScheduleTests
    {
        [Fact]
        public void Create_LoanRepaymentSchedule_ShouldSucceed()
        {
            LoanRepaymentSchedule schedule =
                LoanRepaymentSchedule.Create
                (
                    LoanDate.Create(new DateTime(2022, 1, 15)),
                    MaturityDate.Create(new DateTime(2022, 3, 15)),
                    LoanAmount.Create(1500)
                );

            var installment1 = GetLoanInstallmentForEditing();
            var installment2 = GetLoanInstallmentForEditing();
            var installment3 = GetLoanInstallmentForEditing();
        }


        [Fact]
        public void AddLoanInstallment_LoanRepaymentSchedule_DueDateBeforeLoanDate_ShouldFail()
        {
            LoanRepaymentSchedule schedule =
                LoanRepaymentSchedule.Create
                (
                    LoanDate.Create(new DateTime(2022, 1, 15)),
                    MaturityDate.Create(new DateTime(2022, 3, 15)),
                    LoanAmount.Create(1500)
                );

            var installment = GetLoanInstallmentForEditing();
            installment.UpdatePaymentDueDate(PaymentDueDate.Create(new DateTime(2022, 1, 14)));

            OperationResult<bool> result = schedule.AddLoanInstallment(installment);

            Assert.False(result.Success);
        }

        [Fact]
        public void AddLoanInstallment_LoanRepaymentSchedule_DueDateAfterMaturityDate_ShouldFail()
        {
            LoanRepaymentSchedule schedule =
                LoanRepaymentSchedule.Create
                (
                    LoanDate.Create(new DateTime(2022, 1, 15)),
                    MaturityDate.Create(new DateTime(2022, 3, 15)),
                    LoanAmount.Create(1500)
                );

            var installment = GetLoanInstallmentForEditing();
            installment.UpdatePaymentDueDate(PaymentDueDate.Create(new DateTime(2022, 3, 16)));

            OperationResult<bool> result = schedule.AddLoanInstallment(installment);

            Assert.False(result.Success);
        }

        [Fact]
        public void AddLoanInstallment_LoanRepaymentSchedule_InstallmentNumberTooLarge_ShouldFail()
        {
            LoanRepaymentSchedule schedule =
                LoanRepaymentSchedule.Create
                (
                    LoanDate.Create(new DateTime(2022, 1, 15)),
                    MaturityDate.Create(new DateTime(2022, 3, 15)),
                    LoanAmount.Create(1500)
                );

            var installment = GetLoanInstallmentForEditing();
            installment.UpdateInstallmentNumber(InstallmentNumber.Create(13));

            OperationResult<bool> result = schedule.AddLoanInstallment(installment);

            Assert.False(result.Success);
        }

        [Fact]
        public void AddLoanInstallment_LoanRepaymentSchedule_RepaidPrincipalTooLarge_ShouldFail()
        {
            LoanRepaymentSchedule schedule =
                LoanRepaymentSchedule.Create
                (
                    LoanDate.Create(new DateTime(2022, 1, 15)),
                    MaturityDate.Create(new DateTime(2022, 3, 15)),
                    LoanAmount.Create(1500)
                );

            var installment = GetLoanInstallmentForEditing();
            installment.UpdatePaymentDueDate(PaymentDueDate.Create(new DateTime(2022, 3, 16)));

            OperationResult<bool> result = schedule.AddLoanInstallment(installment);

            Assert.False(result.Success);
        }

        [Fact]
        public void CalcEqualMonthlyInstallment_EqualMonthlyInstallmentCalculator_ShouldSucceed()
        {
            var emi = EqualMonthlyInstallmentCalculator.CalcEqualMonthlyInstallment(36, .08, 3500M);

            Assert.Equal(109.68M, Decimal.Round(emi, 2));
        }

        [Fact]
        public void CalcRepaymentSchedule_LoanAmortizationCalculator_ShouldSucceed()
        {
            decimal annualInterestRate = 0.06M;
            decimal loanPrincipalAmount = 5000M;
            DateTime firstPymtDate = new DateTime(2022, 4, 16);
            DateTime maturityDate = new DateTime(2023, 4, 16);

            LoanAmortizationCalculator calculator = LoanAmortizationCalculator.Create(annualInterestRate, loanPrincipalAmount, firstPymtDate, maturityDate);
            var installments = calculator.RepaymentSchedule;

            int numberOfInstallments = installments.Count;

            Assert.Equal(12, numberOfInstallments);
        }


        private LoanAgreement GetLoanAgreementForEditing() =>
            new LoanAgreement
            (
                LoanAgreementEconEvent.Create(EntityGuidID.Create(new Guid("0a7181c0-3ce9-4981-9559-157fd8e09cfb"))),
                EntityGuidID.Create(new Guid("b49471a0-5c1e-4a4d-97e7-288fb0f6338a")),
                LoanAmount.Create(33000),
                InterestRate.Create(.0725M),
                LoanDate.Create(new DateTime(2021, 11, 15)),
                MaturityDate.Create(new DateTime(2022, 10, 15)),
                NumberOfInstallments.Create(12),
                EntityGuidID.Create(new Guid("660bb318-649e-470d-9d2b-693bfb0b2744"))
            );

        private LoanInstallment GetLoanInstallmentForEditing() =>
            new
            (
                LoanPaymentEconEvent.Create(EntityGuidID.Create(Guid.NewGuid())),
                EntityGuidID.Create(new Guid("41ca2b0a-0ed5-478b-9109-5dfda5b2eba1")),
                InstallmentNumber.Create(1),
                PaymentDueDate.Create(new DateTime(2022, 4, 1)),
                LoanPrincipalAmount.Create(800M),
                LoanInterestAmount.Create(71.25M),
                LoanPrincipalRemaining.Create(32000M),
                false,
                EntityGuidID.Create(new Guid("660bb318-649e-470d-9d2b-693bfb0b2744"))
            );
    }
}