using System;
using Clean.Application.Dto.Auth;
using FluentValidation;

namespace Clean.Application.Feature.Auth.Commands.ChangePassword;

public class ChangePasswordCommandValidator : AbstractValidator<ChangePasswordDto> { }
