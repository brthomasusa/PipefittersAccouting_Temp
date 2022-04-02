#pragma warning disable CS8600
#pragma warning disable CS8604
#pragma warning disable CS8625

using System;
using Xunit;
using PipefittersAccounting.SharedKernel.CommonValueObjects;
using PipefittersAccounting.Core.Financing.LoanAgreementAggregate;
using PipefittersAccounting.Core.Financing.LoanAgreementAggregate.ValueObjects;

namespace PipefittersAccounting.UnitTests.Financing
{
    public class LoanAgreementAggregateTests
    {
        // [Fact]
        // public void Create_LoanAgreement_ValidInfo_ShouldSucceed()
        // {
        //     LoanAgreement agreement = new
        //     (
        //         LoanAgreementEconEvent.Create(EntityGuidID.Create(Guid.NewGuid())),
        //         EntityGuidID.Create(new Guid("bf19cf34-f6ba-4fb2-b70e-ab19d3371886")),
        //         LoanAmount.Create(17500),
        //         InterestRate.Create(.0675M),
        //         LoanDate.Create(new DateTime(2022, 3, 30)),
        //         MaturityDate.Create(new DateTime(2023, 3, 30)),
        //         NumberOfInstallments.Create(12),
        //         EntityGuidID.Create(new Guid("660bb318-649e-470d-9d2b-693bfb0b2744")),
        //         LoanAgreementTestData.GetInstallments12Months()
        //     );

        //     Assert.IsType<LoanAgreement>(agreement);
        // }

        // [Fact]
        // public void UpdateLoanInstallmentPaymentSchedule_LoanAgreement_ValidInfo_ShouldSucceed()
        // {
        //     LoanAgreement agreement = LoanAgreementTestData.GetLoanAgreementForEditing();
        //     agreement.UpdateLoanInstallmentPaymentSchedule(LoanAgreementTestData.GetInstallmentsLoanRepymtScheduleUpdate());

        //     var exception = Record.Exception(
        //         () => agreement.UpdateLoanInstallmentPaymentSchedule(LoanAgreementTestData.GetInstallmentsLoanRepymtScheduleUpdate())
        //     );

        //     Assert.Null(exception);
        // }

        // [Fact]
        // public void UpdateLoanAmount_LoanAgreement_ValidInfo_ShouldSucceed()
        // {
        //     LoanAgreement agreement = LoanAgreementTestData.GetLoanAgreementForEditing();

        //     agreement.UpdateLoanAmount(LoanAmount.Create(45000M));

        //     Assert.Equal(45000M, agreement.LoanAmount);
        // }

        // [Fact]
        // public void UpdateLoanAmount_LoanAgreement_NullLoanAmount_ShouldFail()
        // {
        //     LoanAgreement agreement = LoanAgreementTestData.GetLoanAgreementForEditing();

        //     Action action = () => agreement.UpdateLoanAmount(null);

        //     var caughtException = Assert.Throws<ArgumentNullException>(action);

        //     Assert.Equal("The loan amount for this loan agreement is required.", caughtException.ParamName);
        // }

        // [Fact]
        // public void UpdateInterestRate_LoanAgreement_ValidInfo_ShouldSucceed()
        // {
        //     LoanAgreement agreement = LoanAgreementTestData.GetLoanAgreementForEditing();

        //     agreement.UpdateInterestRate(InterestRate.Create(.025M));
        //     Assert.Equal(.025M, agreement.InterestRate);
        // }

        // [Fact]
        // public void UpdateLoanDate_LoanAgreement_ValidInfo_ShouldSucceed()
        // {
        //     LoanAgreement agreement = LoanAgreementTestData.GetLoanAgreementForEditing();

        //     DateTime newLoanDate = new DateTime(2022, 3, 1);
        //     agreement.UpdateLoanDate(LoanDate.Create(newLoanDate));
        //     Assert.Equal(newLoanDate, agreement.LoanDate);
        // }

        // [Fact]
        // public void UpdateMaturityDate_LoanAgreement_ValidInfo_ShouldSucceed()
        // {
        //     LoanAgreement agreement = LoanAgreementTestData.GetLoanAgreementForEditing();

        //     DateTime maturityDate = new DateTime(2023, 3, 1);
        //     agreement.UpdateMaturityDate(MaturityDate.Create(maturityDate));
        //     Assert.Equal(maturityDate, agreement.MaturityDate);
        // }

        // [Fact]
        // public void UpdatePaymentsPerYear_LoanAgreement_ValidInfo_ShouldSucceed()
        // {
        //     LoanAgreement agreement = LoanAgreementTestData.GetLoanAgreementForEditing();
        //     agreement.UpdateNumberOfInstallments(NumberOfInstallments.Create(24));

