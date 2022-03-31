using System;
using System.Collections.Generic;
using PipefittersAccounting.SharedKernel.CommonValueObjects;
using PipefittersAccounting.Core.Financing.LoanAgreementAggregate;
using PipefittersAccounting.Core.Financing.LoanAgreementAggregate.Components;
using PipefittersAccounting.Core.Financing.LoanAgreementAggregate.ValueObjects;

namespace PipefittersAccounting.UnitTests.Financing
{
    public class LoanAgreementTestData
    {
        public static LoanAgreement GetLoanAgreementForEditing() =>
            new LoanAgreement
            (
                LoanAgreementEconEvent.Create(EntityGuidID.Create(new Guid("0a7181c0-3ce9-4981-9559-157fd8e09cfb"))),
                EntityGuidID.Create(new Guid("b49471a0-5c1e-4a4d-97e7-288fb0f6338a")),
                LoanAmount.Create(33000),
                InterestRate.Create(.0725M),
                LoanDate.Create(new DateTime(2021, 11, 15)),
                MaturityDate.Create(new DateTime(2022, 11, 15)),
                NumberOfInstallments.Create(12),
                EntityGuidID.Create(new Guid("660bb318-649e-470d-9d2b-693bfb0b2744")),
                GetInstallmentsLoanRepymtScheduleUpdate()
            );

        public static LoanInstallment GetLoanInstallmentForEditing() =>
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

        public static List<Installment> GetInstallmentsValidInfo()
        {
            List<Installment> pymtSchedule = new();

            pymtSchedule.Add
            (
                new Installment(InstallmentNumber: 1,
                                PaymentDueDate: new DateTime(2022, 4, 15),
                                Payment: 455.65M,
                                Principal: 447.00M,
                                Interest: 8.65M,
                                TotalInterestPaid: 8.65M,
                                RemainingBalance: 1353.00M)
            );
            pymtSchedule.Add
            (
                new Installment(InstallmentNumber: 2,
                                PaymentDueDate: new DateTime(2022, 5, 15),
                                Payment: 455.65M,
                                Principal: 449.00M,
                                Interest: 6.65M,
                                TotalInterestPaid: 15.30M,
                                RemainingBalance: 904.00M)
            );
            pymtSchedule.Add
            (
                new Installment(InstallmentNumber: 3,
                                PaymentDueDate: new DateTime(2022, 6, 15),
                                Payment: 455.65M,
                                Principal: 451.00M,
                                Interest: 4.65M,
                                TotalInterestPaid: 19.95M,
                                RemainingBalance: 453.00M)
            );
            pymtSchedule.Add
            (
                new Installment(InstallmentNumber: 4,
                                PaymentDueDate: new DateTime(2022, 7, 15),
                                Payment: 455.65M,
                                Principal: 453.00M,
                                Interest: 2.65M,
                                TotalInterestPaid: 22.60M,
                                RemainingBalance: 0M)
            );

            return pymtSchedule;
        }

        public static List<Installment> GetInstallmentsTooFewInstallments()
        {
            List<Installment> pymtSchedule = new();

            pymtSchedule.Add
            (
                new Installment(InstallmentNumber: 1,
                                PaymentDueDate: new DateTime(2022, 4, 15),
                                Payment: 455.65M,
                                Principal: 447.00M,
                                Interest: 8.65M,
                                TotalInterestPaid: 8.65M,
                                RemainingBalance: 1353.00M)
            );
            pymtSchedule.Add
            (
                new Installment(InstallmentNumber: 2,
                                PaymentDueDate: new DateTime(2022, 5, 15),
                                Payment: 455.65M,
                                Principal: 449.00M,
                                Interest: 6.65M,
                                TotalInterestPaid: 15.30M,
                                RemainingBalance: 904.00M)
            );
            pymtSchedule.Add
            (
                new Installment(InstallmentNumber: 3,
                                PaymentDueDate: new DateTime(2022, 6, 15),
                                Payment: 455.65M,
                                Principal: 451.00M,
                                Interest: 4.65M,
                                TotalInterestPaid: 19.95M,
                                RemainingBalance: 453.00M)
            );

            return pymtSchedule;
        }

