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
        public void LoanAmortizationCalculator_Create_ShouldSucceed()
        {
            decimal annualInterestRate = 0.025M;
            decimal principal = 5000M;
            DateTime firstPaymentDate = new DateTime(2022, 4, 15);
            DateTime maturityDate = new DateTime(2023, 4, 15);

            LoanAmortizationCalculator calculator =
                LoanAmortizationCalculator.Create(annualInterestRate, principal, firstPaymentDate, maturityDate);

            Assert.NotNull(calculator);

            PipefittersAccounting.SharedModel.WriteModels.Financing.LoanAgreementWriteModel model
                = new() { LoanId = Guid.NewGuid() };

            foreach (InstallmentRecord installment in calculator.RepaymentSchedule)
            {
                model!.AmortizationSchedule.Add
                (
                    new PipefittersAccounting.SharedModel.WriteModels.Financing.LoanInstallmentWriteModel()
                    {
                        LoanInstallmentId = Guid.NewGuid(),
                        LoanId = model!.LoanId,
                        InstallmentNumber = installment.InstallmentNumber,
                        PaymentDueDate = installment.PaymentDueDate,
                        PaymentAmount = installment.Payment,
                        PrincipalPymtAmount = installment.Principal,
                        InterestPymtAmount = installment.Interest,
                        PrincipalRemaining = installment.RemainingBalance,
                        UserId = new Guid("660bb318-649e-470d-9d2b-693bfb0b2744")
                    }
                );
            }

            int count = model!.AmortizationSchedule.Count;
            Assert.Equal(12, count);
        }

        [Fact]
        public void LoanInstallmentPaymentSchedule_Validation_ValidInfo_Chained_ShouldSucceed()
        {
            int numberOfPayments = 12;
            DateTime firstPaymentDate = new DateTime(2022, 4, 15);
            DateTime maturityDate = new DateTime(2023, 4, 15);
            decimal loanAmount = 10000M;

            OperationResult<LoanAmortizationSchedule> result =
                LoanAmortizationSchedule.Create(LoanAgreementTestData.GetInstallments12MonthsOfValidInfo());

            InstallmentNumberValidationHandler handler = new(numberOfPayments);
            handler.SetNext(new InstallmentPaymentDateValidationHandler(firstPaymentDate, maturityDate))
                   .SetNext(new InstallmentPaymentAmountValidationHandler(loanAmount));

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
            var loanPymt = installments.Where(i => i.InstallmentNumber == 12).FirstOrDefault();
            loanPymt?.UpdateInstallmentNumber(InstallmentNumber.Create(13));

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
            var loanPymt = installments.Where(i => i.InstallmentNumber == 1).FirstOrDefault();
            loanPymt?.UpdateInstallmentNumber(InstallmentNumber.Create(13));

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
            var loanPymt = installments.Where(i => i.InstallmentNumber == 2).ToList();
            loanPymt.ForEach(i => i.UpdatePaymentDueDate(PaymentDueDate.Create(new DateTime(2022, 4, 15))));

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
            var loanPymt = installments.Where(i => i.InstallmentNumber == 12).FirstOrDefault();
            loanPymt?.UpdatePaymentDueDate(PaymentDueDate.Create(new DateTime(2023, 2, 14)));

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
            var loanPymt = installments.Where(i => i.InstallmentNumber == 1).FirstOrDefault();
            loanPymt?.UpdatePaymentDueDate(PaymentDueDate.Create(new DateTime(2022, 3, 1)));

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
            var loanPymt = installments.Where(i => i.InstallmentNumber == 12).FirstOrDefault();
            loanPymt?.UpdatePaymentDueDate(PaymentDueDate.Create(new DateTime(2023, 4, 16)));

            OperationResult<LoanAmortizationSchedule> result =
                LoanAmortizationSchedule.Create(installments);

            InstallmentPaymentDateValidationHandler handler = new(firstPaymentDate, maturityDate);

            Action action = () => handler.Handle(result.Result);

            var caughtException = Assert.Throws<InvalidOperationException>(action);

            Assert.Equal("Invalid installment payment dates(s) found! Valid installment payment dates are between loan agreement loan date and maturity date.", caughtException.Message);
        }

        [Fact]
        public void LoanInstallmentPaymentSchedule_Validation_ErrorInEMI_ShouldFail()
        {
            decimal loanAmount = 10000.00M;

            List<LoanInstallment> installments = LoanAgreementTestData.GetInstallments12MonthsOfValidInfo();
            var loanPymt = installments.Where(i => i.InstallmentNumber == 1).FirstOrDefault();
            loanPymt?.UpdateEqualMonthlyInstallment(EqualMonthlyInstallment.Create(860.00M));

            OperationResult<LoanAmortizationSchedule> result =
                LoanAmortizationSchedule.Create(installments);

            InstallmentPaymentAmountValidationHandler handler = new(loanAmount);
            Action action = () => handler.Handle(result.Result);

            var caughtException = Assert.Throws<InvalidOperationException>(action);
        }

        [Fact]
        public void LoanInstallmentPaymentSchedule_Validation_ErrorInPrincipal_ShouldFail()
        {
            decimal loanAmount = 10000.00M;

            List<LoanInstallment> installments = LoanAgreementTestData.GetInstallments12MonthsOfValidInfo();
            var loanPymt = installments.Where(i => i.InstallmentNumber == 1).FirstOrDefault();
            loanPymt?.UpdateLoanPrincipalAmount(LoanPrincipalAmount.Create(806.00M));

            OperationResult<LoanAmortizationSchedule> result =
                LoanAmortizationSchedule.Create(installments);

            InstallmentPaymentAmountValidationHandler handler = new(loanAmount);
            Action action = () => handler.Handle(result.Result);

            var caughtException = Assert.Throws<InvalidOperationException>(action);
        }

        [Fact]
        public void LoanInstallmentPaymentSchedule_Validation_ErrorInInterest_ShouldFail()
        {
            decimal loanAmount = 10000.00M;

            List<LoanInstallment> installments = LoanAgreementTestData.GetInstallments12MonthsOfValidInfo();
            var loanPymt = installments.Where(i => i.InstallmentNumber == 1).FirstOrDefault();
            loanPymt?.UpdateLoanInterestAmount(LoanInterestAmount.Create(57.00M));

            OperationResult<LoanAmortizationSchedule> result =
                LoanAmortizationSchedule.Create(installments);

            InstallmentPaymentAmountValidationHandler handler = new(loanAmount);
            Action action = () => handler.Handle(result.Result);

            var caughtException = Assert.Throws<InvalidOperationException>(action);
        }
    }
}