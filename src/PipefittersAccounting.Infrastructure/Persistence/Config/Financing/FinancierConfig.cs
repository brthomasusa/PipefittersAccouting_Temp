using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PipefittersAccounting.SharedKernel.CommonValueObjects;
using PipefittersAccounting.Core.Shared;
using PipefittersAccounting.Core.Financing.FinancierAggregate;

namespace PipefittersAccounting.Infrastructure.Persistence.Config.Financing
{
    internal class FinancierConfig : IEntityTypeConfiguration<Financier>
    {
        public void Configure(EntityTypeBuilder<Financier> entity)
        {
            entity.ToTable("Financiers", schema: "Finance");
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.UserId, "idx_Financier$UserId");
            entity.HasIndex(e => e.FinancierName, "idx_FinancierName").IsUnique();

            entity.Property(p => p.Id).HasColumnType("UNIQUEIDENTIFIER").HasColumnName("FinancierID").ValueGeneratedNever();
            entity.Property(p => p.FinancierName)
                .HasConversion(p => p.Value, p => OrganizationName.Create(p))
                .HasColumnType("NVARCHAR(50)")
                .HasColumnName("FinancierName")
                .IsRequired();
            entity.Property(p => p.FinancierTelephone)
                .HasConversion(p => p.Value, p => PhoneNumber.Create(p))
                .HasColumnType("NVARCHAR(14)")
                .HasColumnName("Telephone")
                .IsRequired();
            entity.OwnsOne(p => p.FinancierAddress, p =>
            {
                p.Property(pp => pp.AddressLine1).HasColumnType("NVARCHAR(30)").HasColumnName("AddressLine1").IsRequired();
                p.Property(pp => pp.AddressLine2).HasColumnType("NVARCHAR(30)").HasColumnName("AddressLine2").IsRequired(false);
                p.Property(pp => pp.City).HasColumnType("NVARCHAR(30)").HasColumnName("City");
                p.Property(pp => pp.StateCode).HasColumnType("NCHAR(2)").HasColumnName("StateCode").IsRequired();
                p.Property(pp => pp.Zipcode).HasColumnType("NVARCHAR(10)").HasColumnName("Zipcode");
            });
            entity.OwnsOne(p => p.PointOfContact, poc =>
            {
                poc.Property(pp => pp.FirstName).HasColumnType("NVARCHAR(25)").HasColumnName("ContactFirstName").IsRequired();
                poc.Property(pp => pp.LastName).HasColumnType("NVARCHAR(25)").HasColumnName("ContactLastName").IsRequired(false);
                poc.Property(pp => pp.MiddleInitial).HasColumnType("NCHAR(1)").HasColumnName("ContactMiddleInitial");
                poc.Property(pp => pp.Telephone).HasColumnType("NVARCHAR(14)").HasColumnName("ContactTelephone").IsRequired();
            });
            entity.Property(p => p.UserId)
                .HasColumnType("UNIQUEIDENTIFIER")
                .HasColumnName("UserId")
                .IsRequired();
            entity.Property(p => p.IsActive)
                .HasColumnType("BIT")
                .HasColumnName("IsActive")
                .IsRequired();
            entity.Property(e => e.CreatedDate).HasColumnType("datetime2(7)");
            entity.Property(e => e.LastModifiedDate).HasColumnType("datetime2(7)");
        }
    }
}