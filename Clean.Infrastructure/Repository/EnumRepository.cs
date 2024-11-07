using Clean.Application.Persistence.Contract;
using Clean.Domain.Entities;
using Clean.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Clean.Infrastructure.Repository;

public class EnumRepository : IEnumRepository
{
    private readonly ApplicationDbContext _context;
    private readonly RoleManager<UserRole> _roleManager;

    public EnumRepository(
        ApplicationDbContext applicationDbContext,
        RoleManager<UserRole> roleManager
    )
    {
        _context = applicationDbContext;
        _roleManager = roleManager;
    }

    public async Task<List<ApprovalStatus>> GetApprovalStatusAsync(
        CancellationToken cancellationToken
    )
    {
        return await _context.ApprovalStatuses.ToListAsync(cancellationToken);
    }

    public async Task<List<RequestedType>> GetRequestedTypeAsync(
        CancellationToken cancellationToken
    )
    {
        return await _context.RequestedTypes.ToListAsync(cancellationToken);
    }

    public async Task<List<UserRole>> GetRolesAsync(CancellationToken cancellationToken)
    {
        return await _roleManager.Roles.ToListAsync();
    }
}
