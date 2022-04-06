using System;
using System.Collections.Generic;
using PipefittersAccounting.SharedKernel.CommonValueObjects;
using PipefittersAccounting.Core.Financing.LoanAgreementAggregate;
using PipefittersAccounting.Core.Financing.LoanAgreementAggregate.ValueObjects;

namespace PipefittersAccounting.IntegrationTests.Base
{
    public class LoanAgreementTestData
    {
        public static LoanAgreement GetStandardLoanAgreementForCreating() =>
            new LoanAgreement
            (
                LoanAgreementEconEvent.Create(EntityGuidID.Create(new Guid("0a7181c0-3ce9-4981-9559-157fd8e09cfb"))),
                EntityGuidID.Create(new Guid("b49471a0-5c1e-4a4d-97e7-288fb0f6338a")),
                LoanAmount.Create(10000),
                InterestRate.Create(.0725M),
                LoanDate.Create(new DateTime(2022, 6, 15)),
                MaturityDate.Create(new DateTime(2023, 6, 15)),
                NumberOfInstallments.Create(12),
                EntityGuidID.Create(new Guid("660bb318-649e-470d-9d2b-693bfb0b2744")),
                GetInstallments12MonthsForCreateNewLoan()
            );

        public static LoanAgreement GetCustomLoanAgreementForCreating() =>
            new LoanAgreement
            (
                LoanAgreementEconEvent.Create(EntityGuidID.Create(new Guid("c51e4f98-38da-404f-ae78-26fafda2585f"))),
                EntityGuidID.Create(new Guid("94b1d516-a1c3-4df8-ae85-be1f34966601")),
                LoanAmount.Create(4000M),
                InterestRate.Create(.0250M),
                LoanDate.Create(new DateTime(2022, 4, 15)),
                MaturityDate.Create(new DateTime(2023, 4, 15)),
                NumberOfInstallments.Create(4),
                EntityGuidID.Create(new Guid("660bb318-649e-470d-9d2b-693bfb0b2744")),
                GetInstallments4MonthsOfCustomInstallments()
            );

        public static LoanAgreement GetLoanAgreementForEditing() =>
            new LoanAgreement
            (
                LoanAgreementEconEvent.Create(EntityGuidID.Create(new Guid("0a7181c0-3ce9-4981-9559-157fd8e09cfb"))),
                EntityGuidID.Create(new Guid("b49471a0-5c1e-4a4d-97e7-288fb0f6338a")),
                LoanAmount.Create(10000),
                InterestRate.Create(.0725M),
                LoanDate.Create(new DateTime(2022, 3, 15)),
                MaturityDate.Create(new DateTime(2023, 3, 15)),
                NumberOfInstallments.Create(12),
                EntityGuidID.Create(new Guid("660bb318-649e-470d-9d2b-693bfb0b2744")),
                GetInstallments12MonthsOfValidInfo()
            );

