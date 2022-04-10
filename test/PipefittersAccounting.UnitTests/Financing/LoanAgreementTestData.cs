using System;
using System.Collections.Generic;
using PipefittersAccounting.SharedKernel.CommonValueObjects;
using PipefittersAccounting.SharedModel.WriteModels.Financing;
using PipefittersAccounting.Core.Financing.LoanAgreementAggregate;
using PipefittersAccounting.Core.Financing.LoanAgreementAggregate.Components;
using PipefittersAccounting.Core.Financing.LoanAgreementAggregate.ValueObjects;

namespace PipefittersAccounting.UnitTests.Financing
{
    public class LoanAgreementTestData
    {
        public static CreateLoanInstallmentInfo GetLoanInstallmentInfo()
            => new()
            {
                LoanInstallmentId = Guid.NewGuid(),
                LoanId = new Guid("41ca2b0a-0ed5-478b-9109-5dfda5b2eba1"),
                InstallmentNumber = 12,
                PaymentDueDate = new DateTime(2022, 9, 15),
                EqualMonthlyInstallment = 865.26M,
                LoanPrincipalAmount = 831.00M,
                LoanInterestAmount = 34.26M,
                LoanPrincipalRemaining = 5087.00M,
                UserId = new Guid("660bb318-649e-470d-9d2b-693bfb0b2744")
            };

        public static CreateLoanAgreementInfo GetCreateLoanAgreementInfo() =>
            new CreateLoanAgreementInfo
            {
                LoanId = new Guid("0a7181c0-3ce9-4981-9559-157fd8e09cfb"),
                FinancierId = new Guid("b49471a0-5c1e-4a4d-97e7-288fb0f6338a"),
                LoanAmount = 10000M,
                InterestRate = .0725M,
                LoanDate = new DateTime(2022, 3, 15),
                MaturityDate = new DateTime(2023, 3, 15),
                NumberOfInstallments = 12,
                UserId = new Guid("660bb318-649e-470d-9d2b-693bfb0b2744"),
                AmortizationSchedule = GetCreateLoanInstallmentInfo12MonthsOfValidInfo()
            };

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

