using Clean.Application.Wrappers;
using Clean.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Clean.Application.Feature.Auth.Commands.UpdatePassword;

public class UpdatePasswordCommandHandler : IRequestHandler<UpdatePasswordCommand, BaseResult<Unit>>
{
    private readonly UserManager<AppUser> _userManager;

    public UpdatePasswordCommandHandler(UserManager<AppUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<BaseResult<Unit>> Handle(
        UpdatePasswordCommand request,
        CancellationToken cancellationToken
    )
    {
        try
        {
            var validator = new UpdatePasswordCommandValidator();
            var response = await validator.ValidateAsync(request, cancellationToken);
            if (!response.IsValid)
            {
                var error = response
                    .Errors.Select(e => new Error(400, e.PropertyName, e.ErrorMessage))
                    .ToList();
                return BaseResult<Unit>.Failure(error);
            }
            var user = await _userManager.Users.FirstOrDefaultAsync(x =>
                x.Email == request.UpdatePasswordDto.Email && !x.IsDeleted
            );
            if (user is null)
                return BaseResult<Unit>.Failure(
                    EmployeeErrors.NotFound(request.UpdatePasswordDto.Email)
                );
            var result = await _userManager.ChangePasswordAsync(
                user,
                request.UpdatePasswordDto.CurrentPassword,
                request.UpdatePasswordDto.NewPassword
            );

            if (!result.Succeeded)
            {
                var errors = result
                    .Errors.Select(x => new Error(400, x.Code, x.Description))
                    .ToList();
                return BaseResult<Unit>.Failure(errors);
            }
            user.SetPasswordExpire(false);
            await _userManager.UpdateAsync(user);

            return BaseResult<Unit>.Ok(Unit.Value);
        }
        catch (Exception ex)
        {
            throw;
        }
    }
}
