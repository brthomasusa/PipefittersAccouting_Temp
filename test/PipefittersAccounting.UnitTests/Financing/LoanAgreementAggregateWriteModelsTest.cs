using System;
using Xunit;
using FluentValidation.Results;
using FluentValidation.TestHelper;
using PipefittersAccounting.SharedModel.Validation.Financing;
using PipefittersAccounting.SharedModel.WriteModels.Financing;

namespace PipefittersAccounting.UnitTests.Financing
{
    public class LoanAgreementAggregateWriteModelsTest
    {
        [Fact]
        public void LoanInstallmentWriteModel_Should_not_have_error_when_ValidInfo_is_specified()
        {
            CreateLoanInstallmentInfo installmentInfo = LoanAgreementTestData.GetLoanInstallmentInfo();
            LoanInstallmentWriteModelValidator validator = new();

            ValidationResult result = validator.Validate(installmentInfo);

            Assert.True(result.IsValid);
        }

        [Fact]
        public void LoanInstallmentId_Should_have_error_when_DefaultGuid_is_specified()
        {
            CreateLoanInstallmentInfo installmentInfo = LoanAgreementTestData.GetLoanInstallmentInfo();
            installmentInfo.LoanInstallmentId = new Guid();
            LoanInstallmentWriteModelValidator validator = new();

            var result = validator.TestValidate(installmentInfo);
            result.ShouldHaveValidationErrorFor(installment => installment.LoanInstallmentId);
        }

        [Fact]
        public void LoanId_Should_have_error_when_DefaultGuid_is_specified()
        {
            CreateLoanInstallmentInfo installmentInfo = LoanAgreementTestData.GetLoanInstallmentInfo();
            installmentInfo.LoanId = new Guid();
            LoanInstallmentWriteModelValidator validator = new();

            var result = validator.TestValidate(installmentInfo);
            result.ShouldHaveValidationErrorFor(installment => installment.LoanId);
        }

        [Fact]
        public void InstallmentNumber_Should_have_error_when_value_equal_zero()
        {
            CreateLoanInstallmentInfo installmentInfo = LoanAgreementTestData.GetLoanInstallmentInfo();
            installmentInfo.InstallmentNumber = 0;
            LoanInstallmentWriteModelValidator validator = new();

            var result = validator.TestValidate(installmentInfo);
            result.ShouldHaveValidationErrorFor(installment => installment.InstallmentNumber);
        }

        [Fact]
        public void PaymentDueDate_Should_have_error_when_value_DefaultDate()
        {
            CreateLoanInstallmentInfo installmentInfo = LoanAgreementTestData.GetLoanInstallmentInfo();
            installmentInfo.PaymentDueDate = new DateTime();
            LoanInstallmentWriteModelValidator validator = new();

            var result = validator.TestValidate(installmentInfo);
            result.ShouldHaveValidationErrorFor(installment => installment.PaymentDueDate);
        }

        [Fact]
        public void EqualMonthlyInstallment_Should_have_error_when_value_NotGreaterThanZero()
        {
            CreateLoanInstallmentInfo installmentInfo = LoanAgreementTestData.GetLoanInstallmentInfo();
            installmentInfo.EqualMonthlyInstallment = 0;
            LoanInstallmentWriteModelValidator validator = new();

            var result = validator.TestValidate(installmentInfo);
            result.ShouldHaveValidationErrorFor(installment => installment.EqualMonthlyInstallment);
        }

        [Fact]
        public void LoanPrincipalAmount_Should_have_error_when_value_NotGreaterThanZero()
        {
            CreateLoanInstallmentInfo installmentInfo = LoanAgreementTestData.GetLoanInstallmentInfo();
            installmentInfo.LoanPrincipalAmount = 0;
            LoanInstallmentWriteModelValidator validator = new();

            var result = validator.TestValidate(installmentInfo);
            result.ShouldHaveValidationErrorFor(installment => installment.LoanPrincipalAmount);
        }