        public static List<Installment> GetInstallmentsTooManyInstallments()
        {
            List<Installment> pymtSchedule = new();

            pymtSchedule.Add
            (
                new Installment(InstallmentNumber: 1,
                                PaymentDueDate: new DateTime(2022, 4, 15),
                                Payment: 455.65M,
                                Principal: 447.00M,
                                Interest: 8.65M,
                                TotalInterestPaid: 8.65M,
                                RemainingBalance: 1353.00M)
            );
            pymtSchedule.Add
            (
                new Installment(InstallmentNumber: 2,
                                PaymentDueDate: new DateTime(2022, 5, 15),
                                Payment: 455.65M,
                                Principal: 449.00M,
                                Interest: 6.65M,
                                TotalInterestPaid: 15.30M,
                                RemainingBalance: 904.00M)
            );
            pymtSchedule.Add
            (
                new Installment(InstallmentNumber: 3,
                                PaymentDueDate: new DateTime(2022, 6, 15),
                                Payment: 455.65M,
                                Principal: 451.00M,
                                Interest: 4.65M,
                                TotalInterestPaid: 19.95M,
                                RemainingBalance: 453.00M)
            );
            pymtSchedule.Add
            (
                new Installment(InstallmentNumber: 4,
                                PaymentDueDate: new DateTime(2022, 7, 15),
                                Payment: 455.65M,
                                Principal: 453.00M,
                                Interest: 2.65M,
                                TotalInterestPaid: 22.60M,
                                RemainingBalance: 0M)
            );
            pymtSchedule.Add
            (
                new Installment(InstallmentNumber: 5,
                                PaymentDueDate: new DateTime(2022, 9, 15),  // Invalid payment date
                                Payment: 455.65M,
                                Principal: 453.00M,
                                Interest: 2.65M,
                                TotalInterestPaid: 22.60M,
                                RemainingBalance: 0M)
            );

            return pymtSchedule;
        }

        public static List<Installment> GetInstallmentsDictIndexDoesNotStartAtOne()
        {
            List<Installment> pymtSchedule = new();

            pymtSchedule.Add
            (
                new Installment(InstallmentNumber: 1,
                                PaymentDueDate: new DateTime(2022, 4, 15),
                                Payment: 455.65M,
                                Principal: 447.00M,
                                Interest: 8.65M,
                                TotalInterestPaid: 8.65M,
                                RemainingBalance: 1353.00M)
            );
            pymtSchedule.Add
            (
                new Installment(InstallmentNumber: 2,
                                PaymentDueDate: new DateTime(2022, 5, 15),
                                Payment: 455.65M,
                                Principal: 449.00M,
                                Interest: 6.65M,
                                TotalInterestPaid: 15.30M,
                                RemainingBalance: 904.00M)
            );
            pymtSchedule.Add
            (
                new Installment(InstallmentNumber: 3,
                                PaymentDueDate: new DateTime(2022, 6, 15),
                                Payment: 455.65M,
                                Principal: 451.00M,
                                Interest: 4.65M,
                                TotalInterestPaid: 19.95M,
                                RemainingBalance: 453.00M)
            );
            pymtSchedule.Add
            (
                new Installment(InstallmentNumber: 4,
                                PaymentDueDate: new DateTime(2022, 7, 15),
                                Payment: 455.65M,
                                Principal: 453.00M,
                                Interest: 2.65M,
                                TotalInterestPaid: 22.60M,
                                RemainingBalance: 0M)
            );

            return pymtSchedule;
        }