        public static List<LoanInstallment> GetInstallments12MonthsOfValidInfo()
        {
            List<LoanInstallment> pymtSchedule = new();

            pymtSchedule.Add
            (
                new LoanInstallment
                (
                    LoanPaymentEconEvent.Create(EntityGuidID.Create(Guid.NewGuid())),
                    EntityGuidID.Create(new Guid("41ca2b0a-0ed5-478b-9109-5dfda5b2eba1")),
                    InstallmentNumber.Create(1),
                    PaymentDueDate.Create(new DateTime(2022, 4, 15)),
                    EqualMonthlyInstallment.Create(865.26M),
                    LoanPrincipalAmount.Create(807M),
                    LoanInterestAmount.Create(58.26M),
                    LoanPrincipalRemaining.Create(9193.00M),
                    EntityGuidID.Create(new Guid("660bb318-649e-470d-9d2b-693bfb0b2744"))
                )
            );
            pymtSchedule.Add
            (
                new LoanInstallment
                (
                    LoanPaymentEconEvent.Create(EntityGuidID.Create(Guid.NewGuid())),
                    EntityGuidID.Create(new Guid("41ca2b0a-0ed5-478b-9109-5dfda5b2eba1")),
                    InstallmentNumber.Create(2),
                    PaymentDueDate.Create(new DateTime(2022, 5, 15)),
                    EqualMonthlyInstallment.Create(865.26M),
                    LoanPrincipalAmount.Create(812M),
                    LoanInterestAmount.Create(53.26M),
                    LoanPrincipalRemaining.Create(8381.00M),
                    EntityGuidID.Create(new Guid("660bb318-649e-470d-9d2b-693bfb0b2744"))
                )
            );
            pymtSchedule.Add
            (
                new LoanInstallment
                (
                    LoanPaymentEconEvent.Create(EntityGuidID.Create(Guid.NewGuid())),
                    EntityGuidID.Create(new Guid("41ca2b0a-0ed5-478b-9109-5dfda5b2eba1")),
                    InstallmentNumber.Create(3),
                    PaymentDueDate.Create(new DateTime(2022, 6, 15)),
                    EqualMonthlyInstallment.Create(865.26M),
                    LoanPrincipalAmount.Create(816.00M),
                    LoanInterestAmount.Create(49.26M),
                    LoanPrincipalRemaining.Create(7565.00M),
                    EntityGuidID.Create(new Guid("660bb318-649e-470d-9d2b-693bfb0b2744"))
                )
            );
            pymtSchedule.Add
            (
                new LoanInstallment
                (
                    LoanPaymentEconEvent.Create(EntityGuidID.Create(Guid.NewGuid())),
                    EntityGuidID.Create(new Guid("41ca2b0a-0ed5-478b-9109-5dfda5b2eba1")),
                    InstallmentNumber.Create(4),
                    PaymentDueDate.Create(new DateTime(2022, 7, 15)),
                    EqualMonthlyInstallment.Create(865.26M),
                    LoanPrincipalAmount.Create(821M),
                    LoanInterestAmount.Create(44.26M),
                    LoanPrincipalRemaining.Create(9193.00M),
                    EntityGuidID.Create(new Guid("660bb318-649e-470d-9d2b-693bfb0b2744"))
                )
            );
            pymtSchedule.Add
            (
                new LoanInstallment
                (
                    LoanPaymentEconEvent.Create(EntityGuidID.Create(Guid.NewGuid())),
                    EntityGuidID.Create(new Guid("41ca2b0a-0ed5-478b-9109-5dfda5b2eba1")),
                    InstallmentNumber.Create(5),
                    PaymentDueDate.Create(new DateTime(2022, 8, 15)),
                    EqualMonthlyInstallment.Create(865.26M),
                    LoanPrincipalAmount.Create(826M),
                    LoanInterestAmount.Create(39.26M),
                    LoanPrincipalRemaining.Create(5918.00M),
                    EntityGuidID.Create(new Guid("660bb318-649e-470d-9d2b-693bfb0b2744"))
                )
            );
            pymtSchedule.Add
            (
                new LoanInstallment
                (
                    LoanPaymentEconEvent.Create(EntityGuidID.Create(Guid.NewGuid())),
                    EntityGuidID.Create(new Guid("41ca2b0a-0ed5-478b-9109-5dfda5b2eba1")),
                    InstallmentNumber.Create(6),
                    PaymentDueDate.Create(new DateTime(2022, 9, 15)),
                    EqualMonthlyInstallment.Create(865.26M),
                    LoanPrincipalAmount.Create(831M),
                    LoanInterestAmount.Create(34.26M),
                    LoanPrincipalRemaining.Create(5087.00M),
                    EntityGuidID.Create(new Guid("660bb318-649e-470d-9d2b-693bfb0b2744"))
                )
            );
            pymtSchedule.Add
            (
                new LoanInstallment
                (
                    LoanPaymentEconEvent.Create(EntityGuidID.Create(Guid.NewGuid())),
                    EntityGuidID.Create(new Guid("41ca2b0a-0ed5-478b-9109-5dfda5b2eba1")),
                    InstallmentNumber.Create(7),
                    PaymentDueDate.Create(new DateTime(2022, 10, 15)),
                    EqualMonthlyInstallment.Create(865.26M),
                    LoanPrincipalAmount.Create(835.00M),
                    LoanInterestAmount.Create(30.26M),
                    LoanPrincipalRemaining.Create(4252.00M),
                    EntityGuidID.Create(new Guid("660bb318-649e-470d-9d2b-693bfb0b2744"))
                )
            );
            pymtSchedule.Add
            (
                new LoanInstallment
                (
                    LoanPaymentEconEvent.Create(EntityGuidID.Create(Guid.NewGuid())),
                    EntityGuidID.Create(new Guid("41ca2b0a-0ed5-478b-9109-5dfda5b2eba1")),
                    InstallmentNumber.Create(8),
                    PaymentDueDate.Create(new DateTime(2022, 11, 15)),
                    EqualMonthlyInstallment.Create(865.26M),
                    LoanPrincipalAmount.Create(841M),
                    LoanInterestAmount.Create(24.26M),
                    LoanPrincipalRemaining.Create(3411.00M),
                    EntityGuidID.Create(new Guid("660bb318-649e-470d-9d2b-693bfb0b2744"))
                )
            );
            pymtSchedule.Add
            (
                new LoanInstallment
                (
                    LoanPaymentEconEvent.Create(EntityGuidID.Create(Guid.NewGuid())),
                    EntityGuidID.Create(new Guid("41ca2b0a-0ed5-478b-9109-5dfda5b2eba1")),
                    InstallmentNumber.Create(9),
                    PaymentDueDate.Create(new DateTime(2022, 12, 15)),
                    EqualMonthlyInstallment.Create(865.26M),
                    LoanPrincipalAmount.Create(845M),
                    LoanInterestAmount.Create(20.26M),
                    LoanPrincipalRemaining.Create(2566.00M),
                    EntityGuidID.Create(new Guid("660bb318-649e-470d-9d2b-693bfb0b2744"))
                )
            );
            pymtSchedule.Add
            (
                new LoanInstallment
                (
                    LoanPaymentEconEvent.Create(EntityGuidID.Create(Guid.NewGuid())),
                    EntityGuidID.Create(new Guid("41ca2b0a-0ed5-478b-9109-5dfda5b2eba1")),
                    InstallmentNumber.Create(10),
                    PaymentDueDate.Create(new DateTime(2023, 1, 15)),
                    EqualMonthlyInstallment.Create(865.26M),
                    LoanPrincipalAmount.Create(850M),
                    LoanInterestAmount.Create(15.26M),
                    LoanPrincipalRemaining.Create(1716.00M),
                    EntityGuidID.Create(new Guid("660bb318-649e-470d-9d2b-693bfb0b2744"))
                )
            );
            pymtSchedule.Add
            (
                new LoanInstallment
                (
                    LoanPaymentEconEvent.Create(EntityGuidID.Create(Guid.NewGuid())),
                    EntityGuidID.Create(new Guid("41ca2b0a-0ed5-478b-9109-5dfda5b2eba1")),
                    InstallmentNumber.Create(11),
                    PaymentDueDate.Create(new DateTime(2023, 2, 15)),
                    EqualMonthlyInstallment.Create(865.26M),
                    LoanPrincipalAmount.Create(856.00M),
                    LoanInterestAmount.Create(9.26M),
                    LoanPrincipalRemaining.Create(860.00M),
                    EntityGuidID.Create(new Guid("660bb318-649e-470d-9d2b-693bfb0b2744"))
                )
            );
            pymtSchedule.Add
            (
                new LoanInstallment
                (
                    LoanPaymentEconEvent.Create(EntityGuidID.Create(Guid.NewGuid())),
                    EntityGuidID.Create(new Guid("41ca2b0a-0ed5-478b-9109-5dfda5b2eba1")),
                    InstallmentNumber.Create(12),
                    PaymentDueDate.Create(new DateTime(2023, 3, 15)),
                    EqualMonthlyInstallment.Create(865.26M),
                    LoanPrincipalAmount.Create(860M),
                    LoanInterestAmount.Create(5.26M),
                    LoanPrincipalRemaining.Create(0.00M),
                    EntityGuidID.Create(new Guid("660bb318-649e-470d-9d2b-693bfb0b2744"))
                )
            );

            return pymtSchedule;
        }


