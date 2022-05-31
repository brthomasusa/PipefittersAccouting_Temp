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

        public static DividendDeclaration GetDividendDeclarationValidInfo()
            => new
            (
                EntityGuidID.Create(new Guid("1c967462-140a-4e08-9ba2-04ff760bb1d9")),
                EntityGuidID.Create(new Guid("264632b4-20bd-473f-9a9b-dd6f3b6ddbac")),
                DividendDeclarationDate.Create(new DateTime(2022, 6, 1)),
                DividendPerShare.Create(.01M),
                EntityGuidID.Create(new Guid("660bb318-649e-470d-9d2b-693bfb0b2744"))
            );

        public static DividendDeclaration GetDividendDeclarationDuplicateFields()
            => new
            (
                EntityGuidID.Create(new Guid("64fb1715-3eed-4953-b9fc-acd491e1317a")),
                EntityGuidID.Create(new Guid("264632b4-20bd-473f-9a9b-dd6f3b6ddbac")),
                DividendDeclarationDate.Create(new DateTime(2022, 4, 1)),
                DividendPerShare.Create(.09M),
                EntityGuidID.Create(new Guid("660bb318-649e-470d-9d2b-693bfb0b2744"))
            );

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
    }
}

/* StockSubscription  */
// EntityGuidID stockId,
// EntityGuidID financierId,
// StockIssueDate stockIssueDate,
// SharesIssured sharesIssured,
// PricePerShare pricePerShare,
// EntityGuidID userId

/*  DividendDeclaration   */
// EntityGuidID dividendId,
// EntityGuidID stockId,
// DividendDeclarationDate dividendDeclarationDate,
// DividendPerShare dividendPerShare,
// EntityGuidID userId

/* DividendDeclarationWriteModel */
// public Guid DividendId { get; set; }
// public Guid StockId { get; set; }
// public DateTime DividendDeclarationDate { get; set; }
// public decimal DividendPerShare { get; set; }
// public Guid UserId { get; set; }