using AutoMapper;
using Clean.Application.Dto.Enum;
using Clean.Application.Persistence.Contract;
using Clean.Application.Wrappers;
using Clean.Domain.Entities;
using MediatR;

namespace Clean.Application.Feature.RoleEnum.Queries;

public class GetUserRoleQueryHandler
    : IRequestHandler<GetUserRolesQuery, BaseResult<List<UserRoleEnumDto>>>
{
    private readonly IEnumRepository _enumRepo;

    private readonly IMapper _mapper;

    public GetUserRoleQueryHandler(IEnumRepository enumRepo, IMapper mapper)
    {
        _enumRepo = enumRepo;
        _mapper = mapper;
    }

    public async Task<BaseResult<List<UserRoleEnumDto>>> Handle(
        GetUserRolesQuery request,
        CancellationToken cancellationToken
    )
    {
        try
        {
            List<UserRole> userRoles = await _enumRepo.GetRolesAsync(cancellationToken);
            return BaseResult<List<UserRoleEnumDto>>.Ok(
                _mapper.Map<List<UserRoleEnumDto>>(userRoles)
            );
        }
        catch (Exception e)
        {
            throw;
        }
    }
}
