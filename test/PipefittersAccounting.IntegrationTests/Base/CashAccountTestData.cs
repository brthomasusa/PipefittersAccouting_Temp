#pragma warning disable CS8625

using System;
using PipefittersAccounting.Core.CashManagement.CashAccountAggregate;
using PipefittersAccounting.Core.CashManagement.CashAccountAggregate.ValueObjects;
using PipefittersAccounting.Infrastructure.Interfaces.CashManagement;
using PipefittersAccounting.SharedKernel.CommonValueObjects;
using PipefittersAccounting.SharedModel.WriteModels.CashManagement;

namespace PipefittersAccounting.IntegrationTests.Base
{
    public class CashAccountTestData
    {
        public static CashAccount GetCashAccount(ICashAccountAggregateValidationService validationService)
            => new
            (
                EntityGuidID.Create(new Guid("e848ffef-dd33-4e2c-a4eb-26925c3713d1")),
                CashAccountTypeEnum.NonPayrollOperations,
                BankName.Create("BankOne"),
                CashAccountName.Create("Purchasing"),
                CashAccountNumber.Create("12345-56-5547"),
                RoutingTransitNumber.Create("787-896-1144"),
                DateOpened.Create(new DateTime(2022, 5, 3)),
                EntityGuidID.Create(new Guid("660bb318-649e-470d-9d2b-693bfb0b2744"))
            );

        public static CashAccount GetCashAccountWithDupeAcctNumber(ICashAccountAggregateValidationService validationService)
            => new
            (
                EntityGuidID.Create(new Guid("e848ffef-dd33-4e2c-a4eb-26925c3713d1")),
                CashAccountTypeEnum.NonPayrollOperations,
                BankName.Create("BankOne"),
                CashAccountName.Create("Purchasing"),
                CashAccountNumber.Create("XXXXX-XXXXXXX"),
                RoutingTransitNumber.Create("787-896-1144"),
                DateOpened.Create(new DateTime(2022, 5, 3)),
                EntityGuidID.Create(new Guid("660bb318-649e-470d-9d2b-693bfb0b2744"))
            );

        public static CashAccount GetCashAccountWithDuplicateCashAccountId(ICashAccountAggregateValidationService validationService)
            => new
            (
                EntityGuidID.Create(new Guid("c98ac84f-00bb-463d-9116-5828b2e9f718")),
                CashAccountTypeEnum.NonPayrollOperations,
                BankName.Create("BankOne"),
                CashAccountName.Create("Purchasing"),
                CashAccountNumber.Create("12345-56-5547"),
                RoutingTransitNumber.Create("787-896-1144"),
                DateOpened.Create(new DateTime(2022, 5, 3)),
                EntityGuidID.Create(new Guid("660bb318-649e-470d-9d2b-693bfb0b2744"))
            );

        public static CashTransactionWriteModel GetCreateCashAccountTransactionLoanProceedsInfo()
            => new()
            {
                CashAccountId = new Guid("6a7ed605-c02c-4ec8-89c4-eac6306c885e"),
                TransactionType = 2,
                TransactionDate = new DateTime(2022, 4, 20),
                TransactionAmount = 4000M,
                AgentId = new Guid("b49471a0-5c1e-4a4d-97e7-288fb0f6338a"),
                EventId = new Guid("17b447ea-90a7-45c3-9fc2-c4fb2ea71867"),
                CheckNumber = "2001",
                RemittanceAdvice = "ABCDE",
                UserId = new Guid("660bb318-649e-470d-9d2b-693bfb0b2744")
            };

        public static CashTransactionWriteModel GetCreateCashAccountTransactionInfoDuplicateLoanProceedsDeposit()
            => new()
            {
                CashAccountId = new Guid("6a7ed605-c02c-4ec8-89c4-eac6306c885e"),
                TransactionType = 2,
                TransactionDate = new DateTime(2022, 3, 20),
                TransactionAmount = 10000M,
                AgentId = new Guid("b49471a0-5c1e-4a4d-97e7-288fb0f6338a"),
                EventId = new Guid("1511c20b-6df0-4313-98a5-7c3561757dc2"),
                CheckNumber = "2001",
                RemittanceAdvice = "ABCDE",
                UserId = new Guid("660bb318-649e-470d-9d2b-693bfb0b2744")
            };

