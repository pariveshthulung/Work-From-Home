using Clean.Application.Dto.Employee;
using Clean.Application.Helper;
using Clean.Domain.Entities;
using Clean.Domain.Entities.View;

namespace Clean.Application.Persistence.Contract;

public interface IEmployeeRepository
{
    Task<Employee?> GetEmployeeByGuidIdAsync(Guid guidId, CancellationToken cancellationToken);
    Task<bool> EmployeeExistAsync(int id, CancellationToken cancellationToken);
    Task<bool> EmailExistAsync(string email, CancellationToken cancellationToken);
    Task<PagedList<EmployeeDto>> GetAllEmployeeAsync(
        string? searchTerm,
        string? sortColumn,
        string? sortOrder,
        int page,
        int pageSize,
        CancellationToken cancellationToken
    );
    Task DeleteEmployeeAsync(Employee employee, CancellationToken cancellationToken);
    Task UpdateEmployeeAsync(Employee employee, CancellationToken cancellationToken);
    Task<Employee?> AddEmployeeAsync(Employee employee, CancellationToken cancellationToken);

    Task<Employee?> GetEmployeeByIdAsync(int id, CancellationToken cancellationToken);

    Task<Employee?> GetEmployeeByEmailAsync(string email, CancellationToken cancellationToken);
    Task SaveChangesAsync(CancellationToken cancellationToken);
}
