using Clean.Application.Dto.Employee;
using Clean.Application.Helper;
using Clean.Application.Wrappers;
using MediatR;

namespace Clean.Application.Feature.Employees.Requests.Queries;

public class GetEmployeeListQuery : IRequest<BaseResult<PagedList<EmployeeDto>>>
{
    public string? SearchTerm { get; set; }
    public string? SortColumn { get; set; }
    public string? SortOrder { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 5;
}
