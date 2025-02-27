using Clean.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Clean.Infrastructure.EntitiesConfigure;

public class RequestConfiguration : IEntityTypeConfiguration<Request>
{
    public void Configure(EntityTypeBuilder<Request> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.FromDate).IsRequired();
        builder.Property(e => e.ToDate).IsRequired();
        builder.Property(e => e.RequestedBy).IsRequired();
        builder.Property(e => e.RequestedTo).IsRequired();
        builder
            .HasOne(e => e.Approval)
            .WithOne(a => a.Request)
            .HasForeignKey<Approval>(e => e.RequestId);
        builder.HasQueryFilter(e => !e.IsDeleted);
        builder
            .HasOne(r => r.RequestedByEmployee) // Navigation property
            .WithMany() // No reverse navigation property in Employee
            .HasForeignKey(r => r.RequestedBy) // Foreign key in Request
            .OnDelete(DeleteBehavior.Restrict); // Optional: Define delete behavior
        builder
            .HasOne(r => r.RequestedToEmployee)
            .WithMany()
            .HasForeignKey(r => r.RequestedTo)
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasQueryFilter(e => !e.IsDeleted);
    }
}
