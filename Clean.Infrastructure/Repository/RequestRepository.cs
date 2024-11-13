using System.Linq.Expressions;
using AutoMapper;
using Clean.Application.Dto.Request;
using Clean.Application.Helper;
using Clean.Application.Persistence.Contract;
using Clean.Domain.Entities;
using Clean.Domain.Enums;
using Clean.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Clean.Infrastructure.Repository;

public class RequestRepository(ApplicationDbContext context, IMapper mapper) : IRequestRepository
{
    public async Task DeleteRequestAsync(Request request, CancellationToken cancellationToken)
    {
        try
        {
            context.Requests.Remove(request);
            await context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<PagedList<RequestDto>> GetAllRequestAsync(
        string? searchTerm,
        string? sortColumn,
        string? sortOrder,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken
    )
    {
        try
        {
            IQueryable<Request> requests = context
                .Requests.Include(x => x.Employee)
                .Include(x => x.RequestedByEmployee)
                .Include(x => x.Approval)
                .Include(x => x.RequestedType)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                requests = requests.Where(x =>
                    x.Employee!.Email.Contains(searchTerm) || x.Employee.Name.Contains(searchTerm)
                );
            }

            Expression<Func<Request, object>> keyselector = sortColumn?.ToLower() switch
            {
                "name" => request => request.Employee!.Name,
                "email" => request => request.Employee!.Email,
                _ => request => request.Id,
            };

            if (sortOrder?.ToLower() == "desc")
            {
                requests = requests.OrderByDescending(keyselector);
            }
            else
            {
                requests = requests.OrderBy(keyselector);
            }
            var requestsDto = requests.Select(x => mapper.Map<RequestDto>(x));
            var requestPagedList = await PagedList<RequestDto>.CreateAsync(
                requestsDto,
                pageNumber,
                pageSize
            );
            return requestPagedList;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<List<Request>> GetAllRequestByRequestTypeAsync(
        string requestedType,
        CancellationToken cancellationToken
    )
    {
        try
        {
            return await context
                .Requests.Where(x =>
                    x.RequestedTypeId == RequestTypeEnum.FromName(requestedType).Id
                )
                .Include(x => x.Approval)
                .Include(x => x.Employee)
                .Include(x => x.RequestedToEmployee)
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<List<Request>> GetAllRequestByUserIdAsync(
        int userId,
        CancellationToken cancellationToken
    )
    {
        try
        {
            return await context
                .Requests.Where(x => x.RequestedBy == userId)
                .Include(x => x.Approval)
                .Include(x => x.Employee)
                .Include(x => x.RequestedToEmployee)
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<Request?> GetRequestByIdAsync(int id, CancellationToken cancellationToken)
    {
        try
        {
            return await context
                .Requests.Include(x => x.Approval)
                .Include(x => x.Employee)
                .Include(x => x.RequestedToEmployee)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<Request?> SubmitRequestAsync(
        Request request,
        CancellationToken cancellationToken
    )
    {
        try
        {
            await context.Requests.AddAsync(request, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
            return request;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task ApproveRequestAsync(Request request, CancellationToken cancellationToken)
    {
        try
        {
            context.Requests.Update(request);
            await context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public Task<List<Request>> GetAllRequestByStatusAsync(
        string approvalStatus,
        CancellationToken cancellationToken
    )
    {
        try
        {
            return context
                .Requests.Where(x =>
                    x.Approval!.ApprovalStatusId == ApprovalStatusEnum.FromName(approvalStatus).Id
                )
                .Include(x => x.Approval)
                .Include(x => x.Employee)
                .Include(x => x.RequestedToEmployee)
                .AsNoTracking()
                .ToListAsync(cancellationToken: cancellationToken);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<Request?> GetRequestByGuidIdAsync(
        Guid guidId,
        CancellationToken cancellationToken
    )
    {
        return await context
            .Requests.Include(x => x.Approval)
            .Include(x => x.Employee)
            .Include(x => x.RequestedToEmployee)
            .FirstOrDefaultAsync(e => e.GuidId == guidId, cancellationToken);
    }

    public async Task<List<Request>> GetAllRequestByUserGuidIdAsync(
        Guid userGuidId,
        CancellationToken cancellationToken
    )
    {
        return await context
            .Requests.Where(x => x.Employee!.GuidId == userGuidId)
            .Include(x => x.Approval)
            .Include(x => x.Employee)
            .Include(x => x.RequestedToEmployee)
            .ToListAsync(cancellationToken);
    }

    public async Task<List<Request>> GetAllRequestByUserEmailAsync(
        string email,
        CancellationToken cancellationToken
    )
    {
        var employeeId = await context
            .Employees.Where(x => x.Email == email)
            .Select(x => x.Id)
            .FirstOrDefaultAsync(cancellationToken);
        return await context
            .Requests.Where(r => r.EmployeeId == employeeId)
            .Include(x => x.Approval)
            .Include(x => x.Employee)
            .Include(x => x.RequestedToEmployee)
            .ToListAsync(cancellationToken);
    }

    public async Task<List<Request>> GetAllRequestSubmittedToUserAsync(
        string email,
        CancellationToken cancellationToken
    )
    {
        var emailId = await context
            .Employees.Where(x => x.Email.Equals(email))
            .Select(x => x.Id)
            .FirstOrDefaultAsync(cancellationToken: cancellationToken);
        return await context
            .Requests.Where(x =>
                x.RequestedTo == emailId
                && x.Approval!.ApprovalStatusId == ApprovalStatusEnum.Pending.Id
            )
            .Include(x => x.Approval)
            .Include(x => x.Employee)
            .Include(x => x.RequestedToEmployee)
            .ToListAsync(cancellationToken);
    }
}
