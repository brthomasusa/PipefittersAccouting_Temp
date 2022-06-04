#pragma warning disable CS8625

using System;
using PipefittersAccounting.Core.Financing.StockSubscriptionAggregate;
using PipefittersAccounting.Core.Financing.StockSubscriptionAggregate.ValueObjects;
using PipefittersAccounting.Infrastructure.Interfaces.Financing;
using PipefittersAccounting.SharedKernel.CommonValueObjects;
using PipefittersAccounting.SharedModel.WriteModels.Financing;

namespace PipefittersAccounting.IntegrationTests.Base
{
    public class StockSubscriptionTestData
    {
        public static StockSubscription CreateStockSubscriptionValidInfo()
            => new
            (
                EntityGuidID.Create(new Guid("75cece20-ba7c-4475-b211-15e37ac95e5b")),
                EntityGuidID.Create(new Guid("01da50f9-021b-4d03-853a-3fd2c95e207d")),
                StockIssueDate.Create(new DateTime(2022, 5, 27)),
                SharesIssured.Create(5700),
                PricePerShare.Create(1.00M),
                EntityGuidID.Create(new Guid("660bb318-649e-470d-9d2b-693bfb0b2744"))
            );

        public static StockSubscription CreateStockSubscriptionDuplicateStockId()
            => new
            (
                EntityGuidID.Create(new Guid("6632cec7-29c5-4ec3-a5a9-c82bf8f5eae3")),
                EntityGuidID.Create(new Guid("01da50f9-021b-4d03-853a-3fd2c95e207d")),
                StockIssueDate.Create(new DateTime(2022, 5, 27)),
                SharesIssured.Create(5700),
                PricePerShare.Create(1.00M),
                EntityGuidID.Create(new Guid("660bb318-649e-470d-9d2b-693bfb0b2744"))
            );

        public static StockSubscription CreateStockSubscriptionDuplicateFields()
            => new
            (
                EntityGuidID.Create(new Guid("64fb1715-3eed-4953-b9fc-acd491e1317a")),
                EntityGuidID.Create(new Guid("12998229-7ede-4834-825a-0c55bde75695")),
                StockIssueDate.Create(new DateTime(2022, 2, 1)),
                SharesIssured.Create(5000),
                PricePerShare.Create(3.00M),
                EntityGuidID.Create(new Guid("660bb318-649e-470d-9d2b-693bfb0b2744"))
            );

        public static StockSubscriptionWriteModel GetStockSubscriptionWriteModel_Create()
            => new StockSubscriptionWriteModel
            {
                StockId = new Guid("75cece20-ba7c-4475-b211-15e37ac95e5b"),
                FinancierId = new Guid("01da50f9-021b-4d03-853a-3fd2c95e207d"),
                StockIssueDate = new DateTime(2022, 6, 3),
                SharesIssued = 8700,
                PricePerShare = .95M,
                UserId = new Guid("660bb318-649e-470d-9d2b-693bfb0b2744")
            };

        public static StockSubscriptionWriteModel GetStockSubscriptionWriteModel_ExistingWithNoDeposit()
            => new StockSubscriptionWriteModel
            {
                StockId = new Guid("971bb315-9d40-4c87-b43b-359b33c31354"),
                FinancierId = new Guid("12998229-7ede-4834-825a-0c55bde75695"),
                StockIssueDate = new DateTime(2022, 5, 27),
                SharesIssued = 5700,
                PricePerShare = 1.05M,
                UserId = new Guid("660bb318-649e-470d-9d2b-693bfb0b2744")
            };

        public static StockSubscriptionWriteModel GetStockSubscriptionWriteModel_ExistingWithDeposit()
            => new StockSubscriptionWriteModel
            {
                StockId = new Guid("fb39b013-1633-4479-8186-9f9b240b5727"),
                FinancierId = new Guid("b49471a0-5c1e-4a4d-97e7-288fb0f6338a"),
                StockIssueDate = new DateTime(2022, 1, 11),
                SharesIssued = 12000,
                PricePerShare = 1.00M,
                UserId = new Guid("660bb318-649e-470d-9d2b-693bfb0b2744")
            };

        public static DividendDeclaration GetDividendDeclarationValidInfo()
            => new
            (
                EntityGuidID.Create(new Guid("1c967462-140a-4e08-9ba2-04ff760bb1d9")),
                EntityGuidID.Create(new Guid("264632b4-20bd-473f-9a9b-dd6f3b6ddbac")),
                DividendDeclarationDate.Create(new DateTime(2022, 6, 1)),
                DividendPerShare.Create(.01M),
                EntityGuidID.Create(new Guid("660bb318-649e-470d-9d2b-693bfb0b2744"))
            );

        public static DividendDeclarationWriteModel GetDividendDeclarationWriteModelForCreate()
            => new DividendDeclarationWriteModel
            {
                DividendId = new Guid("1c967462-140a-4e08-9ba2-04ff760bb1d9"),
                StockId = new Guid("264632b4-20bd-473f-9a9b-dd6f3b6ddbac"),
                DividendDeclarationDate = new DateTime(2022, 6, 1),
                DividendPerShare = .01M,
                UserId = new Guid("660bb318-649e-470d-9d2b-693bfb0b2744")
            };

        public static DividendDeclarationWriteModel GetDividendDeclarationWriteModelForEditNotPaid()
            => new DividendDeclarationWriteModel
            {
                DividendId = new Guid("ff0dc77f-7f80-426a-bc24-09d3c10a957f"),
                StockId = new Guid("62d6e2e6-215d-4157-b7ec-1ba9b137c770"),
                DividendDeclarationDate = new DateTime(2022, 6, 3),
                DividendPerShare = .01M,
                UserId = new Guid("660bb318-649e-470d-9d2b-693bfb0b2744")
            };

        public static DividendDeclarationWriteModel GetDividendDeclarationWriteModelForEditPaid()
            => new DividendDeclarationWriteModel
            {
                DividendId = new Guid("2558ab00-118c-4b67-a6d0-1b9888f841bc"),
                StockId = new Guid("264632b4-20bd-473f-9a9b-dd6f3b6ddbac"),
                DividendDeclarationDate = new DateTime(2022, 3, 1),
                DividendPerShare = .09M,
                UserId = new Guid("660bb318-649e-470d-9d2b-693bfb0b2744")
            };
    }
}

