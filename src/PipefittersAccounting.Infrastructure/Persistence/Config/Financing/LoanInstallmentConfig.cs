using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PipefittersAccounting.Core.Financing.LoanAgreementAggregate;
using PipefittersAccounting.Core.Financing.LoanAgreementAggregate.ValueObjects;

namespace PipefittersAccounting.Infrastructure.Persistence.Config.Financing
{
    public class LoanPaymentConfig : IEntityTypeConfiguration<LoanInstallment>
    {
        public void Configure(EntityTypeBuilder<LoanInstallment> entity)
        {
            entity.ToTable("LoanInstallments", schema: "Finance");
            entity.HasKey(e => e.Id);

            entity.Property(p => p.Id).HasColumnType("UNIQUEIDENTIFIER").HasColumnName("LoanInstallmentId");
            entity.Property(p => p.LoanId).HasColumnType("UNIQUEIDENTIFIER").HasColumnName("LoanId").IsRequired();
            entity.Property(p => p.InstallmentNumber)
                .HasConversion(p => p.Value, p => InstallmentNumber.Create(p))
                .HasColumnType("int")
                .HasColumnName("InstallmentNumber")
                .IsRequired();
            entity.Property(p => p.PaymentDueDate)
                .HasConversion(p => p.Value, p => PaymentDueDate.Create(p))
                .HasColumnType("DATETIME2(0)")
                .HasColumnName("PaymentDueDate")
                .IsRequired();
            entity.Property(p => p.EqualMonthlyInstallment)
                .HasConversion(p => p.Value, p => EqualMonthlyInstallment.Create(p))
                .HasColumnType("DECIMAL(18,2)")
                .HasColumnName("EqualMonthlyInstallment")
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
            entity.Property(e => e.CreatedDate).HasColumnType("datetime2(7)");
            entity.Property(e => e.LastModifiedDate).HasColumnType("datetime2(7)");
        }
    }
}