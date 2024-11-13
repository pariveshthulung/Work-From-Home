using Clean.Application.Dto.Auth;
using Clean.Application.Wrappers;
using MediatR;

namespace Clean.Application.Feature.Auth.Commands.UpdatePassword;

public class UpdatePasswordCommand : IRequest<BaseResult<Unit>>
{
    public UpdatePasswordDto UpdatePasswordDto { get; init; }
}
