using System.Linq.Expressions;
using AutoMapper;
using Clean.Application.Dto.Employee;
using Clean.Application.Helper;
using Clean.Application.Persistence.Contract;
using Clean.Domain.Entities;
using Clean.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Clean.Infrastructure.Repository;

public class EmployeeRepository(ApplicationDbContext context, IMapper mapper) : IEmployeeRepository
{
    public async Task<Employee?> AddEmployeeAsync(
        Employee employee,
        CancellationToken cancellationToken
    )
    {
        await context.AddAsync(employee, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        return employee;
    }

    public async Task DeleteEmployeeAsync(Employee employee, CancellationToken cancellationToken)
    {
        // context.Employees.Remove(employee);
        context.Employees.Update(employee);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<Employee?> GetEmployeeByGuidIdAsync(
        Guid guidId,
        CancellationToken cancellationToken
    )
    {
        return await context
            .Employees.Include(x => x.UserRole)
            .Include(x => x.AppUser)
            .Include(x => x.Requests)
            .ThenInclude(x => x.Approval)
            .ThenInclude(y => y.ApprovalStatus)
            .Include(x => x.Requests)
            .ThenInclude(x => x.RequestedToEmployee)
            .Include(x => x.Requests)
            .ThenInclude(x => x.RequestedType)
            .FirstOrDefaultAsync(x => x.GuidId == guidId, cancellationToken);
    }

    public async Task<Employee?> GetEmployeeByIdAsync(int id, CancellationToken cancellationToken)
    {
        return await context
            .Employees.Include(x => x.Requests)
            .ThenInclude(x => x.Approval)
            .Include(x => x.AppUser)
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task UpdateEmployeeAsync(Employee employee, CancellationToken cancellationToken)
    {
        try
        {
            var existingEntity = context
                .ChangeTracker.Entries<Address>()
                .FirstOrDefault(e => e.Entity == employee.Address);

            if (existingEntity != null)
            {
                context.Entry(existingEntity.Entity).State = EntityState.Detached;
            }
            await context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception e)
        {
            throw;
        }
    }

    public async Task<PagedList<EmployeeDto>> GetAllEmployeeAsync(
        string? searchTerm,
        string? sortColumn,
        string? sortOrder,
        int page,
        int pageSize,
        CancellationToken cancellationToken
    )
    {
        try
        {
            IQueryable<Employee> employees = context.Employees.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                employees = employees.Where(x =>
                    x.Name.Contains(searchTerm) || x.Email.Contains(searchTerm)
                );
            }

            Expression<Func<Employee, object>> keySelector = sortColumn?.ToLower() switch
            {
                "email" => employee => employee.Email,
                "name" => employee => employee.Name,
                _ => employee => employee.Id
            };

            if (sortOrder?.ToLower() == "desc")
            {
                employees = employees.OrderByDescending(keySelector);
            }
            else
            {
                employees = employees.OrderBy(keySelector);
            }
            var productDtoResponse = employees
                .Include(x => x.UserRole)
                .Include(x => x.Requests)
                .ThenInclude(x => x.Approval)
                .ThenInclude(y => y.ApprovalStatus)
                .Include(x => x.Requests)
                .ThenInclude(x => x.RequestedToEmployee)
                .Include(x => x.Requests)
                .ThenInclude(x => x.RequestedType)
                .AsNoTracking()
                .Select(x => mapper.Map<EmployeeDto>(x));

            var pagedList = await PagedList<EmployeeDto>.CreateAsync(
                productDtoResponse,
                page,
                pageSize
            );

            return pagedList;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<Employee?> GetEmployeeByEmailAsync(
        string email,
        CancellationToken cancellationToken
    )
    {
        var emp = await context
            .Employees.Include(x => x.UserRole)
            .Include(x => x.Requests)
            .ThenInclude(x => x.Approval)
            .ThenInclude(y => y.ApprovalStatus)
            .Include(x => x.Requests)
            .ThenInclude(x => x.RequestedToEmployee)
            .Include(x => x.Requests)
            .ThenInclude(x => x.RequestedType)
            .FirstOrDefaultAsync(x => x.Email == email, cancellationToken);

        if (emp is null)
            return null;
        return emp;
    }

    public async Task<bool> EmployeeExistAsync(int id, CancellationToken cancellationToken)
    {
        return await context.Employees.AnyAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<bool> EmailExistAsync(string email, CancellationToken cancellationToken)
    {
        return await context.Employees.AnyAsync(x => x.Email == email, cancellationToken);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        await context.SaveChangesAsync(cancellationToken);
    }
}