        public static List<Installment> GetInstallments12Months()
        {
            List<Installment> pymtSchedule = new();

            pymtSchedule.Add
            (
                new Installment(InstallmentNumber: 1,
                                PaymentDueDate: new DateTime(2022, 4, 30),
                                Payment: 1514.21M,
                                Principal: 1412.00M,
                                Interest: 102.21M,
                                TotalInterestPaid: 102.21M,
                                RemainingBalance: 16088.00M)
            );
            pymtSchedule.Add
            (
                new Installment(InstallmentNumber: 2,
                                PaymentDueDate: new DateTime(2022, 5, 30),
                                Payment: 1514.21M,
                                Principal: 1420.00M,
                                Interest: 94.21M,
                                TotalInterestPaid: 196.42M,
                                RemainingBalance: 14668.00M)
            );
            pymtSchedule.Add
            (
                new Installment(InstallmentNumber: 3,
                                PaymentDueDate: new DateTime(2022, 6, 30),
                                Payment: 1514.21M,
                                Principal: 1429.00M,
                                Interest: 85.21M,
                                TotalInterestPaid: 281.63M,
                                RemainingBalance: 13239.00M)
            );
            pymtSchedule.Add
            (
                new Installment(InstallmentNumber: 4,
                                PaymentDueDate: new DateTime(2022, 7, 30),
                                Payment: 1514.21M,
                                Principal: 1437.00M,
                                Interest: 77.21M,
                                TotalInterestPaid: 358.84M,
                                RemainingBalance: 11802.00M)
            );
            pymtSchedule.Add
            (
                new Installment(InstallmentNumber: 5,
                                PaymentDueDate: new DateTime(2022, 8, 30),
                                Payment: 1514.21M,
                                Principal: 1445.00M,
                                Interest: 69.21M,
                                TotalInterestPaid: 428.05M,
                                RemainingBalance: 10357.00M)
            );
            pymtSchedule.Add
            (
                new Installment(InstallmentNumber: 6,
                                PaymentDueDate: new DateTime(2022, 9, 30),
                                Payment: 1514.21M,
                                Principal: 1454.00M,
                                Interest: 60.21M,
                                TotalInterestPaid: 488.26M,
                                RemainingBalance: 8903.00M)
            );
            pymtSchedule.Add
            (
                new Installment(InstallmentNumber: 7,
                                PaymentDueDate: new DateTime(2022, 10, 30),
                                Payment: 1514.21M,
                                Principal: 1463.00M,
                                Interest: 51.21M,
                                TotalInterestPaid: 539.47M,
                                RemainingBalance: 7440.00M)
            );
            pymtSchedule.Add
            (
                new Installment(InstallmentNumber: 8,
                                PaymentDueDate: new DateTime(2022, 11, 30),
                                Payment: 1514.21M,
                                Principal: 1470.00M,
                                Interest: 44.21M,
                                TotalInterestPaid: 583.68M,
                                RemainingBalance: 5970.00M)
            );
            pymtSchedule.Add
            (
                new Installment(InstallmentNumber: 9,
                                PaymentDueDate: new DateTime(2022, 12, 30),
                                Payment: 1514.21M,
                                Principal: 1480.00M,
                                Interest: 34.21M,
                                TotalInterestPaid: 617.89M,
                                RemainingBalance: 4490.00M)
            );
            pymtSchedule.Add
            (
                new Installment(InstallmentNumber: 10,
                                PaymentDueDate: new DateTime(2023, 1, 30),
                                Payment: 1514.21M,
                                Principal: 1488.00M,
                                Interest: 26.21M,
                                TotalInterestPaid: 644.10M,
                                RemainingBalance: 3002.00M)
            );
            pymtSchedule.Add
            (
                new Installment(InstallmentNumber: 11,
                                PaymentDueDate: new DateTime(2023, 2, 28),
                                Payment: 1514.21M,
                                Principal: 1496.00M,
                                Interest: 18.21M,
                                TotalInterestPaid: 662.31M,
                                RemainingBalance: 1506.00M)
            );
            pymtSchedule.Add
            (
                new Installment(InstallmentNumber: 12,
                                PaymentDueDate: new DateTime(2023, 3, 30),
                                Payment: 1514.21M,
                                Principal: 1506.00M,
                                Interest: 8.21M,
                                TotalInterestPaid: 670.52M,
                                RemainingBalance: 0M)
            );

            return pymtSchedule;
        }

