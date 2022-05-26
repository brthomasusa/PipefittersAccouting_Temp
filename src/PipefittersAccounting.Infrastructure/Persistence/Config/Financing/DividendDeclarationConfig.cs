using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PipefittersAccounting.Core.Financing.StockSubscriptionAggregate;
using PipefittersAccounting.Core.Financing.StockSubscriptionAggregate.ValueObjects;
using PipefittersAccounting.SharedKernel.CommonValueObjects;

namespace PipefittersAccounting.Infrastructure.Persistence.Config.Financing
{
    internal class DividendDeclarationConfig : IEntityTypeConfiguration<DividendDeclaration>
    {
        public void Configure(EntityTypeBuilder<DividendDeclaration> entity)
        {
            entity.ToTable("DividendDeclarations", schema: "Finance");
            entity.HasKey(e => e.Id);
            entity.Property(p => p.Id).HasColumnType("UNIQUEIDENTIFIER").HasColumnName("DividendId");
            entity.Property(p => p.StockId)
                .HasColumnType("UNIQUEIDENTIFIER")
                .HasColumnName("StockId").IsRequired();
            entity.Property(p => p.DividendDeclarationDate)
                .HasConversion(p => p.Value, p => DividendDeclarationDate.Create(p))
                .HasColumnType("DATETIME2(0)")
                .HasColumnName("DividendDeclarationDate")
                .IsRequired();
            entity.Property(p => p.DividendPerShare)
                .HasConversion(p => p.Value, p => DividendPerShare.Create(p))
                .HasColumnType("DECIMAL(18,2)")
                .HasColumnName("DividendPerShare")
                .IsRequired();


            entity.Property(p => p.UserId)
                .HasConversion(p => p.Value, p => EntityGuidID.Create(p))
                .HasColumnType("UNIQUEIDENTIFIER")
                .HasColumnName("UserId").IsRequired();
            entity.Property(e => e.CreatedDate).HasColumnType("datetime2(7)");
            entity.Property(e => e.LastModifiedDate).HasColumnType("datetime2(7)");
        }
    }
}