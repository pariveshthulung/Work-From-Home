using Clean.Domain.Entities;

namespace Clean.Application.Persistence.Contract;

public interface IEnumRepository
{
    Task<List<UserRole>> GetRolesAsync(CancellationToken cancellationToken);
    Task<List<ApprovalStatus>> GetApprovalStatusAsync(CancellationToken cancellationToken);
    Task<List<RequestedType>> GetRequestedTypeAsync(CancellationToken cancellationToken);
}
