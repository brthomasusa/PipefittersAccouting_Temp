using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PipefittersAccounting.UI.Sqlite
{
    public class LoanInstallmentModelConfig : IEntityTypeConfiguration<LoanInstallmentModel>
    {
        public void Configure(EntityTypeBuilder<LoanInstallmentModel> entity)
        {
            entity.ToTable("LoanInstallments");
            entity.HasKey(e => e.InstallmentNumber);
            entity.HasIndex(e => e.PaymentDueDate).IsUnique();

            entity.Property(p => p.InstallmentNumber).HasColumnType("int").HasColumnName("InstallmentNumber").ValueGeneratedNever();
            entity.Property(p => p.PaymentDueDate).HasColumnType("datetime2(0)").HasColumnName("PaymentDueDate").IsRequired();
            entity.Property(p => p.PaymentAmount).HasColumnType("DECIMAL(18,2)").HasColumnName("PaymentAmount").IsRequired();
            entity.Property(p => p.PrincipalPymtAmount).HasColumnType("DECIMAL(18,2)").HasColumnName("PrincipalPymtAmount").IsRequired();
            entity.Property(p => p.InterestPymtAmount).HasColumnType("DECIMAL(18,2)").HasColumnName("InterestPymtAmount").IsRequired();
            entity.Property(p => p.PrincipalRemaining).HasColumnType("DECIMAL(18,2)").HasColumnName("PrincipalRemaining");
        }
    }
}