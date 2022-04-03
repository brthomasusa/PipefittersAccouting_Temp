using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using PipefittersAccounting.SharedKernel.CommonValueObjects;
using PipefittersAccounting.Core.Financing.LoanAgreementAggregate;
using PipefittersAccounting.Core.Financing.LoanAgreementAggregate.Components;
using PipefittersAccounting.Core.Financing.LoanAgreementAggregate.ValueObjects;
using PipefittersAccounting.SharedKernel.Utilities;

namespace PipefittersAccounting.UnitTests.Financing
{
    public class LoanAmortizationScheduleValidationTests
    {
        [Fact]
        public void LoanInstallmentPaymentSchedule_Validation_ValidInfo_Chained_ShouldSucceed()
        {
            int numberOfPayments = 12;
            DateTime firstPaymentDate = new DateTime(2022, 4, 15);
            DateTime maturityDate = new DateTime(2023, 4, 15);
            decimal loanAmount = 10000M;
            decimal interestRate = .0725M;

            OperationResult<LoanAmortizationSchedule> result =
                LoanAmortizationSchedule.Create(LoanAgreementTestData.GetInstallments12MonthsOfValidInfo());

            InstallmentNumberValidationHandler handler = new(numberOfPayments);
            handler.SetNext(new InstallmentPaymentDateValidationHandler(firstPaymentDate, maturityDate))
                   .SetNext(new InstallmentPaymentAmountValidationHandler(loanAmount, interestRate));

            var exception = Record.Exception(() => handler.Handle(result.Result));

            Assert.Null(exception);
        }

        [Fact]
        public void LoanInstallmentPaymentSchedule_Validation_InstallmentNumberOutOfRange_ShouldFail()
        {
            int numberOfPayments = 12;
            DateTime firstPaymentDate = new DateTime(2022, 4, 15);
            DateTime maturityDate = new DateTime(2023, 4, 15);

            List<LoanInstallment> installments = LoanAgreementTestData.GetInstallments12MonthsOfValidInfo();
            int index = installments.FindIndex(i => i.InstallmentNumber == 12);
            installments[index] = new LoanInstallment
                (
                    LoanPaymentEconEvent.Create(EntityGuidID.Create(Guid.NewGuid())),
                    EntityGuidID.Create(new Guid("41ca2b0a-0ed5-478b-9109-5dfda5b2eba1")),
                    InstallmentNumber.Create(13),
                    PaymentDueDate.Create(new DateTime(2023, 3, 15)),
                    EqualMonthlyInstallment.Create(865.26M),
                    LoanPrincipalAmount.Create(860M),
                    LoanInterestAmount.Create(5.26M),
                    LoanPrincipalRemaining.Create(0.00M),
                    EntityGuidID.Create(new Guid("660bb318-649e-470d-9d2b-693bfb0b2744"))
                );

            OperationResult<LoanAmortizationSchedule> result =
                LoanAmortizationSchedule.Create(installments);

            InstallmentNumberValidationHandler handler = new(numberOfPayments);

            Action action = () => handler.Handle(result.Result);

            var caughtException = Assert.Throws<InvalidOperationException>(action);
        }

        [Fact]
        public void LoanInstallmentPaymentSchedule_Validation_FirstInstallmentNumberMustEqualOne_ShouldFail()
        {
            int numberOfPayments = 12;
            DateTime firstPaymentDate = new DateTime(2022, 4, 15);
            DateTime maturityDate = new DateTime(2023, 4, 15);

            List<LoanInstallment> installments = LoanAgreementTestData.GetInstallments12MonthsOfValidInfo();

            int index = installments.FindIndex(i => i.InstallmentNumber.Value == 1);
            installments[index] = new LoanInstallment
                (
                    LoanPaymentEconEvent.Create(EntityGuidID.Create(Guid.NewGuid())),
                    EntityGuidID.Create(new Guid("41ca2b0a-0ed5-478b-9109-5dfda5b2eba1")),
                    InstallmentNumber.Create(13),
                    PaymentDueDate.Create(new DateTime(2023, 3, 15)),
                    EqualMonthlyInstallment.Create(865.26M),
                    LoanPrincipalAmount.Create(860M),
                    LoanInterestAmount.Create(5.26M),
                    LoanPrincipalRemaining.Create(0.00M),
                    EntityGuidID.Create(new Guid("660bb318-649e-470d-9d2b-693bfb0b2744"))
                );

            OperationResult<LoanAmortizationSchedule> result =
                LoanAmortizationSchedule.Create(installments);

            InstallmentNumberValidationHandler handler = new(numberOfPayments);

            Action action = () => handler.Handle(result.Result);

            var caughtException = Assert.Throws<InvalidOperationException>(action);
        }

