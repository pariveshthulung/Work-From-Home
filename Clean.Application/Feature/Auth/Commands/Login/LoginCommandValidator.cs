using System;
using Clean.Application.Dto.Auth;
using FluentValidation;

namespace Clean.Application.Feature.Auth.Commands.Login;

public class LoginCommandValidator : AbstractValidator<LoginDto>
{
    public LoginCommandValidator()
    {
        RuleFor(e => e.Email).NotEmpty().WithMessage("{PropertyName} is required.");
        RuleFor(e => e.Password).NotEmpty().WithMessage("{PropertyName} is required.");
    }
}
