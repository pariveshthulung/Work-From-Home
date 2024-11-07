using Clean.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Clean.Infrastructure.EntitiesConfigure;

public class AppUserConfigurations : IEntityTypeConfiguration<AppUser>
{
    public void Configure(EntityTypeBuilder<AppUser> builder)
    {
        // builder.HasOne(a => a.Employee).WithOne().HasForeignKey<AppUser>(a => a.EmployeeId);
        // builder.ToTable("AspNetUsers");
        // builder.HasQueryFilter(e => !e.IsDeleted);
    }
}