        [Fact]
        public void LoanInstallmentPaymentSchedule_Validation_TooFewInstallments_ShouldFail()
        {
            int numberOfPayments = 12;
            DateTime firstPaymentDate = new DateTime(2022, 4, 15);
            DateTime maturityDate = new DateTime(2023, 4, 15);

            List<LoanInstallment> installments = LoanAgreementTestData.GetInstallments12MonthsOfValidInfo();
            installments.RemoveAt(11);

            OperationResult<LoanAmortizationSchedule> result =
                LoanAmortizationSchedule.Create(installments);

            InstallmentNumberValidationHandler handler = new(numberOfPayments);

            Action action = () => handler.Handle(result.Result);

            var caughtException = Assert.Throws<InvalidOperationException>(action);

            Assert.Equal($"The repayment schedule only contains {installments.Count} installments. It should contain {numberOfPayments}", caughtException.Message);
        }

        [Fact]
        public void LoanInstallmentPaymentSchedule_Validation_TooManyInstallments_ShouldFail()
        {
            int numberOfPayments = 12;
            DateTime firstPaymentDate = new DateTime(2022, 4, 15);
            DateTime maturityDate = new DateTime(2023, 4, 15);

            List<LoanInstallment> installments = LoanAgreementTestData.GetInstallments12MonthsOfValidInfo();
            installments.Add(
                new LoanInstallment
                (
                    LoanPaymentEconEvent.Create(EntityGuidID.Create(Guid.NewGuid())),
                    EntityGuidID.Create(new Guid("41ca2b0a-0ed5-478b-9109-5dfda5b2eba1")),
                    InstallmentNumber.Create(13),
                    PaymentDueDate.Create(new DateTime(2023, 4, 15)),
                    EqualMonthlyInstallment.Create(865.26M),
                    LoanPrincipalAmount.Create(807M),
                    LoanInterestAmount.Create(58.26M),
                    LoanPrincipalRemaining.Create(9193.00M),
                    EntityGuidID.Create(new Guid("660bb318-649e-470d-9d2b-693bfb0b2744"))
                )
            );


            OperationResult<LoanAmortizationSchedule> result =
                LoanAmortizationSchedule.Create(installments);

            InstallmentNumberValidationHandler handler = new(numberOfPayments);

            Action action = () => handler.Handle(result.Result);

            var caughtException = Assert.Throws<InvalidOperationException>(action);

            Assert.Equal($"The repayment schedule contains too many ({installments.Count}) installments. It should contain {numberOfPayments}", caughtException.Message);
        }

        [Fact]
        public void LoanInstallmentPaymentSchedule_Validation_2ndPymtDueDateNotGreaterThanPrevDueDate_ShouldFail()
        {
            DateTime firstPaymentDate = new DateTime(2022, 4, 15);
            DateTime maturityDate = new DateTime(2023, 4, 15);

            List<LoanInstallment> installments = LoanAgreementTestData.GetInstallments12MonthsOfValidInfo();

            int index = installments.FindIndex(i => i.InstallmentNumber == 2);
            installments[index] = new LoanInstallment
                (
                    LoanPaymentEconEvent.Create(EntityGuidID.Create(Guid.NewGuid())),
                    EntityGuidID.Create(new Guid("41ca2b0a-0ed5-478b-9109-5dfda5b2eba1")),
                    InstallmentNumber.Create(2),
                    PaymentDueDate.Create(new DateTime(2022, 4, 15)),
                    EqualMonthlyInstallment.Create(865.26M),
                    LoanPrincipalAmount.Create(860M),
                    LoanInterestAmount.Create(5.26M),
                    LoanPrincipalRemaining.Create(0.00M),
                    EntityGuidID.Create(new Guid("660bb318-649e-470d-9d2b-693bfb0b2744"))
                );

            OperationResult<LoanAmortizationSchedule> result =
                LoanAmortizationSchedule.Create(installments);

            InstallmentPaymentDateValidationHandler handler = new(firstPaymentDate, maturityDate);

            Action action = () => handler.Handle(result.Result);

            var caughtException = Assert.Throws<InvalidOperationException>(action);

            Assert.Equal("The payment due date for installment 2 can not be less than or equal the payment date for installment 1.", caughtException.Message);
        }

