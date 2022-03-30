using System;
using System.Collections.Generic;
using Xunit;
using PipefittersAccounting.Core.Financing.LoanAgreementAggregate.Components;

namespace PipefittersAccounting.UnitTests.Financing
{
    public class LoanRepaymentScheduleValidationTests
    {
        [Fact]
        public void LoanInstallmentPaymentSchedule_Validation_ValidInfo_Chained_ShouldSucceed()
        {
            int numberOfPayments = 4;
            DateTime firstPaymentDate = new DateTime(2022, 4, 15);
            DateTime maturityDate = firstPaymentDate.AddMonths(3);
            decimal loanAmount = 1800M;
            decimal interestRate = .06M;

            LoanInstallmentPaymentSchedule schedule = new(LoanAgreementTestData.GetInstallmentsValidInfo());

            InstallmentNumberValidationHandler handler = new(numberOfPayments);
            handler.SetNext(new InstallmentPaymentDateValidationHandler(firstPaymentDate, maturityDate))
                   .SetNext(new InstallmentPaymentAmountValidationHandler(loanAmount, interestRate));

            var exception = Record.Exception(() => handler.Handle(schedule));

            Assert.Null(exception);
        }

        [Fact]
        public void LoanInstallmentPaymentSchedule_Validation_InstallmentNumberTooHigh_ShouldFail()
        {
            int numberOfPayments = 4;
            DateTime firstPaymentDate = new DateTime(2022, 4, 15);
            DateTime maturityDate = firstPaymentDate.AddMonths(3);

            SortedDictionary<int, Installment> badData = LoanAgreementTestData.GetInstallmentsValidInfo();
            badData[3] = new Installment(InstallmentNumber: 5,                           // Invalid installment number
                            PaymentDueDate: new DateTime(2022, 6, 15),
                            Payment: 455.65M,
                            Principal: 451.00M,
                            Interest: 4.65M,
                            TotalInterestPaid: 19.95M,
                            RemainingBalance: 453.00M);

            LoanInstallmentPaymentSchedule schedule = new(badData);

            InstallmentNumberValidationHandler handler = new(numberOfPayments);

            Action action = () => handler.Handle(schedule);

            var caughtException = Assert.Throws<InvalidOperationException>(action);
        }

        [Fact]
        public void LoanInstallmentPaymentSchedule_Validation_InstallmentNumberTooLow_ShouldFail()
        {
            int numberOfPayments = 4;
            DateTime firstPaymentDate = new DateTime(2022, 4, 15);
            DateTime maturityDate = firstPaymentDate.AddMonths(3);

            SortedDictionary<int, Installment> badData = LoanAgreementTestData.GetInstallmentsValidInfo();
            badData[1] = new Installment(InstallmentNumber: 0,                           // Invalid installment number
                            PaymentDueDate: new DateTime(2022, 6, 15),
                            Payment: 455.65M,
                            Principal: 451.00M,
                            Interest: 4.65M,
                            TotalInterestPaid: 19.95M,
                            RemainingBalance: 453.00M);

            LoanInstallmentPaymentSchedule schedule = new(badData);

            InstallmentNumberValidationHandler handler = new(numberOfPayments);

            Action action = () => handler.Handle(schedule);

            var caughtException = Assert.Throws<InvalidOperationException>(action);
        }

        [Fact]
        public void LoanInstallmentPaymentSchedule_Validation_TooFewInstallments_ShouldFail()
        {
            int numberOfPayments = 4;
            DateTime firstPaymentDate = new DateTime(2022, 4, 15);
            DateTime maturityDate = firstPaymentDate.AddMonths(3);

            SortedDictionary<int, Installment> badData = LoanAgreementTestData.GetInstallmentsTooFewInstallments();
            LoanInstallmentPaymentSchedule schedule = new(badData);

            InstallmentNumberValidationHandler handler = new(numberOfPayments);

            Action action = () => handler.Handle(schedule);

            var caughtException = Assert.Throws<InvalidOperationException>(action);

            Assert.Equal($"The repayment schedule only contains {badData.Count} installments. It should contain {numberOfPayments}", caughtException.Message);
        }