        public static List<CreateLoanInstallmentInfo> GetCreateLoanInstallmentInfo12MonthsOfValidInfo()
        {
            List<CreateLoanInstallmentInfo> pymtSchedule = new();

            pymtSchedule.Add
            (
                new CreateLoanInstallmentInfo
                {
                    LoanInstallmentId = Guid.NewGuid(),
                    LoanId = new Guid("41ca2b0a-0ed5-478b-9109-5dfda5b2eba1"),
                    InstallmentNumber = 1,
                    PaymentDueDate = new DateTime(2022, 4, 15),
                    EqualMonthlyInstallment = 865.26M,
                    LoanPrincipalAmount = 807M,
                    LoanInterestAmount = 58.26M,
                    LoanPrincipalRemaining = 9193.00M,
                    UserId = new Guid("660bb318-649e-470d-9d2b-693bfb0b2744")
                }
            );
            pymtSchedule.Add
            (
                new CreateLoanInstallmentInfo
                {
                    LoanInstallmentId = Guid.NewGuid(),
                    LoanId = new Guid("41ca2b0a-0ed5-478b-9109-5dfda5b2eba1"),
                    InstallmentNumber = 2,
                    PaymentDueDate = new DateTime(2022, 5, 15),
                    EqualMonthlyInstallment = 865.26M,
                    LoanPrincipalAmount = 812M,
                    LoanInterestAmount = 53.26M,
                    LoanPrincipalRemaining = 8381.00M,
                    UserId = new Guid("660bb318-649e-470d-9d2b-693bfb0b2744")
                }
            );
            pymtSchedule.Add
            (
                new CreateLoanInstallmentInfo
                {
                    LoanInstallmentId = Guid.NewGuid(),
                    LoanId = new Guid("41ca2b0a-0ed5-478b-9109-5dfda5b2eba1"),
                    InstallmentNumber = 3,
                    PaymentDueDate = new DateTime(2022, 6, 15),
                    EqualMonthlyInstallment = 865.26M,
                    LoanPrincipalAmount = 816.00M,
                    LoanInterestAmount = 49.26M,
                    LoanPrincipalRemaining = 7565.00M,
                    UserId = new Guid("660bb318-649e-470d-9d2b-693bfb0b2744")
                }
            );
            pymtSchedule.Add
            (
                new CreateLoanInstallmentInfo
                {
                    LoanInstallmentId = Guid.NewGuid(),
                    LoanId = new Guid("41ca2b0a-0ed5-478b-9109-5dfda5b2eba1"),
                    InstallmentNumber = 4,
                    PaymentDueDate = new DateTime(2022, 7, 15),
                    EqualMonthlyInstallment = 865.26M,
                    LoanPrincipalAmount = 821M,
                    LoanInterestAmount = 44.26M,
                    LoanPrincipalRemaining = 6744.00M,
                    UserId = new Guid("660bb318-649e-470d-9d2b-693bfb0b2744")
                }
            );
            pymtSchedule.Add
            (
                new CreateLoanInstallmentInfo
                {
                    LoanInstallmentId = Guid.NewGuid(),
                    LoanId = new Guid("41ca2b0a-0ed5-478b-9109-5dfda5b2eba1"),
                    InstallmentNumber = 5,
                    PaymentDueDate = new DateTime(2022, 8, 15),
                    EqualMonthlyInstallment = 865.26M,
                    LoanPrincipalAmount = 826M,
                    LoanInterestAmount = 39.26M,
                    LoanPrincipalRemaining = 5918.00M,
                    UserId = new Guid("660bb318-649e-470d-9d2b-693bfb0b2744")
                }
            );
            pymtSchedule.Add
            (
                new CreateLoanInstallmentInfo
                {
                    LoanInstallmentId = Guid.NewGuid(),
                    LoanId = new Guid("41ca2b0a-0ed5-478b-9109-5dfda5b2eba1"),
                    InstallmentNumber = 6,
                    PaymentDueDate = new DateTime(2022, 9, 15),
                    EqualMonthlyInstallment = 865.26M,
                    LoanPrincipalAmount = 831M,
                    LoanInterestAmount = 34.26M,
                    LoanPrincipalRemaining = 5087.00M,
                    UserId = new Guid("660bb318-649e-470d-9d2b-693bfb0b2744")
                }
            );
            pymtSchedule.Add
            (
                new CreateLoanInstallmentInfo
                {
                    LoanInstallmentId = Guid.NewGuid(),
                    LoanId = new Guid("41ca2b0a-0ed5-478b-9109-5dfda5b2eba1"),
                    InstallmentNumber = 7,
                    PaymentDueDate = new DateTime(2022, 10, 15),
                    EqualMonthlyInstallment = 865.26M,
                    LoanPrincipalAmount = 835.00M,
                    LoanInterestAmount = 30.26M,
                    LoanPrincipalRemaining = 4252.00M,
                    UserId = new Guid("660bb318-649e-470d-9d2b-693bfb0b2744")
                }
            );
            pymtSchedule.Add
            (
                new CreateLoanInstallmentInfo
                {
                    LoanInstallmentId = Guid.NewGuid(),
                    LoanId = new Guid("41ca2b0a-0ed5-478b-9109-5dfda5b2eba1"),
                    InstallmentNumber = 8,
                    PaymentDueDate = new DateTime(2022, 11, 15),
                    EqualMonthlyInstallment = 865.26M,
                    LoanPrincipalAmount = 841M,
                    LoanInterestAmount = 24.26M,
                    LoanPrincipalRemaining = 3411.00M,
                    UserId = new Guid("660bb318-649e-470d-9d2b-693bfb0b2744")
                }
            );
            pymtSchedule.Add
            (
                new CreateLoanInstallmentInfo
                {
                    LoanInstallmentId = Guid.NewGuid(),
                    LoanId = new Guid("41ca2b0a-0ed5-478b-9109-5dfda5b2eba1"),
                    InstallmentNumber = 9,
                    PaymentDueDate = new DateTime(2022, 12, 15),
                    EqualMonthlyInstallment = 865.26M,
                    LoanPrincipalAmount = 845M,
                    LoanInterestAmount = 20.26M,
                    LoanPrincipalRemaining = 2566.00M,
                    UserId = new Guid("660bb318-649e-470d-9d2b-693bfb0b2744")
                }
            );
            pymtSchedule.Add
            (
                new CreateLoanInstallmentInfo
                {
                    LoanInstallmentId = Guid.NewGuid(),
                    LoanId = new Guid("41ca2b0a-0ed5-478b-9109-5dfda5b2eba1"),
                    InstallmentNumber = 10,
                    PaymentDueDate = new DateTime(2023, 1, 15),
                    EqualMonthlyInstallment = 865.26M,
                    LoanPrincipalAmount = 850M,
                    LoanInterestAmount = 15.26M,
                    LoanPrincipalRemaining = 1716.00M,
                    UserId = new Guid("660bb318-649e-470d-9d2b-693bfb0b2744")
                }
            );
            pymtSchedule.Add
            (
                new CreateLoanInstallmentInfo
                {
                    LoanInstallmentId = Guid.NewGuid(),
                    LoanId = new Guid("41ca2b0a-0ed5-478b-9109-5dfda5b2eba1"),
                    InstallmentNumber = 11,
                    PaymentDueDate = new DateTime(2023, 2, 15),
                    EqualMonthlyInstallment = 865.26M,
                    LoanPrincipalAmount = 856.00M,
                    LoanInterestAmount = 9.26M,
                    LoanPrincipalRemaining = 860.00M,
                    UserId = new Guid("660bb318-649e-470d-9d2b-693bfb0b2744")
                }
            );
            pymtSchedule.Add
            (
                new CreateLoanInstallmentInfo
                {
                    LoanInstallmentId = Guid.NewGuid(),
                    LoanId = new Guid("41ca2b0a-0ed5-478b-9109-5dfda5b2eba1"),
                    InstallmentNumber = 12,
                    PaymentDueDate = new DateTime(2023, 3, 15),
                    EqualMonthlyInstallment = 865.26M,
                    LoanPrincipalAmount = 860M,
                    LoanInterestAmount = 5.26M,
                    LoanPrincipalRemaining = 0.00M,
                    UserId = new Guid("660bb318-649e-470d-9d2b-693bfb0b2744")
                }
            );

            return pymtSchedule;
        }

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
                    LoanPrincipalRemaining.Create(6744.00M),
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

