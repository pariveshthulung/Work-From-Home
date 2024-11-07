using System;
using Clean.Application.Dto.Auth;
using Clean.Application.Wrappers;
using MediatR;

namespace Clean.Application.Feature.Auth.Commands.Login;

public class LoginCommand : IRequest<BaseResult<AuthResponse>>
{
    public LoginDto LoginDto { get; set; } = null!;
}
