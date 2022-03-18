using Microsoft.EntityFrameworkCore;
using PipefittersAccounting.Core.Shared;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PipefittersAccounting.Core.Financing.LoanAgreementAggregate;

namespace PipefittersAccounting.Infrastructure.Persistence.Config.Shared
{
    internal class EconomicEventConfig : IEntityTypeConfiguration<EconomicEvent>
    {
        public void Configure(EntityTypeBuilder<EconomicEvent> entity)
        {
            entity.ToTable("EconomicEvents", schema: "Shared");
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.EventType, "idx_EconomicEvents$EventTypeId");

            entity.Property(p => p.Id).HasColumnType("UNIQUEIDENTIFIER").HasColumnName("EventId").ValueGeneratedNever();
            entity.Property(p => p.EventType).HasColumnType("int").HasColumnName("EventTypeId").IsRequired();
            entity.HasOne(e => e.LoanAgreement).WithOne().HasForeignKey<LoanAgreement>("Id");
            entity.HasOne(e => e.LoanInstallment).WithOne().HasForeignKey<LoanInstallment>("Id");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime2(7)");
            entity.Property(e => e.LastModifiedDate).HasColumnType("datetime2(7)");
        }
    }
}