        public static CashTransactionWriteModel GetCreateCashAccountTransactionInfoLoanPymt()
            => new()
            {
                CashAccountId = new Guid("417f8a5f-60e7-411a-8e87-dfab0ae62589"),
                TransactionType = 4,
                TransactionDate = new DateTime(2022, 5, 2),
                TransactionAmount = 1370.54M,
                AgentId = new Guid("94b1d516-a1c3-4df8-ae85-be1f34966601"),
                EventId = new Guid("e4ca6c30-6fd7-44ea-89b5-e11ecfc5989b"),
                CheckNumber = "2311",
                RemittanceAdvice = "ABCDE",
                UserId = new Guid("660bb318-649e-470d-9d2b-693bfb0b2744")
            };

        public static CashTransactionWriteModel GetCreateCashAccountTransactionInfoLoanPymtDuplicate()
            => new()
            {
                CashAccountId = new Guid("417f8a5f-60e7-411a-8e87-dfab0ae62589"),
                TransactionType = 4,
                TransactionDate = new DateTime(2022, 5, 17),
                TransactionAmount = 1370.54M,
                AgentId = new Guid("94b1d516-a1c3-4df8-ae85-be1f34966601"),
                EventId = new Guid("97fa51e0-e02a-46c1-9f09-73f72a5519c9"),
                CheckNumber = "2312",
                RemittanceAdvice = "ABCDE",
                UserId = new Guid("660bb318-649e-470d-9d2b-693bfb0b2744")
            };

        public static CashTransactionWriteModel GetCreateCashAccountTransactionInfoLoanPymtNoProeedsDeposited()
            => new()
            {
                CashAccountId = new Guid("417f8a5f-60e7-411a-8e87-dfab0ae62589"),
                TransactionType = 4,
                TransactionDate = new DateTime(2022, 5, 17),
                TransactionAmount = 1100M,
                AgentId = new Guid("b49471a0-5c1e-4a4d-97e7-288fb0f6338a"),
                EventId = new Guid("0bd39edb-8da3-40f9-854f-b90e798b82c2"),
                CheckNumber = "2001",
                RemittanceAdvice = "ABCDE",
                UserId = new Guid("660bb318-649e-470d-9d2b-693bfb0b2744")
            };


        public static CashTransactionWriteModel GetCreateCashAccountTransactionStockIssueProceedsInfo()
            => new()
            {
                CashAccountId = new Guid("6a7ed605-c02c-4ec8-89c4-eac6306c885e"),
                TransactionType = 3,
                TransactionDate = new DateTime(2022, 5, 29),
                TransactionAmount = 5985M,
                AgentId = new Guid("12998229-7ede-4834-825a-0c55bde75695"),
                EventId = new Guid("971bb315-9d40-4c87-b43b-359b33c31354"),
                CheckNumber = "22001",
                RemittanceAdvice = "ABCDE",
                UserId = new Guid("660bb318-649e-470d-9d2b-693bfb0b2744")
            };

        public static CashTransactionWriteModel GetCreateCashAccountTransactionInfo_ProceedsAlreadyRcvd()
            => new()
            {
                CashAccountId = new Guid("6a7ed605-c02c-4ec8-89c4-eac6306c885e"),
                TransactionType = 3,
                TransactionDate = new DateTime(2022, 4, 4),
                TransactionAmount = 15625M,
                AgentId = new Guid("12998229-7ede-4834-825a-0c55bde75695"),
                EventId = new Guid("5997f125-bfca-4540-a144-01e444f6dc25"),
                CheckNumber = "22001",
                RemittanceAdvice = "ABCDE",
                UserId = new Guid("660bb318-649e-470d-9d2b-693bfb0b2744")
            };

