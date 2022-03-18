using System;
using Microsoft.EntityFrameworkCore;
using PipefittersAccounting.Core.Shared;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PipefittersAccounting.Core.Financing.LoanAgreementAggregate;

namespace PipefittersAccounting.Infrastructure.Persistence.Config.Financing
{
    public class LoanPaymentConfig : IEntityTypeConfiguration<LoanPayment>
    {
        public void Configure(EntityTypeBuilder<LoanPayment> entity)
        {
            entity.ToTable("LoanPayments", schema: "Finance");
            entity.HasKey(e => e.Id);

            entity.Property(p => p.Id).HasColumnType("UNIQUEIDENTIFIER").HasColumnName("LoanPaymentId");
            entity.Property(p => p.LoanId).HasColumnType("UNIQUEIDENTIFIER").HasColumnName("LoanId").IsRequired();
            entity.Property(p => p.PaymentNumber)
                .HasConversion(p => p.Value, p => PaymentNumber.Create(p))
                .HasColumnType("int")
                .HasColumnName("PaymentNumber")
                .IsRequired();
            entity.Property(p => p.PaymentDueDate)
                .HasConversion(p => p.Value, p => PaymentDueDate.Create(p))
                .HasColumnType("DATETIME2(0)")
                .HasColumnName("PaymentDueDate")
                .IsRequired();
            entity.Property(p => p.LoanPrincipalAmount)
                .HasConversion(p => p.Value, p => LoanPrincipalAmount.Create(p))
                .HasColumnType("DECIMAL(18,2)")
                .HasColumnName("PrincipalAmount")
                .IsRequired();
            entity.Property(p => p.LoanInterestAmount)
                .HasConversion(p => p.Value, p => LoanInterestAmount.Create(p))
                .HasColumnType("DECIMAL(18,2)")
                .HasColumnName("InterestAmount")
                .IsRequired();
            entity.Property(p => p.LoanPrincipalRemaining)
                .HasConversion(p => p.Value, p => LoanPrincipalRemaining.Create(p))
                .HasColumnType("DECIMAL(18,2)")
                .HasColumnName("PrincipalRemaining")
                .IsRequired();
            entity.Property(p => p.UserId).HasColumnType("UNIQUEIDENTIFIER").HasColumnName("UserId").IsRequired();
            entity.Property(e => e.CreatedDate).HasColumnType("datetime2(7)");
            entity.Property(e => e.LastModifiedDate).HasColumnType("datetime2(7)");
        }
    }
}