using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PipefittersAccounting.Core.HumanResources.EmployeeAggregate;
using PipefittersAccounting.Core.HumanResources.EmployeeAggregate.ValueObjects;
using PipefittersAccounting.SharedKernel.CommonValueObjects;

namespace PipefittersAccounting.Infrastructure.Persistence.Config.HumanResources
{
    internal class TimeCardConfig : IEntityTypeConfiguration<TimeCard>
    {
        public void Configure(EntityTypeBuilder<TimeCard> entity)
        {
            entity.ToTable("TimeCards", schema: "HumanResources");
            entity.HasKey(e => e.Id);

            entity.Property(p => p.EmployeeId)
                .HasColumnType("UNIQUEIDENTIFIER")
                .HasColumnName("EmployeeId").IsRequired();
            entity.Property(p => p.SupervisorId)
                .HasColumnType("UNIQUEIDENTIFIER")
                .HasColumnName("SupervisorId").IsRequired();
            entity.Property(p => p.PayPeriodEnded)
                .HasConversion(p => p.Value, p => EntityDate.Create(p))
                .HasColumnType("DATETIME2(0)")
                .HasColumnName("PayPeriodEnded")
                .IsRequired();
            entity.Property(p => p.RegularHours)
                .HasConversion(p => p.Value, p => DecimalNotNegative.Create(p))
                .HasColumnType("INT")
                .HasColumnName("RegularHours")
                .IsRequired();
            entity.Property(p => p.OvertimeHours)
                .HasConversion(p => p.Value, p => DecimalNotNegative.Create(p))
                .HasColumnType("INT")
                .HasColumnName("OverTimeHours")
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