        public static CashTransactionWriteModel GetCreateCashAccountTransactionInfoDividendPymt()
            => new()
            {
                CashAccountId = new Guid("417f8a5f-60e7-411a-8e87-dfab0ae62589"),
                TransactionType = 5,
                TransactionDate = new DateTime(2022, 6, 5),
                TransactionAmount = 100.00M,
                AgentId = new Guid("bf19cf34-f6ba-4fb2-b70e-ab19d3371886"),
                EventId = new Guid("ff0dc77f-7f80-426a-bc24-09d3c10a957f"),
                CheckNumber = "22311",
                RemittanceAdvice = "ABCDE",
                UserId = new Guid("660bb318-649e-470d-9d2b-693bfb0b2744")
            };

        public static CashTransactionWriteModel GetCreateCashAccountTransactionInfoDividendPymtPaid()
            => new()
            {
                CashAccountId = new Guid("417f8a5f-60e7-411a-8e87-dfab0ae62589"),
                TransactionType = 5,
                TransactionDate = new DateTime(2022, 6, 5),
                TransactionAmount = 100.00M,
                AgentId = new Guid("bf19cf34-f6ba-4fb2-b70e-ab19d3371886"),
                EventId = new Guid("ef21754e-51fb-4721-a04b-281cdfa8e66b"),
                CheckNumber = "22311",
                RemittanceAdvice = "ABCDE",
                UserId = new Guid("660bb318-649e-470d-9d2b-693bfb0b2744")
            };

        public static CashAccountWriteModel GetCreateCashAccountInfo()
            => new CashAccountWriteModel
            {
                CashAccountId = new Guid("210d34d7-7474-44e7-a90b-93998137917a"),
                CashAccountType = 2,
                BankName = "Big Bank",
                CashAccountName = "Party Party Party!",
                CashAccountNumber = "123456789",
                RoutingTransitNumber = "987654321",
                DateOpened = new DateTime(2022, 5, 3),
                UserId = new Guid("660bb318-649e-470d-9d2b-693bfb0b2744")
            };

        public static CashAccountWriteModel GetEditCashAccountInfo()
            => new CashAccountWriteModel
            {
                CashAccountId = new Guid("765ec2b0-406a-4e42-b831-c9aa63800e76"),
                CashAccountType = 2,
                BankName = "Big Bank",
                CashAccountName = "Party Party Party!",
                RoutingTransitNumber = "987654321",
                DateOpened = new DateTime(2022, 5, 3),
                UserId = new Guid("660bb318-649e-470d-9d2b-693bfb0b2744")
            };

        public static CashAccountWriteModel GetEditCashAccountInfoWithAcctTypeUpdate()
            => new CashAccountWriteModel
            {
                CashAccountId = new Guid("417f8a5f-60e7-411a-8e87-dfab0ae62589"),
                CashAccountType = 3,
                BankName = "First Bank and Trust",
                CashAccountName = "Primary Checking",
                RoutingTransitNumber = "987654321",
                DateOpened = new DateTime(2022, 5, 10),
                UserId = new Guid("660bb318-649e-470d-9d2b-693bfb0b2744")
            };

        public static CashAccountTransferWriteModel GetCreateCashAccountTransferInfo()
            => new CashAccountTransferWriteModel
            {
                CashTransferId = new Guid("b954a137-ab47-4f13-85ae-762a97944230"),
                SourceCashAccountId = new Guid("6a7ed605-c02c-4ec8-89c4-eac6306c885e"),
                DestinationCashAccountId = new Guid("c98ac84f-00bb-463d-9116-5828b2e9f718"),
                CashTransferDate = new DateTime(2022, 5, 20),
                CashTransferAmount = 19999.99M,
                UserId = new Guid("660bb318-649e-470d-9d2b-693bfb0b2744")
            };
    }
}