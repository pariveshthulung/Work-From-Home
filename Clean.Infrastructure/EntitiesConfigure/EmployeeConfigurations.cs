using Clean.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Clean.Infrastructure.EntitiesConfigure;

public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Name).IsRequired().HasMaxLength(40);
        builder.Property(e => e.PhoneNumber).IsRequired().HasMaxLength(10);
        builder.Property(e => e.Email).IsRequired().HasMaxLength(50);
        builder.HasIndex(e => e.Email).IsUnique();
        builder.OwnsOne(e => e.Address);
        builder
            .HasDiscriminator<string>("Discriminator")
            .HasValue<GeneralEmployee>("GeneralEmployee");
        builder.HasOne(a => a.AppUser).WithOne().HasForeignKey<Employee>(a => a.AppUserId);
        builder.HasQueryFilter(e => !e.IsDeleted);
        builder.HasMany(e => e.Requests).WithOne(r => r.Employee).HasForeignKey(r => r.EmployeeId);
    }
}
