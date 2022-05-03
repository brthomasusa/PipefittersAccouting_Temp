#pragma warning disable CS8602

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PipefittersAccounting.Core.Financing.CashAccountAggregate;
using PipefittersAccounting.Core.Financing.CashAccountAggregate.ValueObjects;

namespace PipefittersAccounting.Infrastructure.Persistence.Config.Financing
{
    public class CashTransactionConfig : IEntityTypeConfiguration<CashTransaction>
    {
        public void Configure(EntityTypeBuilder<CashTransaction> entity)
        {
            entity.ToTable("CashAccountTransactions", schema: "Finance");
            entity.HasKey(e => e.Id);

            entity.Property(p => p.Id).HasColumnType("INT").HasColumnName("CashTransactionId");
            entity.Property(p => p.CashAccountId).HasColumnType("UNIQUEIDENTIFIER").HasColumnName("CashAccountId").IsRequired();
            entity.Property(p => p.CashTransactionType)
                .HasColumnType("INT")
                .HasColumnName("CashTransactionTypeId")
                .IsRequired();
            entity.Property(p => p.CashTransactionDate)
                .HasConversion(p => p.Value, p => CashTransactionDate.Create(p))
                .HasColumnType("datetime2(0)")
                .HasColumnName("CashAcctTransactionDate")
                .IsRequired();
            entity.Property(p => p.CashTransactionAmount)
                .HasConversion(p => p.Value, p => CashTransactionAmount.Create(p))
                .HasColumnType("DECIMAL(18,2)")
                .HasColumnName("CashAcctTransactionAmount")
                .IsRequired();
            entity.Property(p => p.AgentId).HasColumnType("UNIQUEIDENTIFIER").HasColumnName("AgentId").IsRequired();
            entity.Property(p => p.EventId).HasColumnType("UNIQUEIDENTIFIER").HasColumnName("EventId").IsRequired();
            entity.Property(p => p.CheckNumber)
                .HasConversion(p => p.Value, p => CheckNumber.Create(p))
                .HasColumnType("NVARCHAR(25)")
                .HasColumnName("CheckNumber")
                .IsRequired();
            entity.Property(p => p.RemittanceAdvice)
                .HasConversion(p => p.Value, p => RemittanceAdvice.Create(p))
                .HasColumnType("NVARCHAR(50)")
                .HasColumnName("RemittanceAdvice");

            entity.Property(p => p.UserId).HasColumnType("UNIQUEIDENTIFIER").HasColumnName("UserId").IsRequired();
            entity.Property(e => e.CreatedDate).HasColumnType("datetime2(7)");
            entity.Property(e => e.LastModifiedDate).HasColumnType("datetime2(7)");
        }
    }
}