#pragma warning disable CS8602

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PipefittersAccounting.Core.Financing.CashAccountAggregate;
using PipefittersAccounting.Core.Financing.CashAccountAggregate.ValueObjects;

namespace PipefittersAccounting.Infrastructure.Persistence.Config.Financing
{
    public class CashTransferConfig : IEntityTypeConfiguration<CashTransfer>
    {
        public void Configure(EntityTypeBuilder<CashTransfer> entity)
        {
            entity.ToTable("CashTransfers", schema: "CashManagement");
            entity.HasKey(e => e.Id);

            entity.Property(p => p.Id).HasColumnType("UNIQUEIDENTIFIER").HasColumnName("CashTransferId");
            entity.Property(p => p.SourceCashAccountId).HasColumnType("UNIQUEIDENTIFIER").HasColumnName("SourceCashAccountId").IsRequired();
            entity.Property(p => p.DestintaionCashAccountId).HasColumnType("UNIQUEIDENTIFIER").HasColumnName("DestinationCashAccountId").IsRequired();
            entity.Property(p => p.TransferAmount)
                .HasConversion(p => p.Value, p => CashTransactionAmount.Create(p))
                .HasColumnType("DECIMAL(18,2)")
                .HasColumnName("CashTransferAmount")
                .IsRequired();
            entity.Property(p => p.TransactionDate)
                .HasConversion(p => p.Value, p => CashTransactionDate.Create(p))
                .HasColumnType("datetime2(0)")
                .HasColumnName("CashTransferDate")
                .IsRequired();
            entity.Property(p => p.UserId).HasColumnType("UNIQUEIDENTIFIER").HasColumnName("UserId").IsRequired();
            entity.Property(e => e.CreatedDate).HasColumnType("datetime2(7)");
            entity.Property(e => e.LastModifiedDate).HasColumnType("datetime2(7)");
        }
    }
}