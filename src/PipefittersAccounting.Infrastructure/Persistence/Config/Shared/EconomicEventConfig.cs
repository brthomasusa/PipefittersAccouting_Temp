using Microsoft.EntityFrameworkCore;
using PipefittersAccounting.Core.Shared;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PipefittersAccounting.Infrastructure.Persistence.Config.Shared
{
    internal class EconomicEventConfig : IEntityTypeConfiguration<EconomicEvent>
    {
        public void Configure(EntityTypeBuilder<EconomicEvent> entity)
        {
            entity.ToTable("EconomicEvents", schema: "Shared");
            entity.HasKey(e => e.Id);
            entity.Property(p => p.Id).HasColumnType("UNIQUEIDENTIFIER").HasColumnName("EventId");
            entity.Property(p => p.EventType).HasColumnType("int").HasColumnName("EventTypeId").IsRequired();
            // entity.HasOne(e => e.Financier).WithOne(e => e.ExternalAgent).HasForeignKey<Financier>(e => e.Id);
            entity.Property(e => e.CreatedDate)
                .HasColumnType("datetime2(7)")
                .ValueGeneratedOnAdd()
                .HasDefaultValueSql("sysdatetime()");
            entity.Property(e => e.LastModifiedDate).HasColumnType("datetime2(7)");
        }
    }
}