        public static LoanInstallment GetLoanInstallmentForEditing() =>
            new
            (
                LoanPaymentEconEvent.Create(EntityGuidID.Create(Guid.NewGuid())),
                EntityGuidID.Create(new Guid("41ca2b0a-0ed5-478b-9109-5dfda5b2eba1")),
                InstallmentNumber.Create(1),
                PaymentDueDate.Create(new DateTime(2022, 4, 2)),
                EqualMonthlyInstallment.Create(865.26M),
                LoanPrincipalAmount.Create(807M),
                LoanInterestAmount.Create(58.26M),
                LoanPrincipalRemaining.Create(9193.00M),
                EntityGuidID.Create(new Guid("660bb318-649e-470d-9d2b-693bfb0b2744"))
            );

        public static List<LoanInstallment> GetInstallments12MonthsForCreateNewLoan()
        {
            List<LoanInstallment> pymtSchedule = new();

            pymtSchedule.Add
            (
                new LoanInstallment
                (
                    LoanPaymentEconEvent.Create(EntityGuidID.Create(Guid.NewGuid())),
                    EntityGuidID.Create(new Guid("41ca2b0a-0ed5-478b-9109-5dfda5b2eba1")),
                    InstallmentNumber.Create(1),
                    PaymentDueDate.Create(new DateTime(2022, 7, 15)),
                    EqualMonthlyInstallment.Create(865.26M),
                    LoanPrincipalAmount.Create(807M),
                    LoanInterestAmount.Create(58.26M),
                    LoanPrincipalRemaining.Create(9193.00M),
                    EntityGuidID.Create(new Guid("660bb318-649e-470d-9d2b-693bfb0b2744"))
                )
            );
            pymtSchedule.Add
            (
                new LoanInstallment
                (
                    LoanPaymentEconEvent.Create(EntityGuidID.Create(Guid.NewGuid())),
                    EntityGuidID.Create(new Guid("41ca2b0a-0ed5-478b-9109-5dfda5b2eba1")),
                    InstallmentNumber.Create(2),
                    PaymentDueDate.Create(new DateTime(2022, 8, 15)),
                    EqualMonthlyInstallment.Create(865.26M),
                    LoanPrincipalAmount.Create(812M),
                    LoanInterestAmount.Create(53.26M),
                    LoanPrincipalRemaining.Create(8381.00M),
                    EntityGuidID.Create(new Guid("660bb318-649e-470d-9d2b-693bfb0b2744"))
                )
            );
            pymtSchedule.Add
            (
                new LoanInstallment
                (
                    LoanPaymentEconEvent.Create(EntityGuidID.Create(Guid.NewGuid())),
                    EntityGuidID.Create(new Guid("41ca2b0a-0ed5-478b-9109-5dfda5b2eba1")),
                    InstallmentNumber.Create(3),
                    PaymentDueDate.Create(new DateTime(2022, 9, 15)),
                    EqualMonthlyInstallment.Create(865.26M),
                    LoanPrincipalAmount.Create(816.00M),
                    LoanInterestAmount.Create(49.26M),
                    LoanPrincipalRemaining.Create(7565.00M),
                    EntityGuidID.Create(new Guid("660bb318-649e-470d-9d2b-693bfb0b2744"))
                )
            );
            pymtSchedule.Add
            (
                new LoanInstallment
                (
                    LoanPaymentEconEvent.Create(EntityGuidID.Create(Guid.NewGuid())),
                    EntityGuidID.Create(new Guid("41ca2b0a-0ed5-478b-9109-5dfda5b2eba1")),
                    InstallmentNumber.Create(4),
                    PaymentDueDate.Create(new DateTime(2022, 10, 15)),
                    EqualMonthlyInstallment.Create(865.26M),
                    LoanPrincipalAmount.Create(821M),
                    LoanInterestAmount.Create(44.26M),
                    LoanPrincipalRemaining.Create(9193.00M),
                    EntityGuidID.Create(new Guid("660bb318-649e-470d-9d2b-693bfb0b2744"))
                )
            );
            pymtSchedule.Add
            (
                new LoanInstallment
                (
                    LoanPaymentEconEvent.Create(EntityGuidID.Create(Guid.NewGuid())),
                    EntityGuidID.Create(new Guid("41ca2b0a-0ed5-478b-9109-5dfda5b2eba1")),
                    InstallmentNumber.Create(5),
                    PaymentDueDate.Create(new DateTime(2022, 11, 15)),
                    EqualMonthlyInstallment.Create(865.26M),
                    LoanPrincipalAmount.Create(826M),
                    LoanInterestAmount.Create(39.26M),
                    LoanPrincipalRemaining.Create(5918.00M),
                    EntityGuidID.Create(new Guid("660bb318-649e-470d-9d2b-693bfb0b2744"))
                )
            );
            pymtSchedule.Add
            (
                new LoanInstallment
                (
                    LoanPaymentEconEvent.Create(EntityGuidID.Create(Guid.NewGuid())),
                    EntityGuidID.Create(new Guid("41ca2b0a-0ed5-478b-9109-5dfda5b2eba1")),
                    InstallmentNumber.Create(6),
                    PaymentDueDate.Create(new DateTime(2022, 12, 15)),
                    EqualMonthlyInstallment.Create(865.26M),
                    LoanPrincipalAmount.Create(831M),
                    LoanInterestAmount.Create(34.26M),
                    LoanPrincipalRemaining.Create(5087.00M),
                    EntityGuidID.Create(new Guid("660bb318-649e-470d-9d2b-693bfb0b2744"))
                )
            );
            pymtSchedule.Add
            (
                new LoanInstallment
                (
                    LoanPaymentEconEvent.Create(EntityGuidID.Create(Guid.NewGuid())),
                    EntityGuidID.Create(new Guid("41ca2b0a-0ed5-478b-9109-5dfda5b2eba1")),
                    InstallmentNumber.Create(7),
                    PaymentDueDate.Create(new DateTime(2023, 1, 15)),
                    EqualMonthlyInstallment.Create(865.26M),
                    LoanPrincipalAmount.Create(835.00M),
                    LoanInterestAmount.Create(30.26M),
                    LoanPrincipalRemaining.Create(4252.00M),
                    EntityGuidID.Create(new Guid("660bb318-649e-470d-9d2b-693bfb0b2744"))
                )
            );
            pymtSchedule.Add
            (
                new LoanInstallment
                (
                    LoanPaymentEconEvent.Create(EntityGuidID.Create(Guid.NewGuid())),
                    EntityGuidID.Create(new Guid("41ca2b0a-0ed5-478b-9109-5dfda5b2eba1")),
                    InstallmentNumber.Create(8),
                    PaymentDueDate.Create(new DateTime(2023, 2, 15)),
                    EqualMonthlyInstallment.Create(865.26M),
                    LoanPrincipalAmount.Create(841M),
                    LoanInterestAmount.Create(24.26M),
                    LoanPrincipalRemaining.Create(3411.00M),
                    EntityGuidID.Create(new Guid("660bb318-649e-470d-9d2b-693bfb0b2744"))
                )
            );
            pymtSchedule.Add
            (
                new LoanInstallment
                (
                    LoanPaymentEconEvent.Create(EntityGuidID.Create(Guid.NewGuid())),
                    EntityGuidID.Create(new Guid("41ca2b0a-0ed5-478b-9109-5dfda5b2eba1")),
                    InstallmentNumber.Create(9),
                    PaymentDueDate.Create(new DateTime(2023, 3, 15)),
                    EqualMonthlyInstallment.Create(865.26M),
                    LoanPrincipalAmount.Create(845M),
                    LoanInterestAmount.Create(20.26M),
                    LoanPrincipalRemaining.Create(2566.00M),
                    EntityGuidID.Create(new Guid("660bb318-649e-470d-9d2b-693bfb0b2744"))
                )
            );
            pymtSchedule.Add
            (
                new LoanInstallment
                (
                    LoanPaymentEconEvent.Create(EntityGuidID.Create(Guid.NewGuid())),
                    EntityGuidID.Create(new Guid("41ca2b0a-0ed5-478b-9109-5dfda5b2eba1")),
                    InstallmentNumber.Create(10),
                    PaymentDueDate.Create(new DateTime(2023, 4, 15)),
                    EqualMonthlyInstallment.Create(865.26M),
                    LoanPrincipalAmount.Create(850M),
                    LoanInterestAmount.Create(15.26M),
                    LoanPrincipalRemaining.Create(1716.00M),
                    EntityGuidID.Create(new Guid("660bb318-649e-470d-9d2b-693bfb0b2744"))
                )
            );
            pymtSchedule.Add
            (
                new LoanInstallment
                (
                    LoanPaymentEconEvent.Create(EntityGuidID.Create(Guid.NewGuid())),
                    EntityGuidID.Create(new Guid("41ca2b0a-0ed5-478b-9109-5dfda5b2eba1")),
                    InstallmentNumber.Create(11),
                    PaymentDueDate.Create(new DateTime(2023, 5, 15)),
                    EqualMonthlyInstallment.Create(865.26M),
                    LoanPrincipalAmount.Create(856.00M),
                    LoanInterestAmount.Create(9.26M),
                    LoanPrincipalRemaining.Create(860.00M),
                    EntityGuidID.Create(new Guid("660bb318-649e-470d-9d2b-693bfb0b2744"))
                )
            );
            pymtSchedule.Add
            (
                new LoanInstallment
                (
                    LoanPaymentEconEvent.Create(EntityGuidID.Create(Guid.NewGuid())),
                    EntityGuidID.Create(new Guid("41ca2b0a-0ed5-478b-9109-5dfda5b2eba1")),
                    InstallmentNumber.Create(12),
                    PaymentDueDate.Create(new DateTime(2023, 6, 15)),
                    EqualMonthlyInstallment.Create(865.26M),
                    LoanPrincipalAmount.Create(860M),
                    LoanInterestAmount.Create(5.26M),
                    LoanPrincipalRemaining.Create(0.00M),
                    EntityGuidID.Create(new Guid("660bb318-649e-470d-9d2b-693bfb0b2744"))
                )
            );

            return pymtSchedule;
        }

