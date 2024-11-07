using Clean.Application.Helper;
using Clean.Domain.Entities;

namespace Clean.Application.Persistence.Contract;

public interface IRequestRepository
{
    Task<Request?> GetRequestByIdAsync(int id, CancellationToken cancellationToken);
    Task<Request?> GetRequestByGuidIdAsync(Guid guidId, CancellationToken cancellationToken);
    Task<List<Request>> GetAllRequestAsync(QueryObject query, CancellationToken cancellationToken);
    Task<List<Request>> GetAllRequestByStatusAsync(
        string approvalStatus,
        CancellationToken cancellationToken
    );
    Task<List<Request>> GetAllRequestByUserIdAsync(int userId, CancellationToken cancellationToken);
    Task<List<Request>> GetAllRequestByUserEmailAsync(
        string email,
        CancellationToken cancellationToken
    );
    Task<List<Request>> GetAllRequestByUserGuidIdAsync(
        Guid userGuidId,
        CancellationToken cancellationToken
    );
    Task<List<Request>> GetAllRequestByRequestTypeAsync(
        string requestType,
        CancellationToken cancellationToken
    );
    Task DeleteRequestAsync(Request request, CancellationToken cancellationToken);

    Task<Request?> SubmitRequestAsync(Request request, CancellationToken cancellationToken);
    Task ApproveRequestAsync(Request request, CancellationToken cancellationToken);

    Task<List<Request>> GetAllRequestSubmittedToUserAsync(
        string email,
        CancellationToken cancellationToken
    );
}