        [Fact]
        public void LoanInstallmentPaymentSchedule_Validation_LastPymtDueDateNotGreaterThanPrevDueDate_ShouldFail()
        {
            DateTime firstPaymentDate = new DateTime(2022, 4, 15);
            DateTime maturityDate = new DateTime(2023, 4, 15);

            List<LoanInstallment> installments = LoanAgreementTestData.GetInstallments12MonthsOfValidInfo();

            int index = installments.FindIndex(i => i.InstallmentNumber == 12);
            installments[index] = new LoanInstallment
                (
                    LoanPaymentEconEvent.Create(EntityGuidID.Create(Guid.NewGuid())),
                    EntityGuidID.Create(new Guid("41ca2b0a-0ed5-478b-9109-5dfda5b2eba1")),
                    InstallmentNumber.Create(12),
                    PaymentDueDate.Create(new DateTime(2023, 2, 14)),
                    EqualMonthlyInstallment.Create(865.26M),
                    LoanPrincipalAmount.Create(860M),
                    LoanInterestAmount.Create(5.26M),
                    LoanPrincipalRemaining.Create(0.00M),
                    EntityGuidID.Create(new Guid("660bb318-649e-470d-9d2b-693bfb0b2744"))
                );

            OperationResult<LoanAmortizationSchedule> result =
                LoanAmortizationSchedule.Create(installments);

            InstallmentPaymentDateValidationHandler handler = new(firstPaymentDate, maturityDate);

            Action action = () => handler.Handle(result.Result);

            var caughtException = Assert.Throws<InvalidOperationException>(action);

            Assert.Equal("The payment due date for installment 12 can not be less than or equal the payment date for installment 11.", caughtException.Message);
        }

        [Fact]
        public void LoanInstallmentPaymentSchedule_Validation_PymtDueDateBeforeLoanAgreementLoanDate_ShouldFail()
        {
            DateTime firstPaymentDate = new DateTime(2022, 4, 15);
            DateTime maturityDate = new DateTime(2023, 4, 15);

            List<LoanInstallment> installments = LoanAgreementTestData.GetInstallments12MonthsOfValidInfo();

            int index = installments.FindIndex(i => i.InstallmentNumber == 1);
            installments[index] = new LoanInstallment
                (
                    LoanPaymentEconEvent.Create(EntityGuidID.Create(Guid.NewGuid())),
                    EntityGuidID.Create(new Guid("41ca2b0a-0ed5-478b-9109-5dfda5b2eba1")),
                    InstallmentNumber.Create(1),
                    PaymentDueDate.Create(new DateTime(2022, 3, 15)),
                    EqualMonthlyInstallment.Create(865.26M),
                    LoanPrincipalAmount.Create(807M),
                    LoanInterestAmount.Create(58.26M),
                    LoanPrincipalRemaining.Create(9193.00M),
                    EntityGuidID.Create(new Guid("660bb318-649e-470d-9d2b-693bfb0b2744"))
                );

            OperationResult<LoanAmortizationSchedule> result =
                LoanAmortizationSchedule.Create(installments);

            InstallmentPaymentDateValidationHandler handler = new(firstPaymentDate, maturityDate);

            Action action = () => handler.Handle(result.Result);

            var caughtException = Assert.Throws<InvalidOperationException>(action);

            Assert.Equal("Invalid installment payment dates(s) found! Valid installment payment dates are between loan agreement loan date and maturity date.", caughtException.Message);
        }

        [Fact]
        public void LoanInstallmentPaymentSchedule_Validation_PymtDueDateAfterLoanAgreementMaturityDate_ShouldFail()
        {
            DateTime firstPaymentDate = new DateTime(2022, 4, 15);
            DateTime maturityDate = new DateTime(2023, 4, 15);

            List<LoanInstallment> installments = LoanAgreementTestData.GetInstallments12MonthsOfValidInfo();

            int index = installments.FindIndex(i => i.InstallmentNumber == 12);
            installments[index] = new LoanInstallment
                (
                    LoanPaymentEconEvent.Create(EntityGuidID.Create(Guid.NewGuid())),
                    EntityGuidID.Create(new Guid("41ca2b0a-0ed5-478b-9109-5dfda5b2eba1")),
                    InstallmentNumber.Create(12),
                    PaymentDueDate.Create(new DateTime(2023, 4, 16)),
                    EqualMonthlyInstallment.Create(865.26M),
                    LoanPrincipalAmount.Create(860M),
                    LoanInterestAmount.Create(5.26M),
                    LoanPrincipalRemaining.Create(0.00M),
                    EntityGuidID.Create(new Guid("660bb318-649e-470d-9d2b-693bfb0b2744"))
                );

            OperationResult<LoanAmortizationSchedule> result =
                LoanAmortizationSchedule.Create(installments);

            InstallmentPaymentDateValidationHandler handler = new(firstPaymentDate, maturityDate);

            Action action = () => handler.Handle(result.Result);

            var caughtException = Assert.Throws<InvalidOperationException>(action);

            Assert.Equal("Invalid installment payment dates(s) found! Valid installment payment dates are between loan agreement loan date and maturity date.", caughtException.Message);
        }
    }
}