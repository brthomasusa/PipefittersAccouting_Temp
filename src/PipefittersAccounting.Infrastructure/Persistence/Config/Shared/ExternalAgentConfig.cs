using Microsoft.EntityFrameworkCore;
using PipefittersAccounting.Core.Shared;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PipefittersAccounting.Core.HumanResources.EmployeeAggregate;
using PipefittersAccounting.Core.Financing.FinancierAggregate;

namespace PipefittersAccounting.Infrastructure.Persistence.Config.Shared
{
    internal class ExternalAgentConfig : IEntityTypeConfiguration<ExternalAgent>
    {
        public void Configure(EntityTypeBuilder<ExternalAgent> entity)
        {
            entity.ToTable("ExternalAgents", schema: "Shared");
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.AgentType, "idx_ExternalAgentTypes$AgentTypeId");
            entity.HasOne(e => e.Employee).WithOne(e => e.ExternalAgent).HasForeignKey<Employee>(e => e.Id);
            entity.HasOne(e => e.Financier).WithOne(e => e.ExternalAgent).HasForeignKey<Financier>(e => e.Id);

            entity.Property(p => p.Id).HasColumnType("UNIQUEIDENTIFIER").HasColumnName("AgentId").ValueGeneratedNever();
            entity.Property(p => p.AgentType).HasColumnType("int").HasColumnName("AgentTypeId").IsRequired();
            entity.Property(e => e.CreatedDate).HasColumnType("datetime2(7)");
            entity.Property(e => e.LastModifiedDate).HasColumnType("datetime2(7)");
        }
    }
}