        [Fact]
        public void LoanInstallmentPaymentSchedule_Validation_TooManyInstallments_ShouldFail()
        {
            int numberOfPayments = 4;
            DateTime firstPaymentDate = new DateTime(2022, 4, 15);
            DateTime maturityDate = firstPaymentDate.AddMonths(3);

            SortedDictionary<int, Installment> badData = LoanAgreementTestData.GetInstallmentsTooManyInstallments();
            LoanInstallmentPaymentSchedule schedule = new(badData);

            InstallmentNumberValidationHandler handler = new(numberOfPayments);

            Action action = () => handler.Handle(schedule);

            var caughtException = Assert.Throws<InvalidOperationException>(action);

            Assert.Equal($"The repayment schedule contains too many ({badData.Count}) installments. It should contain {numberOfPayments}", caughtException.Message);
        }

        [Fact]
        public void LoanInstallmentPaymentSchedule_Validation_DictIndexDoesNotStartAtOne_ShouldFail()
        {
            int numberOfPayments = 4;
            DateTime firstPaymentDate = new DateTime(2022, 4, 15);
            DateTime maturityDate = firstPaymentDate.AddMonths(3);

            SortedDictionary<int, Installment> badData = LoanAgreementTestData.GetInstallmentsDictIndexDoesNotStartAtOne();
            LoanInstallmentPaymentSchedule schedule = new(badData);

            InstallmentNumberValidationHandler handler = new(numberOfPayments);

            Action action = () => handler.Handle(schedule);

            var caughtException = Assert.Throws<InvalidOperationException>(action);

            Assert.Equal("The numbering of the dictionary is out of order.", caughtException.Message);
        }

        [Fact]
        public void LoanInstallmentPaymentSchedule_Validation_DuplicateOrOutOfOrderInstallmentNumbers_ShouldFail()
        {
            int numberOfPayments = 4;
            DateTime firstPaymentDate = new DateTime(2022, 4, 15);
            DateTime maturityDate = firstPaymentDate.AddMonths(3);

            SortedDictionary<int, Installment> badData = LoanAgreementTestData.GetInstallmentsValidInfo();
            badData[3] = new Installment(InstallmentNumber: 2,               // Duplicate or out of order installment numbers
                                PaymentDueDate: new DateTime(2022, 5, 15),
                                Payment: 455.65M,
                                Principal: 449.00M,
                                Interest: 6.65M,
                                TotalInterestPaid: 15.30M,
                                RemainingBalance: 904.00M);

            LoanInstallmentPaymentSchedule schedule = new(badData);

            InstallmentNumberValidationHandler handler = new(numberOfPayments);

            Action action = () => handler.Handle(schedule);

            var caughtException = Assert.Throws<InvalidOperationException>(action);
        }

        [Fact]
        public void LoanInstallmentPaymentSchedule_Validation_2ndPymtDueDateNotGreaterThanPrevDueDate_ShouldFail()
        {
            DateTime firstPaymentDate = new DateTime(2022, 4, 15);
            DateTime maturityDate = firstPaymentDate.AddMonths(3);

            SortedDictionary<int, Installment> badData = LoanAgreementTestData.GetInstallmentsValidInfo();
            badData[2] = new Installment(InstallmentNumber: 2,
                                PaymentDueDate: new DateTime(2022, 4, 15),  // Equal to first payment date, it must be greater than 2022-04-15
                                Payment: 455.65M,
                                Principal: 449.00M,
                                Interest: 6.65M,
                                TotalInterestPaid: 15.30M,
                                RemainingBalance: 904.00M);

            LoanInstallmentPaymentSchedule schedule = new(badData);

            InstallmentPaymentDateValidationHandler handler = new(firstPaymentDate, maturityDate);

            Action action = () => handler.Handle(schedule);

            var caughtException = Assert.Throws<InvalidOperationException>(action);

            Assert.Equal("The payment due date for installment 2 can not be less than or equal the payment date for installment 1.", caughtException.Message);
        }