        //     Assert.Equal(24, agreement.NumberOfInstallments);
        // }

        // [Fact]
        // public void UpdateUserId_LoanAgreement_ValidInfo_ShouldSucceed()
        // {
        //     LoanAgreement agreement = LoanAgreementTestData.GetLoanAgreementForEditing();
        //     Guid uID = EntityGuidID.Create(new Guid("94b1d516-a1c3-4df8-ae85-be1f34966601"));

        //     agreement.UpdateUserId(EntityGuidID.Create(uID));
        //     Assert.Equal(uID, agreement.UserId);
        // }

        // [Fact]
        // public void Create_LoanInstallment_ValidInfo_ShouldSucceed()
        // {
        //     LoanInstallment installment = new
        //     (
        //         LoanPaymentEconEvent.Create(EntityGuidID.Create(Guid.NewGuid())),
        //         EntityGuidID.Create(new Guid("41ca2b0a-0ed5-478b-9109-5dfda5b2eba1")),
        //         InstallmentNumber.Create(1),
        //         PaymentDueDate.Create(new DateTime(2022, 4, 1)),
        //         LoanPrincipalAmount.Create(800M),
        //         LoanInterestAmount.Create(71.25M),
        //         LoanPrincipalRemaining.Create(32000M),
        //         false,
        //         EntityGuidID.Create(new Guid("660bb318-649e-470d-9d2b-693bfb0b2744"))
        //     );
        // }

        // [Fact]
        // public void UpdateInstallmentNumber_LoanInstallment_ValidInfo_ShouldSucceed()
        // {
        //     LoanInstallment installment = LoanAgreementTestData.GetLoanInstallmentForEditing();
        //     installment.UpdateInstallmentNumber(InstallmentNumber.Create(2));

        //     Assert.Equal(2, installment.InstallmentNumber);
        // }

        // [Fact]
        // public void UpdatePaymentDueDate_LoanInstallment_ValidInfo_ShouldSucceed()
        // {
        //     DateTime dueDate = new(2022, 5, 2);
        //     LoanInstallment installment = LoanAgreementTestData.GetLoanInstallmentForEditing();
        //     installment.UpdatePaymentDueDate(PaymentDueDate.Create(dueDate));

        //     Assert.Equal(dueDate, installment.PaymentDueDate);
        // }

        // [Fact]
        // public void UpdateLoanPrincipalAmount_LoanInstallment_ValidInfo_ShouldSucceed()
        // {
        //     LoanInstallment installment = LoanAgreementTestData.GetLoanInstallmentForEditing();
        //     installment.UpdateLoanPrincipalAmount(LoanPrincipalAmount.Create(500M));

        //     Assert.Equal(500M, installment.LoanPrincipalAmount);
        // }

        // [Fact]
        // public void UpdateLoanInterestAmount_LoanInstallment_ValidInfo_ShouldSucceed()
        // {
        //     LoanInstallment installment = LoanAgreementTestData.GetLoanInstallmentForEditing();
        //     installment.UpdateLoanInterestAmount(LoanInterestAmount.Create(50M));

        //     Assert.Equal(50M, installment.LoanInterestAmount);
        // }

        // [Fact]
        // public void UpdateLoanPrincipalRemaining_LoanInstallment_ValidInfo_ShouldSucceed()
        // {
        //     LoanInstallment installment = LoanAgreementTestData.GetLoanInstallmentForEditing();
        //     installment.UpdateLoanPrincipalRemaining(LoanPrincipalRemaining.Create(50000M));

        //     Assert.Equal(50000M, installment.LoanPrincipalRemaining);
        // }

        // [Fact]
        // public void UpdatePaymentStatus_LoanInstallment_ValidInfo_ShouldSucceed()
        // {
        //     LoanInstallment installment = LoanAgreementTestData.GetLoanInstallmentForEditing();
        //     installment.UpdatePaymentStatus(true);

        //     Assert.True(installment.IsPaid);
        // }

        // [Fact]
        // public void UpdateUserId_LoanInstallment_ValidInfo_ShouldSucceed()
        // {
        //     LoanAgreement agreement = LoanAgreementTestData.GetLoanAgreementForEditing();
        //     Guid uID = EntityGuidID.Create(new Guid("94b1d516-a1c3-4df8-ae85-be1f34966601"));

        //     agreement.UpdateUserId(EntityGuidID.Create(uID));
        //     Assert.Equal(uID, agreement.UserId);
        // }
    }
}