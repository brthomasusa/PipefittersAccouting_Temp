using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PipefittersAccounting.Core.Financing.LoanAgreementAggregate;
using PipefittersAccounting.Core.Financing.LoanAgreementAggregate.ValueObjects;

namespace PipefittersAccounting.Infrastructure.Persistence.Config.Financing
{
    public class LoanAgreementConfig : IEntityTypeConfiguration<LoanAgreement>
    {
        public void Configure(EntityTypeBuilder<LoanAgreement> entity)
        {
            entity.ToTable("LoanAgreements", schema: "Finance");
            entity.HasKey(e => e.Id);
            entity.HasMany<LoanInstallment>(p => p.LoanAmortizationTable).WithOne().HasForeignKey(p => p.LoanId);

            entity.Property(p => p.Id).HasColumnType("UNIQUEIDENTIFIER").HasColumnName("LoanId").ValueGeneratedNever();
            entity.Property(p => p.FinancierId).HasColumnType("UNIQUEIDENTIFIER").HasColumnName("FinancierId").IsRequired();
            entity.Property(p => p.LoanAmount)
                .HasConversion(p => p.Value, p => LoanAmount.Create(p))
                .HasColumnType("DECIMAL(18,2)")
                .HasColumnName("LoanAmount")
                .IsRequired();
            entity.Property(p => p.InterestRate)
                .HasConversion(p => p.Value, p => InterestRate.Create(p))
                .HasColumnType("NUMERIC(9,6)")
                .HasColumnName("InterestRate")
                .IsRequired();
            entity.Property(p => p.LoanDate)
                .HasConversion(p => p.Value, p => LoanDate.Create(p))
                .HasColumnType("DATETIME2(0)")
                .HasColumnName("LoanDate")
                .IsRequired();
            entity.Property(p => p.MaturityDate)
                .HasConversion(p => p.Value, p => MaturityDate.Create(p))
                .HasColumnType("DATETIME2(0)")
                .HasColumnName("MaturityDate")
                .IsRequired();
            entity.Property(p => p.NumberOfInstallments)
                .HasConversion(p => p.Value, p => NumberOfInstallments.Create(p))
                .HasColumnType("INT")
                .HasColumnName("NumberOfInstallments")
                .IsRequired();
            // entity.Property(p => p.LoanAmortizationTable).HasField("_loanAmortizationSchedule");
            entity.Property(p => p.UserId).HasColumnType("UNIQUEIDENTIFIER").HasColumnName("UserId").IsRequired();
            entity.Property(e => e.CreatedDate).HasColumnType("datetime2(7)");
            entity.Property(e => e.LastModifiedDate).HasColumnType("datetime2(7)");
        }
    }
}