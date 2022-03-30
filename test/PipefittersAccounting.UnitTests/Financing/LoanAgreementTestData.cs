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
                MaturityDate.Create(new DateTime(2022, 10, 15)),
                NumberOfInstallments.Create(12),
                EntityGuidID.Create(new Guid("660bb318-649e-470d-9d2b-693bfb0b2744"))
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

        public static SortedDictionary<int, Installment> GetInstallmentsValidInfo()
        {
            List<Installment> pymtSchedule = new();
            SortedDictionary<int, Installment> sortedDictionary = new();

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

            pymtSchedule.ForEach(item => sortedDictionary.Add(item.InstallmentNumber, item));

            return sortedDictionary;
        }

        public static SortedDictionary<int, Installment> GetInstallmentsTooFewInstallments()
        {
            List<Installment> pymtSchedule = new();
            SortedDictionary<int, Installment> sortedDictionary = new();

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

            pymtSchedule.ForEach(item => sortedDictionary.Add(item.InstallmentNumber, item));

            return sortedDictionary;
        }

        public static SortedDictionary<int, Installment> GetInstallmentsTooManyInstallments()
        {
            List<Installment> pymtSchedule = new();
            SortedDictionary<int, Installment> sortedDictionary = new();

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

            pymtSchedule.ForEach(item => sortedDictionary.Add(item.InstallmentNumber, item));

            return sortedDictionary;
        }

        public static SortedDictionary<int, Installment> GetInstallmentsDictIndexDoesNotStartAtOne()
        {
            List<Installment> pymtSchedule = new();
            SortedDictionary<int, Installment> sortedDictionary = new();

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

            pymtSchedule.ForEach(item => sortedDictionary.Add(item.InstallmentNumber + 2, item));

            return sortedDictionary;
        }
    }
}