        [Fact]
        public void LoanPrincipalAmount_Should_have_error_when_value_Over_100000()
        {
            CreateLoanInstallmentInfo installmentInfo = LoanAgreementTestData.GetLoanInstallmentInfo();
            installmentInfo.LoanPrincipalAmount = 100000.01M;
            LoanInstallmentWriteModelValidator validator = new();

            var result = validator.TestValidate(installmentInfo);
            result.ShouldHaveValidationErrorFor(installment => installment.LoanPrincipalAmount);
        }

        [Fact]
        public void LoanInterestAmount_Should_have_error_when_value_IsNegative()
        {
            CreateLoanInstallmentInfo installmentInfo = LoanAgreementTestData.GetLoanInstallmentInfo();
            installmentInfo.LoanInterestAmount = -1M;
            LoanInstallmentWriteModelValidator validator = new();

            var result = validator.TestValidate(installmentInfo);
            result.ShouldHaveValidationErrorFor(installment => installment.LoanInterestAmount);
        }

        [Fact]
        public void LoanPrincipalRemaining_Should_have_error_when_value_IsNegative()
        {
            CreateLoanInstallmentInfo installmentInfo = LoanAgreementTestData.GetLoanInstallmentInfo();
            installmentInfo.LoanPrincipalRemaining = -1M;
            LoanInstallmentWriteModelValidator validator = new();

            var result = validator.TestValidate(installmentInfo);
            result.ShouldHaveValidationErrorFor(installment => installment.LoanPrincipalRemaining);
        }

        [Fact]
        public void LoanPrincipalRemaining_Should_have_error_when_value_Over_115000()
        {
            CreateLoanInstallmentInfo installmentInfo = LoanAgreementTestData.GetLoanInstallmentInfo();
            installmentInfo.LoanPrincipalRemaining = 115000.01M;
            LoanInstallmentWriteModelValidator validator = new();

            var result = validator.TestValidate(installmentInfo);
            result.ShouldHaveValidationErrorFor(installment => installment.LoanPrincipalRemaining);
        }


        [Fact]
        public void EMI_Principal_Interest_Should_have_error_when_InValidEMI_is_specified()
        {
            CreateLoanInstallmentInfo installmentInfo = LoanAgreementTestData.GetLoanInstallmentInfo();
            installmentInfo.EqualMonthlyInstallment = 865.20M;          // Off by a six cents
            LoanInstallmentWriteModelValidator validator = new();

            ValidationResult result = validator.Validate(installmentInfo);

            Assert.False(result.IsValid);
        }

        [Fact]
        public void EMI_Principal_Interest_Should_have_error_when_InValidPrincipal_is_specified()
        {
            CreateLoanInstallmentInfo installmentInfo = LoanAgreementTestData.GetLoanInstallmentInfo();
            installmentInfo.LoanPrincipalAmount = 831.01M;              // Off by a penny
            LoanInstallmentWriteModelValidator validator = new();

            ValidationResult result = validator.Validate(installmentInfo);

            Assert.False(result.IsValid);
        }

        [Fact]
        public void EMI_Principal_Interest_Should_have_error_when_InValidInterest_is_specified()
        {
            CreateLoanInstallmentInfo installmentInfo = LoanAgreementTestData.GetLoanInstallmentInfo();
            installmentInfo.LoanInterestAmount = 34.27M;              // Off by a penny
            LoanInstallmentWriteModelValidator validator = new();

            ValidationResult result = validator.Validate(installmentInfo);

            Assert.False(result.IsValid);
        }

        /*   LoanAgreement write model validation tests   */

        [Fact]
        public void LoanAgreementWriteModel_Should_not_have_error_when_ValidInfo_is_specified()
        {
            CreateLoanAgreementInfo agreementInfo = LoanAgreementTestData.GetCreateLoanAgreementInfo();
            LoanAgreementWriteModelValidator validator = new();

            ValidationResult result = validator.Validate(agreementInfo);

            Assert.True(result.IsValid);
        }

    }
}