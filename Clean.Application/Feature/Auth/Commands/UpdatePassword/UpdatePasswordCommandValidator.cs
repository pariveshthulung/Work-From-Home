using System;
using FluentValidation;

namespace Clean.Application.Feature.Auth.Commands.UpdatePassword;

public class UpdatePasswordCommandValidator : AbstractValidator<UpdatePasswordCommand>
{
    public UpdatePasswordCommandValidator()
    {
        RuleFor(e => e.UpdatePasswordDto.ConfirmPassword)
            .NotEmpty()
            .WithMessage("Confirm Password filed is required.")
            .Must((e, confirmPassword) => e.UpdatePasswordDto.NewPassword.Equals(confirmPassword))
            .WithMessage("New Password must match.");
        RuleFor(e => e.UpdatePasswordDto.Email).NotEmpty().WithMessage("Email filed is required.");
        RuleFor(e => e.UpdatePasswordDto.NewPassword)
            .NotEmpty()
            .WithMessage("NewPassword filed is required.");
        RuleFor(e => e.UpdatePasswordDto.CurrentPassword)
            .NotEmpty()
            .WithMessage("CurrentPassword filed is required.");
    }
}
