using Clean.Domain.Entities;
using Clean.Domain.Entities.StoreProcedure;
using Clean.Domain.Entities.View;
using Clean.Infrastructure.EntitiesConfigure;
using Clean.Infrastructure.OutBox;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Clean.Infrastructure.Data;

public class ApplicationDbContext : IdentityDbContext<AppUser, UserRole, int>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }

    public DbSet<Employee> Employees { get; set; }
    public DbSet<AppUser> AppUsers { get; set; }
    public DbSet<GeneralEmployee> GeneralEmployees { get; set; }
    public DbSet<Request> Requests { get; set; }
    public DbSet<GeneralRequest> GeneralRequests { get; set; }
    public DbSet<Approval> Approvals { get; set; }

    public DbSet<UserRole> ApplicationRoles { get; set; }
    public DbSet<ApprovalStatus> ApprovalStatuses { get; set; }
    public DbSet<RequestedType> RequestedTypes { get; set; }
    public DbSet<OutBoxMessage> OutBoxMessages { get; set; }
    public DbSet<GetAllEmployees> GetAllEmployees { get; set; } //sp
    public DbSet<SqlInjection> SqlInjections { get; set; } //view
    public DbSet<EmployeeDetails> EmployeeDetails { get; set; } // view

    public override Task<int> SaveChangesAsync(
        CancellationToken cancellationToken = new CancellationToken()
    )
    {
        foreach (var entity in ChangeTracker.Entries().Where(e => e.State == EntityState.Deleted))
        {
            entity.State = EntityState.Modified;
            entity.Property("IsDeleted").CurrentValue = true;
            // entity.Entity.GetType().GetProperty("IsDeleted").SetValue(entity, true);
        }
        return base.SaveChangesAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfiguration(new AppUserConfigurations());
        modelBuilder.ApplyConfiguration(new ApprovalConfiguration());
        modelBuilder.ApplyConfiguration(new EmployeeConfiguration());
        modelBuilder.ApplyConfiguration(new RequestConfiguration());
        modelBuilder.Entity<AppUser>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<GetAllEmployees>(entity => entity.HasKey(e => e.Id));
    }
}
