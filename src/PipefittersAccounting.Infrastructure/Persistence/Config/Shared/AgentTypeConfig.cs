using Microsoft.EntityFrameworkCore;
using PipefittersAccounting.Core.Shared;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PipefittersAccounting.Infrastructure.Persistence.Config.Shared
{
    internal class AgentTypeConfig : IEntityTypeConfiguration<AgentType>
    {
        public void Configure(EntityTypeBuilder<AgentType> entity)
        {
            entity.ToTable("ExternalAgentTypes", schema: "Shared");
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.AgentTypeName, "idx_ExternalAgentTypes$AgentTypeName").IsUnique();

            entity.Property(p => p.Id).HasColumnType("integer").HasColumnName("AgentTypeId");
            entity.Property(p => p.AgentTypeName).HasColumnType("NVARCHAR(50)").HasColumnName("AgentTypeName").IsRequired();
            entity.Property(e => e.CreatedDate).HasColumnType("datetime2(7)");
            entity.Property(e => e.LastModifiedDate).HasColumnType("datetime2(7)");
        }
    }
}