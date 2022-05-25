#pragma warning disable CS8625

using System;
using PipefittersAccounting.Core.Financing.CashAccountAggregate;
using PipefittersAccounting.Core.Financing.CashAccountAggregate.ValueObjects;
using PipefittersAccounting.Core.Interfaces.Financing;
using PipefittersAccounting.Core.Shared;
using PipefittersAccounting.SharedKernel.CommonValueObjects;

namespace PipefittersAccounting.UnitTests.Financing
{
    public class CashAccountTestData
    {
        public static CashAccount GetCashAccount()
            => new
            (
                EntityGuidID.Create(new Guid("e848ffef-dd33-4e2c-a4eb-26925c3713d1")),
                CashAccountTypeEnum.NonPayrollOperations,
                BankName.Create("BankOne"),
                CashAccountName.Create("Purchasing"),
                CashAccountNumber.Create("12345-56-5547"),
                RoutingTransitNumber.Create("787896114"),
                DateOpened.Create(new DateTime(2022, 4, 17)),
                EntityGuidID.Create(new Guid("660bb318-649e-470d-9d2b-693bfb0b2744"))
            );

        private static CashTransaction GetCashTransactionLoanProceeds()
            => new
            (
                CashTransactionTypeEnum.CashReceiptDebtIssueProceeds,
                EntityGuidID.Create(new Guid("6a7ed605-c02c-4ec8-89c4-eac6306c885e")),  // CashAccount Id financing proceeds
                CashTransactionDate.Create(new DateTime(2022, 4, 15)),                  // Loan date
                CashTransactionAmount.Create(4000M),                                    // LoanAmount
                EntityGuidID.Create(new Guid("b49471a0-5c1e-4a4d-97e7-288fb0f6338a")),  // AgentId financier
                EntityGuidID.Create(new Guid("17b447ea-90a7-45c3-9fc2-c4fb2ea71867")),  // EventId loan agreement id
                CheckNumber.Create("2001"),
                RemittanceAdvice.Create("ABCDE"),
                EntityGuidID.Create(new Guid("660bb318-649e-470d-9d2b-693bfb0b2744"))
            );

        public static CashTransaction GetCashTransactionLoanProceedsDuplicateDeposit()
            => new
            (
                CashTransactionTypeEnum.CashReceiptDebtIssueProceeds,
                EntityGuidID.Create(new Guid("6a7ed605-c02c-4ec8-89c4-eac6306c885e")),  // CashAccount Id financing proceeds
                CashTransactionDate.Create(new DateTime(2022, 1, 5)),
                CashTransactionAmount.Create(25000M),
                EntityGuidID.Create(new Guid("12998229-7ede-4834-825a-0c55bde75695")),  // AgentId financier
                EntityGuidID.Create(new Guid("41ca2b0a-0ed5-478b-9109-5dfda5b2eba1")),  // EventId loan agreement id
                CheckNumber.Create("2001"),
                RemittanceAdvice.Create("ABCDE"),
                EntityGuidID.Create(new Guid("660bb318-649e-470d-9d2b-693bfb0b2744"))
            );

        public static CashTransaction GetCashTransactionLoanInstallmentPymt()
            => new
            (
                CashTransactionTypeEnum.CashDisbursementLoanPayment,
                EntityGuidID.Create(new Guid("417f8a5f-60e7-411a-8e87-dfab0ae62589")),  // CashAccount Id primary checking
                CashTransactionDate.Create(new DateTime(2022, 7, 15)),                  // Payment due date
                CashTransactionAmount.Create(1100M),                                    // Equal Monthly Installment
                EntityGuidID.Create(new Guid("b49471a0-5c1e-4a4d-97e7-288fb0f6338a")),  // AgentId financier
                EntityGuidID.Create(new Guid("0bd39edb-8da3-40f9-854f-b90e798b82c2")),  // EventId loan installment id
                CheckNumber.Create("2011"),
                RemittanceAdvice.Create("ABCDE"),
                EntityGuidID.Create(new Guid("660bb318-649e-470d-9d2b-693bfb0b2744"))
            );

        public static CashTransaction GetCashTransactionLoanInstallmentPymtAlreadyPaid()
            => new
            (
                CashTransactionTypeEnum.CashDisbursementLoanPayment,
                EntityGuidID.Create(new Guid("417f8a5f-60e7-411a-8e87-dfab0ae62589")),  // CashAccount Id primary checking
                CashTransactionDate.Create(new DateTime(2022, 3, 5)),                  // Payment due date
                CashTransactionAmount.Create(2186.28M),                                    // Equal Monthly Installment
                EntityGuidID.Create(new Guid("12998229-7ede-4834-825a-0c55bde75695")),  // AgentId financier
                EntityGuidID.Create(new Guid("f479f59a-5001-47af-9d6c-2eae07077490")),  // EventId loan installment id
                CheckNumber.Create("2011"),
                RemittanceAdvice.Create("ABCDE"),
                EntityGuidID.Create(new Guid("660bb318-649e-470d-9d2b-693bfb0b2744"))
            );
    }
}