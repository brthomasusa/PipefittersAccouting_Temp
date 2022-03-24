#pragma warning disable CS8600
#pragma warning disable CS8604
#pragma warning disable CS8625

using System;
using Xunit;
using PipefittersAccounting.SharedKernel;
using PipefittersAccounting.SharedKernel.CommonValueObjects;
using PipefittersAccounting.Core.Financing.LoanAgreementAggregate;

namespace PipefittersAccounting.UnitTests.ValueObjects.Financing
{
    public class LoanAgreementAggregateTests
    {
        [Fact]
        public void Create_LoanAgreement_ValidInfo_ShouldSucceed()
        {
            LoanAgreement agreement = new
            (
                LoanAgreementEconEvent.Create(EntityGuidID.Create(Guid.NewGuid())),
                EntityGuidID.Create(new Guid("bf19cf34-f6ba-4fb2-b70e-ab19d3371886")),
                LoanAmount.Create(175000),
                InterestRate.Create(.0675),
                LoanDate.Create(new DateTime(2021, 11, 5)),
                MaturityDate.Create(new DateTime(2022, 11, 5)),
                PaymentsPerYear.Create(12),
                EntityGuidID.Create(new Guid("660bb318-649e-470d-9d2b-693bfb0b2744"))
            );

            Assert.IsType<LoanAgreement>(agreement);
        }

        [Fact]
        public void UpdateLoanAmount_LoanAgreement_ValidInfo_ShouldSucceed()
        {
            LoanAgreement agreement = GetLoanAgreementForEditing();

            agreement.UpdateLoanAmount(LoanAmount.Create(45000M));

            Assert.Equal(45000M, agreement.LoanAmount);
        }

        [Fact]
        public void UpdateLoanAmount_LoanAgreement_NullLoanAmount_ShouldFail()
        {
            LoanAgreement agreement = GetLoanAgreementForEditing();

            Action action = () => agreement.UpdateLoanAmount(null);

            var caughtException = Assert.Throws<ArgumentNullException>(action);

            Assert.Equal("The loan amount for this loan agreement is required.", caughtException.ParamName);
        }

        [Fact]
        public void UpdateInterestRate_LoanAgreement_ValidInfo_ShouldSucceed()
        {
            LoanAgreement agreement = GetLoanAgreementForEditing();

            agreement.UpdateInterestRate(InterestRate.Create(.025));
            Assert.Equal(.025, agreement.InterestRate);
        }

        [Fact]
        public void UpdateLoanDate_LoanAgreement_ValidInfo_ShouldSucceed()
        {
            LoanAgreement agreement = GetLoanAgreementForEditing();

            DateTime newLoanDate = new DateTime(2022, 3, 1);
            agreement.UpdateLoanDate(LoanDate.Create(newLoanDate));
            Assert.Equal(newLoanDate, agreement.LoanDate);
        }

        [Fact]
        public void UpdateMaturityDate_LoanAgreement_ValidInfo_ShouldSucceed()
        {
            LoanAgreement agreement = GetLoanAgreementForEditing();

            DateTime maturityDate = new DateTime(2023, 3, 1);
            agreement.UpdateMaturityDate(MaturityDate.Create(maturityDate));
            Assert.Equal(maturityDate, agreement.MaturityDate);
        }

        [Fact]
        public void UpdatePaymentsPerYear_LoanAgreement_ValidInfo_ShouldSucceed()
        {
            LoanAgreement agreement = GetLoanAgreementForEditing();
            agreement.UpdatePaymentsPerYear(PaymentsPerYear.Create(24));

            Assert.Equal(24, agreement.PaymentsPerYear);
        }

        [Fact]
        public void UpdateUserId_LoanAgreement_ValidInfo_ShouldSucceed()
        {
            LoanAgreement agreement = GetLoanAgreementForEditing();
            Guid uID = EntityGuidID.Create(new Guid("94b1d516-a1c3-4df8-ae85-be1f34966601"));

            agreement.UpdateUserId(EntityGuidID.Create(uID));
            Assert.Equal(uID, agreement.UserId);
        }

        [Fact]
        public void Create_LoanInstallment_ValidInfo_ShouldSucceed()
        {
            LoanInstallment installment = new
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

        [Fact]
        public void UpdateInstallmentNumber_LoanInstallment_ValidInfo_ShouldSucceed()
        {
            LoanInstallment installment = GetLoanInstallmentForEditing();
            installment.UpdateInstallmentNumber(InstallmentNumber.Create(2));

            Assert.Equal(2, installment.InstallmentNumber);
        }

        [Fact]
        public void UpdatePaymentDueDate_LoanInstallment_ValidInfo_ShouldSucceed()
        {
            DateTime dueDate = new(2022, 5, 2);
            LoanInstallment installment = GetLoanInstallmentForEditing();
            installment.UpdatePaymentDueDate(PaymentDueDate.Create(dueDate));

            Assert.Equal(dueDate, installment.PaymentDueDate);
        }

        [Fact]
        public void UpdateLoanPrincipalAmount_LoanInstallment_ValidInfo_ShouldSucceed()
        {
            LoanInstallment installment = GetLoanInstallmentForEditing();
            installment.UpdateLoanPrincipalAmount(LoanPrincipalAmount.Create(500M));

            Assert.Equal(500M, installment.LoanPrincipalAmount);
        }

        [Fact]
        public void UpdateLoanInterestAmount_LoanInstallment_ValidInfo_ShouldSucceed()
        {
            LoanInstallment installment = GetLoanInstallmentForEditing();
            installment.UpdateLoanInterestAmount(LoanInterestAmount.Create(50M));

            Assert.Equal(50M, installment.LoanInterestAmount);
        }

        [Fact]
        public void UpdateLoanPrincipalRemaining_LoanInstallment_ValidInfo_ShouldSucceed()
        {
            LoanInstallment installment = GetLoanInstallmentForEditing();
            installment.UpdateLoanPrincipalRemaining(LoanPrincipalRemaining.Create(50000M));

            Assert.Equal(50000M, installment.LoanPrincipalRemaining);
        }

        [Fact]
        public void UpdatePaymentStatus_LoanInstallment_ValidInfo_ShouldSucceed()
        {
            LoanInstallment installment = GetLoanInstallmentForEditing();
            installment.UpdatePaymentStatus(true);

            Assert.True(installment.IsPaid);
        }

        [Fact]
        public void UpdateUserId_LoanInstallment_ValidInfo_ShouldSucceed()
        {
            LoanAgreement agreement = GetLoanAgreementForEditing();
            Guid uID = EntityGuidID.Create(new Guid("94b1d516-a1c3-4df8-ae85-be1f34966601"));

            agreement.UpdateUserId(EntityGuidID.Create(uID));
            Assert.Equal(uID, agreement.UserId);
        }

        private LoanAgreement GetLoanAgreementForEditing() =>
            new LoanAgreement
            (
                LoanAgreementEconEvent.Create(EntityGuidID.Create(new Guid("0a7181c0-3ce9-4981-9559-157fd8e09cfb"))),
                EntityGuidID.Create(new Guid("b49471a0-5c1e-4a4d-97e7-288fb0f6338a")),
                LoanAmount.Create(33000),
                InterestRate.Create(.0725),
                LoanDate.Create(new DateTime(2021, 11, 15)),
                MaturityDate.Create(new DateTime(2022, 10, 15)),
                PaymentsPerYear.Create(12),
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