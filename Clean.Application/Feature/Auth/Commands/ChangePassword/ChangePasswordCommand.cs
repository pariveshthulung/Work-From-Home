using System;
using Clean.Application.Dto.Auth;
using Clean.Application.Wrappers;
using MediatR;

namespace Clean.Application.Feature.Auth.Commands.ChangePassword;

public class ChangePasswordCommand : IRequest<BaseResult<Unit>>
{
    public ChangePasswordDto ChangePasswordDto { get; set; }
}
