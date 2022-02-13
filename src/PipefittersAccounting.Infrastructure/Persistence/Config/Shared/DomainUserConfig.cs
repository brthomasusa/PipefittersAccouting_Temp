using Microsoft.EntityFrameworkCore;
using PipefittersAccounting.Core.Shared;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PipefittersAccounting.Infrastructure.Persistence.Config.Shared
{
    internal class DomainUserConfig : IEntityTypeConfiguration<DomainUser>
    {
        public void Configure(EntityTypeBuilder<DomainUser> entity)
        {
            entity.ToTable("DomainUsers", schema: "Shared");
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.Email, "idx_DomainUsers$Email").IsUnique();
            entity.HasIndex(e => e.UserName, "idx_DomainUsers$UserName").IsUnique();
            entity.HasOne<ExternalAgent>(e => e.Agent).WithOne().HasForeignKey<DomainUser>(e => e.Id);

            entity.Property(p => p.Id).HasColumnType("UNIQUEIDENTIFIER").HasColumnName("UserId").ValueGeneratedNever();
            entity.Property(p => p.UserName).HasColumnType("NVARCHAR(256)").HasColumnName("UserName");
            entity.Property(p => p.Email).HasColumnType("NVARCHAR(256)").HasColumnName("Email");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime2(7)");
            entity.Property(e => e.LastModifiedDate).HasColumnType("datetime2(7)");
        }
    }
}