        [Fact]
        public void LoanInstallmentPaymentSchedule_Validation_LastPymtDueDateNotGreaterThanPrevDueDate_ShouldFail()
        {
            DateTime firstPaymentDate = new DateTime(2022, 4, 15);
            DateTime maturityDate = firstPaymentDate.AddMonths(3);

            SortedDictionary<int, Installment> badData = LoanAgreementTestData.GetInstallmentsValidInfo();
            badData[4] = new Installment(InstallmentNumber: 4,
                                PaymentDueDate: new DateTime(2022, 6, 15),
                                Payment: 455.65M,
                                Principal: 453.00M,
                                Interest: 2.65M,
                                TotalInterestPaid: 22.60M,
                                RemainingBalance: 0M);

            LoanInstallmentPaymentSchedule schedule = new(badData);

            InstallmentPaymentDateValidationHandler handler = new(firstPaymentDate, maturityDate);

            Action action = () => handler.Handle(schedule);

            var caughtException = Assert.Throws<InvalidOperationException>(action);

            Assert.Equal("The payment due date for installment 4 can not be less than or equal the payment date for installment 3.", caughtException.Message);
        }

        [Fact]
        public void LoanInstallmentPaymentSchedule_Validation_PymtDueDateBeforeLoanAgreementLoanDate_ShouldFail()
        {
            DateTime firstPaymentDate = new DateTime(2022, 4, 15);
            DateTime maturityDate = firstPaymentDate.AddMonths(3);

            SortedDictionary<int, Installment> badData = LoanAgreementTestData.GetInstallmentsValidInfo();
            badData[1] = new Installment(InstallmentNumber: 1,
                                PaymentDueDate: new DateTime(2022, 4, 14),
                                Payment: 455.65M,
                                Principal: 447.00M,
                                Interest: 8.65M,
                                TotalInterestPaid: 8.65M,
                                RemainingBalance: 1353.00M);

            LoanInstallmentPaymentSchedule schedule = new(badData);

            InstallmentPaymentDateValidationHandler handler = new(firstPaymentDate, maturityDate);

            Action action = () => handler.Handle(schedule);

            var caughtException = Assert.Throws<InvalidOperationException>(action);

            Assert.Equal("Invalid installment payment dates(s) found! Valid installment payment dates are between loan agreement loan date and maturity date.", caughtException.Message);
        }

        [Fact]
        public void LoanInstallmentPaymentSchedule_Validation_PymtDueDateAfterLoanAgreementMaturityDate_ShouldFail()
        {
            DateTime firstPaymentDate = new DateTime(2022, 4, 15);
            DateTime maturityDate = firstPaymentDate.AddMonths(3);

            SortedDictionary<int, Installment> badData = LoanAgreementTestData.GetInstallmentsValidInfo();
            badData[1] = new Installment(InstallmentNumber: 1,
                                PaymentDueDate: new DateTime(2022, 7, 16),
                                Payment: 455.65M,
                                Principal: 447.00M,
                                Interest: 8.65M,
                                TotalInterestPaid: 8.65M,
                                RemainingBalance: 1353.00M);

            LoanInstallmentPaymentSchedule schedule = new(badData);

            InstallmentPaymentDateValidationHandler handler = new(firstPaymentDate, maturityDate);

            Action action = () => handler.Handle(schedule);

            var caughtException = Assert.Throws<InvalidOperationException>(action);

            Assert.Equal("Invalid installment payment dates(s) found! Valid installment payment dates are between loan agreement loan date and maturity date.", caughtException.Message);
        }

        [Fact]
        public void LoanInstallmentPaymentSchedule_Validation_PymtDueDateSetToDefaultDate_ShouldFail()
        {
            DateTime firstPaymentDate = new DateTime(2022, 4, 15);
            DateTime maturityDate = firstPaymentDate.AddMonths(3);

            SortedDictionary<int, Installment> badData = LoanAgreementTestData.GetInstallmentsValidInfo();
            badData[4] = new Installment(InstallmentNumber: 4,
                                PaymentDueDate: new DateTime(),
                                Payment: 455.65M,
                                Principal: 453.00M,
                                Interest: 2.65M,
                                TotalInterestPaid: 22.60M,
                                RemainingBalance: 0M);

            LoanInstallmentPaymentSchedule schedule = new(badData);

            InstallmentPaymentDateValidationHandler handler = new(firstPaymentDate, maturityDate);

            Action action = () => handler.Handle(schedule);

            var caughtException = Assert.Throws<InvalidOperationException>(action);

            Assert.Equal("One or more payment dates were found to have a default date value, please correct with an actual date value.", caughtException.Message);
        }
    }
}