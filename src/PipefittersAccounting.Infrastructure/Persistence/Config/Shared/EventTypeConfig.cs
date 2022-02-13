using Microsoft.EntityFrameworkCore;
using PipefittersAccounting.Core.Shared;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PipefittersAccounting.Infrastructure.Persistence.Config.Shared
{
    internal class EventTypeConfig : IEntityTypeConfiguration<EventType>
    {
        public void Configure(EntityTypeBuilder<EventType> entity)
        {
            entity.ToTable("EconomicEventTypes", schema: "Shared");
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.EventTypeName, "idx_EconomicEventTypes$EventTypeName").IsUnique();

            entity.Property(p => p.Id).HasColumnType("integer").HasColumnName("EventTypeId");
            entity.Property(p => p.EventTypeName).HasColumnType("NVARCHAR(50)").HasColumnName("EventTypeName").IsRequired();
            entity.Property(e => e.CreatedDate).HasColumnType("datetime2(7)");
            entity.Property(e => e.LastModifiedDate).HasColumnType("datetime2(7)");
        }
    }
}