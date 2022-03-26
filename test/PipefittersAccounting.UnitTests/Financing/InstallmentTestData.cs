using System;
using System.Collections.Generic;
using PipefittersAccounting.Core.Financing.LoanAgreementAggregate.Components;

namespace PipefittersAccounting.UnitTests.Financing
{
    public class InstallmentTestData
    {
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

        public static SortedDictionary<int, Installment> GetInstallmentsInvalidInstallmentNumbers()
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
                new Installment(InstallmentNumber: 5,                           // Invalid installment number
                                PaymentDueDate: new DateTime(2022, 6, 15),
                                Payment: 455.65M,
                                Principal: 451.00M,
                                Interest: 4.65M,
                                TotalInterestPaid: 19.95M,
                                RemainingBalance: 453.00M)
            );
            pymtSchedule.Add
            (
                new Installment(InstallmentNumber: 6,
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

        public static SortedDictionary<int, Installment> GetInstallmentsInvalidPymtDates()
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
    }
}