        public static List<LoanInstallment> GetInstallments4MonthsOfCustomInstallments()
        {
            List<LoanInstallment> pymtSchedule = new();

            pymtSchedule.Add
            (
                new LoanInstallment
                (
                    LoanPaymentEconEvent.Create(EntityGuidID.Create(Guid.NewGuid())),
                    EntityGuidID.Create(new Guid("c51e4f98-38da-404f-ae78-26fafda2585f")),
                    InstallmentNumber.Create(1),
                    PaymentDueDate.Create(new DateTime(2022, 7, 15)),
                    EqualMonthlyInstallment.Create(1100.00M),
                    LoanPrincipalAmount.Create(1000M),
                    LoanInterestAmount.Create(100M),
                    LoanPrincipalRemaining.Create(3000.00M),
                    EntityGuidID.Create(new Guid("660bb318-649e-470d-9d2b-693bfb0b2744"))
                )
            );
            pymtSchedule.Add
            (
                new LoanInstallment
                (
                    LoanPaymentEconEvent.Create(EntityGuidID.Create(Guid.NewGuid())),
                    EntityGuidID.Create(new Guid("c51e4f98-38da-404f-ae78-26fafda2585f")),
                    InstallmentNumber.Create(2),
                    PaymentDueDate.Create(new DateTime(2022, 10, 15)),
                    EqualMonthlyInstallment.Create(1100.00M),
                    LoanPrincipalAmount.Create(1010M),
                    LoanInterestAmount.Create(90M),
                    LoanPrincipalRemaining.Create(1990.00M),
                    EntityGuidID.Create(new Guid("660bb318-649e-470d-9d2b-693bfb0b2744"))
                )
            );
            pymtSchedule.Add
            (
                new LoanInstallment
                (
                    LoanPaymentEconEvent.Create(EntityGuidID.Create(Guid.NewGuid())),
                    EntityGuidID.Create(new Guid("c51e4f98-38da-404f-ae78-26fafda2585f")),
                    InstallmentNumber.Create(3),
                    PaymentDueDate.Create(new DateTime(2023, 1, 15)),
                    EqualMonthlyInstallment.Create(1100.00M),
                    LoanPrincipalAmount.Create(1060M),
                    LoanInterestAmount.Create(40M),
                    LoanPrincipalRemaining.Create(930.00M),
                    EntityGuidID.Create(new Guid("660bb318-649e-470d-9d2b-693bfb0b2744"))
                )
            );
            pymtSchedule.Add
            (
                new LoanInstallment
                (
                    LoanPaymentEconEvent.Create(EntityGuidID.Create(Guid.NewGuid())),
                    EntityGuidID.Create(new Guid("c51e4f98-38da-404f-ae78-26fafda2585f")),
                    InstallmentNumber.Create(4),
                    PaymentDueDate.Create(new DateTime(2023, 4, 15)),
                    EqualMonthlyInstallment.Create(1100.00M),
                    LoanPrincipalAmount.Create(930M),
                    LoanInterestAmount.Create(170M),
                    LoanPrincipalRemaining.Create(0.00M),
                    EntityGuidID.Create(new Guid("660bb318-649e-470d-9d2b-693bfb0b2744"))
                )
            );

            return pymtSchedule;
        }
    }
}