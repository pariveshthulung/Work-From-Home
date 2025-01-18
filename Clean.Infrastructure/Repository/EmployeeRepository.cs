using System.Linq.Expressions;
using AutoMapper;
using Clean.Application.Dto.Employee;
using Clean.Application.Helper;
using Clean.Application.Persistence.Contract;
using Clean.Domain.Entities;
using Clean.Domain.Enums;
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
        try
        {
            await context.AddAsync(employee, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
            return employee;
        }
        catch (Exception e)
        {
            throw;
        }
    }

    public async Task DeleteEmployeeAsync(Employee employee, CancellationToken cancellationToken)
    {
        try
        {
            // context.Employees.Remove(employee);
            context.Employees.Update(employee);
            await context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<Employee?> GetEmployeeByGuidIdAsync(
        Guid guidId,
        CancellationToken cancellationToken
    )
    {
        try
        {
            return await context
                .Employees.Include(e => e.Address)
                .Include(x => x.UserRole)
                .Include(x => x.Manager)
                .Include(x => x.AppUser)
                .Include(x => x.Requests)
                .ThenInclude(x => x.Approval)
                .ThenInclude(y => y!.ApprovalStatus)
                .Include(x => x.Requests)
                .ThenInclude(x => x.RequestedToEmployee)
                .Include(x => x.Requests)
                .ThenInclude(x => x.RequestedType)
                .FirstOrDefaultAsync(x => x.GuidId == guidId, cancellationToken);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<Employee?> GetEmployeeByIdAsync(int id, CancellationToken cancellationToken)
    {
        try
        {
            return await context
                .Employees.Include(x => x.Requests)
                .ThenInclude(x => x.Approval)
                .Include(x => x.AppUser)
                .Include(x => x.Manager)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task UpdateEmployeeAsync(
        Employee employee,
        Employee existingEntity,
        CancellationToken cancellationToken
    )
    {
        try
        {
            if (existingEntity != null)
            {
                context.Entry(existingEntity).State = EntityState.Detached;
            }
            context.Employees.Attach(employee);

            context.Entry(employee).State = EntityState.Modified;
            if (employee.AppUser != null)
            {
                context.Entry(employee.AppUser).State = EntityState.Modified;
            }
            context.Entry(employee.Address!).State = EntityState.Modified;
            foreach (var request in employee.Requests)
            {
                if (request.Id != 0)
                {
                    context.Entry(request).State = EntityState.Modified;
                    if (request.Approval != null)
                    {
                        context.Entry(request.Approval).State = EntityState.Modified;
                    }
                }
                else
                {
                    context.Entry(request).State = EntityState.Added;
                }
            }

            context.Update(employee);

            await context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
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
                .Include(x => x.Manager)
                .Include(x => x.Requests)
                .ThenInclude(x => x.Approval)
                .ThenInclude(y => y!.ApprovalStatus)
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
        catch (Exception e)
        {
            throw;
        }
    }

    public async Task<Employee?> GetEmployeeByEmailAsync(
        string email,
        CancellationToken cancellationToken
    )
    {
        try
        {
            var emp = await context
                .Employees.Include(x => x.Manager)
                .Include(x => x.UserRole)
                .Include(x => x.Requests)
                .ThenInclude(x => x.Approval)
                .ThenInclude(y => y!.ApprovalStatus)
                .Include(x => x.Requests)
                .ThenInclude(x => x.RequestedToEmployee)
                .Include(x => x.Requests)
                .ThenInclude(x => x.RequestedType)
                .FirstOrDefaultAsync(x => x.Email == email, cancellationToken);

            if (emp is null)
                return null;
            return emp;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<bool> EmailExistAsync(string email, CancellationToken cancellationToken)
    {
        try
        {
            return await context.Employees.AnyAsync(x => x.Email == email, cancellationToken);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<string?> GetEmployeeManagerEmailAsync(
        string email,
        CancellationToken cancellationToken
    )
    {
        try
        {
            return await context
                .Employees.Include(x => x.Manager)
                .Where(x => x.Email == email)
                .Select(x => x.Manager.Email)
                .AsNoTracking()
                .FirstOrDefaultAsync(cancellationToken);
        }
        catch (Exception e)
        {
            throw;
        }
    }

    public async Task<bool> EmailExistIncludeDeletedAsync(
        string email,
        CancellationToken cancellationToken
    )
    {
        return await context.Employees.IgnoreQueryFilters().AnyAsync(x => x.Email == email);
    }

    public async Task<PagedList<EmployeeDto>> GetAllEmployeeAsync(
        int currentUserId,
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
            var currentUserRoleId = await context
                .Employees.Where(x => x.Id == currentUserId)
                .Select(x => x.UserRoleId)
                .FirstOrDefaultAsync();
            IQueryable<Employee> employees;
            if (currentUserRoleId == UserRoleEnum.SuperAdmin.Id)
            {
                employees = context.Employees.AsQueryable();
            }
            else if (currentUserRoleId == UserRoleEnum.Admin.Id)
            {
                employees = context
                    .Employees.Where(x => x.UserRoleId != UserRoleEnum.SuperAdmin.Id)
                    .AsQueryable();
            }
            else
            {
                employees = context
                    .Employees.Where(x => x.ManagerId == currentUserId)
                    .AsQueryable();
            }

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
                .Include(x => x.Manager)
                .Include(x => x.Requests)
                .ThenInclude(x => x.Approval)
                .ThenInclude(y => y!.ApprovalStatus)
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
        catch (Exception e)
        {
            throw;
        }
    }
}
