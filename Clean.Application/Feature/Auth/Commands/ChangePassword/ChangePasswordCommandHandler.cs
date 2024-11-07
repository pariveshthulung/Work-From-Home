using System;
using Clean.Application.Dto.Auth;
using Clean.Application.Exceptions;
using Clean.Application.Persistence.Contract;
using Clean.Application.Wrappers;
using Clean.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Clean.Application.Feature.Auth.Commands.ChangePassword;

public class ChangePasswordCommandHandler(
    UserManager<AppUser> userManager,
    SignInManager<AppUser> signInManager,
    ICurrentUserService currentUserService
) : IRequestHandler<ChangePasswordCommand, BaseResult<Unit>>
{
    public async Task<BaseResult<Unit>> Handle(
        ChangePasswordCommand request,
        CancellationToken cancellationToken
    )
    {
        var validator = new ChangePasswordCommandValidator();
        var validationResult = await validator.ValidateAsync(
            request.ChangePasswordDto,
            cancellationToken
        );
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(e => new Error(
                400,
                e.PropertyName,
                e.ErrorMessage
            ));
            return BaseResult<Unit>.Failure(errors);
        }

        var user =
            await userManager.Users.FirstOrDefaultAsync(x =>
                x.Email == currentUserService.UserEmail
            ) ?? throw new Exception("Unauthorize user");

        var result = await userManager.ChangePasswordAsync(
            user,
            request.ChangePasswordDto.CurrentPassword,
            request.ChangePasswordDto.NewPassword
        );

        if (!result.Succeeded)
        {
            var errors = result.Errors.Select(e => new Error(400, e.Code, e.Description)).ToList();
            return BaseResult<Unit>.Failure(errors);
        }

        await signInManager.SignOutAsync();
        return BaseResult<Unit>.Ok(Unit.Value);
    }
}
