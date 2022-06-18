using Microsoft.EntityFrameworkCore;
using PipefittersAccounting.Core.Shared;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PipefittersAccounting.Core.Financing.CashAccountAggregate;
using PipefittersAccounting.Core.Financing.LoanAgreementAggregate;
using PipefittersAccounting.Core.Financing.StockSubscriptionAggregate;
using PipefittersAccounting.Core.HumanResources.EmployeeAggregate;

namespace PipefittersAccounting.Infrastructure.Persistence.Config.Shared
{
    internal class EconomicEventConfig : IEntityTypeConfiguration<EconomicEvent>
    {
        public void Configure(EntityTypeBuilder<EconomicEvent> entity)
        {
            entity.ToTable("EconomicEvents", schema: "Shared");
            entity.HasKey(e => e.Id);
            entity.HasOne<CashTransfer>().WithOne().HasForeignKey<CashTransfer>("Id");
            entity.HasOne<LoanAgreement>().WithOne().HasForeignKey<LoanAgreement>("Id");
            entity.HasOne<LoanInstallment>().WithOne().HasForeignKey<LoanInstallment>("Id");
            entity.HasOne<StockSubscription>().WithOne().HasForeignKey<StockSubscription>("Id");
            entity.HasOne<DividendDeclaration>().WithOne().HasForeignKey<DividendDeclaration>("Id");
            entity.HasOne<TimeCard>().WithOne().HasForeignKey<TimeCard>("Id");

            entity.Property(p => p.Id).HasColumnType("UNIQUEIDENTIFIER").HasColumnName("EventId").ValueGeneratedNever();
            entity.Property(p => p.EventType).HasColumnType("int").HasColumnName("EventTypeId").IsRequired();
            entity.Property(e => e.CreatedDate).HasColumnType("datetime2(7)");
            entity.Property(e => e.LastModifiedDate).HasColumnType("datetime2(7)");
        }
    }
}