        public static List<InstallmentRecord> GetInstallments12Months()
        {
            List<InstallmentRecord> pymtSchedule = new();

            pymtSchedule.Add
            (
                new InstallmentRecord(InstallmentNumber: 1,
                                PaymentDueDate: new DateTime(2022, 4, 30),
                                Payment: 1514.21M,
                                Principal: 1412.00M,
                                Interest: 102.21M,
                                TotalInterestPaid: 102.21M,
                                RemainingBalance: 16088.00M)
            );
            pymtSchedule.Add
            (
                new InstallmentRecord(InstallmentNumber: 2,
                                PaymentDueDate: new DateTime(2022, 5, 30),
                                Payment: 1514.21M,
                                Principal: 1420.00M,
                                Interest: 94.21M,
                                TotalInterestPaid: 196.42M,
                                RemainingBalance: 14668.00M)
            );
            pymtSchedule.Add
            (
                new InstallmentRecord(InstallmentNumber: 3,
                                PaymentDueDate: new DateTime(2022, 6, 30),
                                Payment: 1514.21M,
                                Principal: 1429.00M,
                                Interest: 85.21M,
                                TotalInterestPaid: 281.63M,
                                RemainingBalance: 13239.00M)
            );
            pymtSchedule.Add
            (
                new InstallmentRecord(InstallmentNumber: 4,
                                PaymentDueDate: new DateTime(2022, 7, 30),
                                Payment: 1514.21M,
                                Principal: 1437.00M,
                                Interest: 77.21M,
                                TotalInterestPaid: 358.84M,
                                RemainingBalance: 11802.00M)
            );
            pymtSchedule.Add
            (
                new InstallmentRecord(InstallmentNumber: 5,
                                PaymentDueDate: new DateTime(2022, 8, 30),
                                Payment: 1514.21M,
                                Principal: 1445.00M,
                                Interest: 69.21M,
                                TotalInterestPaid: 428.05M,
                                RemainingBalance: 10357.00M)
            );
            pymtSchedule.Add
            (
                new InstallmentRecord(InstallmentNumber: 6,
                                PaymentDueDate: new DateTime(2022, 9, 30),
                                Payment: 1514.21M,
                                Principal: 1454.00M,
                                Interest: 60.21M,
                                TotalInterestPaid: 488.26M,
                                RemainingBalance: 8903.00M)
            );
            pymtSchedule.Add
            (
                new InstallmentRecord(InstallmentNumber: 7,
                                PaymentDueDate: new DateTime(2022, 10, 30),
                                Payment: 1514.21M,
                                Principal: 1463.00M,
                                Interest: 51.21M,
                                TotalInterestPaid: 539.47M,
                                RemainingBalance: 7440.00M)
            );
            pymtSchedule.Add
            (
                new InstallmentRecord(InstallmentNumber: 8,
                                PaymentDueDate: new DateTime(2022, 11, 30),
                                Payment: 1514.21M,
                                Principal: 1470.00M,
                                Interest: 44.21M,
                                TotalInterestPaid: 583.68M,
                                RemainingBalance: 5970.00M)
            );
            pymtSchedule.Add
            (
                new InstallmentRecord(InstallmentNumber: 9,
                                PaymentDueDate: new DateTime(2022, 12, 30),
                                Payment: 1514.21M,
                                Principal: 1480.00M,
                                Interest: 34.21M,
                                TotalInterestPaid: 617.89M,
                                RemainingBalance: 4490.00M)
            );
            pymtSchedule.Add
            (
                new InstallmentRecord(InstallmentNumber: 10,
                                PaymentDueDate: new DateTime(2023, 1, 30),
                                Payment: 1514.21M,
                                Principal: 1488.00M,
                                Interest: 26.21M,
                                TotalInterestPaid: 644.10M,
                                RemainingBalance: 3002.00M)
            );
            pymtSchedule.Add
            (
                new InstallmentRecord(InstallmentNumber: 11,
                                PaymentDueDate: new DateTime(2023, 2, 28),
                                Payment: 1514.21M,
                                Principal: 1496.00M,
                                Interest: 18.21M,
                                TotalInterestPaid: 662.31M,
                                RemainingBalance: 1506.00M)
            );
            pymtSchedule.Add
            (
                new InstallmentRecord(InstallmentNumber: 12,
                                PaymentDueDate: new DateTime(2023, 3, 30),
                                Payment: 1514.21M,
                                Principal: 1506.00M,
                                Interest: 8.21M,
                                TotalInterestPaid: 670.52M,
                                RemainingBalance: 0M)
            );

            return pymtSchedule;
        }
    }
}