        public static List<Installment> GetInstallmentsLoanRepymtScheduleUpdate()
        {
            List<Installment> pymtSchedule = new();

            pymtSchedule.Add
            (
                new Installment(InstallmentNumber: 1,
                                PaymentDueDate: new DateTime(2021, 12, 15),
                                Payment: 2855.37M,
                                Principal: 2663.00M,
                                Interest: 192.37M,
                                TotalInterestPaid: 192.37M,
                                RemainingBalance: 30337.00M)
            );
            pymtSchedule.Add
            (
                new Installment(InstallmentNumber: 2,
                                PaymentDueDate: new DateTime(2022, 5, 30),
                                Payment: 2855.37M,
                                Principal: 2678.00M,
                                Interest: 177.37M,
                                TotalInterestPaid: 369.74M,
                                RemainingBalance: 27659.00M)
            );
            pymtSchedule.Add
            (
                new Installment(InstallmentNumber: 3,
                                PaymentDueDate: new DateTime(2022, 6, 30),
                                Payment: 2855.37M,
                                Principal: 2694.00M,
                                Interest: 161.37M,
                                TotalInterestPaid: 531.11M,
                                RemainingBalance: 24965.00M)
            );
            pymtSchedule.Add
            (
                new Installment(InstallmentNumber: 4,
                                PaymentDueDate: new DateTime(2022, 7, 30),
                                Payment: 2855.37M,
                                Principal: 2710.00M,
                                Interest: 145.37M,
                                TotalInterestPaid: 676.48M,
                                RemainingBalance: 22255.00M)
            );
            pymtSchedule.Add
            (
                new Installment(InstallmentNumber: 5,
                                PaymentDueDate: new DateTime(2022, 8, 30),
                                Payment: 2855.37M,
                                Principal: 2726.00M,
                                Interest: 129.37M,
                                TotalInterestPaid: 805.85M,
                                RemainingBalance: 19529.00M)
            );
            pymtSchedule.Add
            (
                new Installment(InstallmentNumber: 6,
                                PaymentDueDate: new DateTime(2022, 9, 30),
                                Payment: 2855.37M,
                                Principal: 2741.00M,
                                Interest: 114.37M,
                                TotalInterestPaid: 920.22M,
                                RemainingBalance: 16788.00M)
            );
            pymtSchedule.Add
            (
                new Installment(InstallmentNumber: 7,
                                PaymentDueDate: new DateTime(2022, 10, 30),
                                Payment: 2855.37M,
                                Principal: 2757.00M,
                                Interest: 98.37M,
                                TotalInterestPaid: 1018.59M,
                                RemainingBalance: 14031.00M)
            );
            pymtSchedule.Add
            (
                new Installment(InstallmentNumber: 8,
                                PaymentDueDate: new DateTime(2022, 11, 30),
                                Payment: 2855.37M,
                                Principal: 2774.00M,
                                Interest: 81.37M,
                                TotalInterestPaid: 1099.96M,
                                RemainingBalance: 11257.00M)
            );
            pymtSchedule.Add
            (
                new Installment(InstallmentNumber: 9,
                                PaymentDueDate: new DateTime(2022, 12, 30),
                                Payment: 2855.37M,
                                Principal: 2790.00M,
                                Interest: 65.37M,
                                TotalInterestPaid: 1165.33M,
                                RemainingBalance: 8467.00M)
            );
            pymtSchedule.Add
            (
                new Installment(InstallmentNumber: 10,
                                PaymentDueDate: new DateTime(2023, 1, 30),
                                Payment: 2855.37M,
                                Principal: 2806.00M,
                                Interest: 49.37M,
                                TotalInterestPaid: 1214.70M,
                                RemainingBalance: 5661.00M)
            );
            pymtSchedule.Add
            (
                new Installment(InstallmentNumber: 11,
                                PaymentDueDate: new DateTime(2023, 2, 28),
                                Payment: 2855.37M,
                                Principal: 2822.00M,
                                Interest: 33.37M,
                                TotalInterestPaid: 1248.07M,
                                RemainingBalance: 2839.00M)
            );
            pymtSchedule.Add
            (
                new Installment(InstallmentNumber: 12,
                                PaymentDueDate: new DateTime(2023, 3, 30),
                                Payment: 2855.37M,
                                Principal: 2839.00M,
                                Interest: 16.37M,
                                TotalInterestPaid: 1264.44M,
                                RemainingBalance: 0M)
            );

            return pymtSchedule;
        }
    }
}