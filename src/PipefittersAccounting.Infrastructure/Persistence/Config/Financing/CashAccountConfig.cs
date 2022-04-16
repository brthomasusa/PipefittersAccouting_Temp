using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PipefittersAccounting.Core.Financing.CashAccountAggregate;
using PipefittersAccounting.Core.Financing.CashAccountAggregate.ValueObjects;

namespace PipefittersAccounting.Infrastructure.Persistence.Config.Financing
{
    public class CashAccountConfig : IEntityTypeConfiguration<CashAccount>
    {
        public void Configure(EntityTypeBuilder<CashAccount> entity)
        {
            entity.ToTable("CashAccounts", schema: "Finance");
            entity.HasKey(e => e.Id);
            entity.HasMany<CashTransaction>(p => p.CashTransactions).WithOne().HasForeignKey(p => p.CashAccountId);

            entity.Property(p => p.Id).HasColumnType("UNIQUEIDENTIFIER").HasColumnName("CashAccountId");
            entity.Property(p => p.BankName)
                .HasConversion(p => p.Value, p => BankName.Create(p))
                .HasColumnType("NVARCHAR(50)")
                .HasColumnName("BankName")
                .IsRequired();
            entity.Property(p => p.CashAccountName)
                .HasConversion(p => p.Value, p => CashAccountName.Create(p))
                .HasColumnType("NVARCHAR(50)")
                .HasColumnName("AccountName")
                .IsRequired();
            entity.Property(p => p.CashAccountNumber)
                .HasConversion(p => p.Value, p => CashAccountNumber.Create(p))
                .HasColumnType("NVARCHAR(50)")
                .HasColumnName("AccountNumber")
                .IsRequired();
            entity.Property(p => p.RoutingTransitNumber)
                .HasConversion(p => p.Value, p => RoutingTransitNumber.Create(p))
                .HasColumnType("NCHAR(9)")
                .HasColumnName("RoutingTransitNumber")
                .IsRequired();
            entity.Property(p => p.DateOpened)
                .HasConversion(p => p.Value, p => DateOpened.Create(p))
                .HasColumnType("datetime2(0)")
                .HasColumnName("DateOpened")
                .IsRequired();
            entity.Property(p => p.UserId).HasColumnType("UNIQUEIDENTIFIER").HasColumnName("UserId").IsRequired();
            entity.Property(e => e.CreatedDate).HasColumnType("datetime2(7)");
            entity.Property(e => e.LastModifiedDate).HasColumnType("datetime2(7)");
        }
    }
}