using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PipefittersAccounting.Core.Shared;
using PipefittersAccounting.Core.CashManagement.CashAccountAggregate;

namespace PipefittersAccounting.Infrastructure.Persistence.Config.Shared
{
    internal class EconomicResourceConfig : IEntityTypeConfiguration<EconomicResource>
    {
        public void Configure(EntityTypeBuilder<EconomicResource> entity)
        {
            entity.ToTable("EconomicResources", schema: "Shared");
            entity.HasKey(e => e.Id);
            entity.HasOne<CashAccount>().WithOne().HasForeignKey<CashAccount>("Id");

            entity.Property(p => p.Id).HasColumnType("UNIQUEIDENTIFIER").HasColumnName("ResourceId").ValueGeneratedNever();
            entity.Property(p => p.ResourceType).HasColumnType("int").HasColumnName("ResourceTypeId").IsRequired();
            entity.Property(e => e.CreatedDate).HasColumnType("datetime2(7)");
            entity.Property(e => e.LastModifiedDate).HasColumnType("datetime2(7)");
        }
    }
}