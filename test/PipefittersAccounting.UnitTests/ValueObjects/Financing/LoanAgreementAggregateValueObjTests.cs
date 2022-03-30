#pragma warning disable CS8600
#pragma warning disable CS8604
#pragma warning disable CS8625

using System;
using Xunit;
using PipefittersAccounting.Core.Financing.LoanAgreementAggregate;
using PipefittersAccounting.Core.Financing.LoanAgreementAggregate.ValueObjects;

namespace PipefittersAccounting.UnitTests.ValueObjects.Financing
{
    public class LoanAgreementAggregateValueObjTests
    {
        [Theory]
        [InlineData(1, 1)]
        [InlineData(60, 60)]
        public void Create_InstallmentNumber_ValidInt_ShouldSucceed(int input, int expectedResult)
        {
            InstallmentNumber actualResult = InstallmentNumber.Create(input);
            Assert.IsType<InstallmentNumber>(actualResult);
            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void Create_InstallmentNumber_NegativeInstallments_ShouldFail()
        {
            Action action = () => InstallmentNumber.Create(-1);

            var caughtException = Assert.Throws<ArgumentOutOfRangeException>(action);

            Assert.Equal("The installment number must be greater than or equal to one.", caughtException.ParamName);
        }

        [Fact]
        public void Create_InstallmentNumber_ZeroInstallments_ShouldFail()
        {
            Action action = () => InstallmentNumber.Create(0);

            var caughtException = Assert.Throws<ArgumentOutOfRangeException>(action);

            Assert.Equal("The installment number must be greater than or equal to one.", caughtException.ParamName);
        }

        [Theory]
        [InlineData(.0001, .0001)]
        [InlineData(.9999, .9999)]
        public void Create_InterestRate_ValidInfo_ShouldSucceed(decimal input, decimal expectedResult)
        {
            InterestRate rate = InterestRate.Create(input);

            Assert.IsType<InterestRate>(rate);
            Assert.Equal(expectedResult, rate);
        }

        [Fact]
        public void Create_InterestRate_NegativePercent_ShouldFail()
        {
            Action action = () => InterestRate.Create(-.00001M);

            var caughtException = Assert.Throws<ArgumentException>(action);

            Assert.Equal("The interest rate can not be negative.", caughtException.Message);
        }

        [Fact]
        public void Create_InterestRate_GreaterThan100Percent_ShouldFail()
        {
            Action action = () => InterestRate.Create(1.0000M);

            var caughtException = Assert.Throws<ArgumentException>(action);

            Assert.Equal("An interest rate of 100% is not allowed.", caughtException.Message);
        }

        [Fact]
        public void Create_LoanDate_ValidInfo_ShouldSucceed()
        {
            DateTime testDate = new(2022, 3, 19);
            LoanDate loanDate = LoanDate.Create(testDate);

            Assert.IsType<LoanDate>(loanDate);
            Assert.Equal(testDate, loanDate);
        }

        [Fact]
        public void Create_LoanDate_DefaultDate_ShouldFail()
        {
            Action action = () => LoanDate.Create(new DateTime());

            var caughtException = Assert.Throws<ArgumentNullException>(action);

            Assert.Equal("The loan date is required.", caughtException.ParamName);
        }

        [Fact]
        public void Create_LoanInterestAmount_ValidInfo_ShouldSucceed()
        {
            LoanInterestAmount loanInterestAmount = LoanInterestAmount.Create(320M);

            Assert.IsType<LoanInterestAmount>(loanInterestAmount);
            Assert.Equal(320M, loanInterestAmount);
        }

        [Fact]
        public void Create_LoanInterestAmount_NegativeAmt_ShouldFail()
        {
            Action action = () => LoanInterestAmount.Create(-1M);

            var caughtException = Assert.Throws<ArgumentException>(action);

            Assert.Equal("The loan interest amount can not be negative.", caughtException.Message);
        }

        [Fact]
        public void Create_LoanPrincipalAmount_ValidInfo_ShouldSucceed()
        {
            LoanPrincipalAmount loanPrincipalAmount = LoanPrincipalAmount.Create(320M);

            Assert.IsType<LoanPrincipalAmount>(loanPrincipalAmount);
            Assert.Equal(320M, loanPrincipalAmount);
        }

        [Fact]
        public void Create_LoanPrincipalAmount_NegativeAmt_ShouldFail()
        {
            Action action = () => LoanPrincipalAmount.Create(-1M);

            var caughtException = Assert.Throws<ArgumentException>(action);

            Assert.Equal("The principal amount of the loan installment can not negative.", caughtException.Message);
        }

        [Fact]
        public void Create_MaturityDate_ValidInfo_ShouldSucceed()
        {
            DateTime testDate = new(2023, 3, 19);
            MaturityDate maturityDate = MaturityDate.Create(testDate);

            Assert.IsType<MaturityDate>(maturityDate);
            Assert.Equal(testDate, maturityDate);
        }

        [Fact]
        public void Create_MaturityDate_DefaultDate_ShouldFail()
        {
            Action action = () => MaturityDate.Create(new DateTime());

            var caughtException = Assert.Throws<ArgumentNullException>(action);

            Assert.Equal("The loan maturity date is required.", caughtException.ParamName);
        }

        [Fact]
        public void Create_PaymentDueDate_ValidInfo_ShouldSucceed()
        {
            DateTime testDate = new(2023, 3, 19);
            PaymentDueDate pymtDueDate = PaymentDueDate.Create(testDate);

            Assert.IsType<PaymentDueDate>(pymtDueDate);
            Assert.Equal(testDate, pymtDueDate);
        }

        [Fact]
        public void Create_PaymentDueDate_DefaultDate_ShouldFail()
        {
            Action action = () => PaymentDueDate.Create(new DateTime());

            var caughtException = Assert.Throws<ArgumentNullException>(action);

            Assert.Equal("The payment due date is required.", caughtException.ParamName);
        }

        [Theory]
        [InlineData(1, 1)]
        [InlineData(12, 12)]
        public void Create_PaymentsPerYear_ValidInfo_ShouldSucceed(int input, int expectedResult)
        {
            NumberOfInstallments actualResult = NumberOfInstallments.Create(input);
            Assert.IsType<NumberOfInstallments>(actualResult);
            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void Create_PaymentsPerYear_NegativePymtsPerYear_ShouldFail()
        {
            Action action = () => NumberOfInstallments.Create(-1);

            var caughtException = Assert.Throws<ArgumentOutOfRangeException>(action);

            Assert.Equal("The number of installments must be greater than 1.", caughtException.ParamName);
        }
    }
}