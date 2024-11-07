using Clean.Application.Dto.Enum;
using Clean.Application.Wrappers;
using MediatR;

namespace Clean.Application.Feature.RoleEnum.Queries;

public class GetUserRolesQuery : IRequest<BaseResult<List<UserRoleEnumDto>>> { }
