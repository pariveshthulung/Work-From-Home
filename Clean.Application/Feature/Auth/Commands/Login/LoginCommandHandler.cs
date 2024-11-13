using Clean.Application.Dto.Auth;
using Clean.Application.Exceptions;
using Clean.Application.Persistence.Contract;
using Clean.Application.Wrappers;
using Clean.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Clean.Application.Feature.Auth.Commands.Login;

public class LoginCommandHandler : IRequestHandler<LoginCommand, BaseResult<AuthResponse>>
{
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly ITokenService _tokenService;

    public LoginCommandHandler(
        UserManager<AppUser> userManager,
        SignInManager<AppUser> signInManager,
        ITokenService tokenService
    )
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _tokenService = tokenService;
    }

    public async Task<BaseResult<AuthResponse>> Handle(
        LoginCommand request,
        CancellationToken cancellationToken
    )
    {
        try
        {
            var validator = new LoginCommandValidator();
            var validationResult = await validator.ValidateAsync(
                request.LoginDto,
                cancellationToken
            );

            if (!validationResult.IsValid)
            {
                var errors = validationResult
                    .Errors.Select(e => new Error(400, e.PropertyName, e.ErrorMessage))
                    .ToList();
                return BaseResult<AuthResponse>.Failure(errors);
            }

            var user = await _userManager.Users.FirstOrDefaultAsync(
                x => x.Email == request.LoginDto.Email && !x.IsDeleted,
                cancellationToken
            );

            if (user == null)
            {
                return BaseResult<AuthResponse>.Failure(
                    new Error(401, "Login", "Invalid username and password")
                );
            }

            var signInResult = await _signInManager.CheckPasswordSignInAsync(
                user,
                request.LoginDto.Password,
                false
            );

            if (!signInResult.Succeeded)
            {
                return BaseResult<AuthResponse>.Failure(
                    new Error(401, "Login", "Invalid username and password")
                );
            }

            if (user.IsPasswordExpire)
            {
                return BaseResult<AuthResponse>.Failure(
                    new Error(403, "Login", "Password expired. Please update your password!")
                );
            }

            var accessToken = await _tokenService.GenerateAccessTokenAsync(user);
            var refreshToken = _tokenService.GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(5);

            var updateResult = await _userManager.UpdateAsync(user);

            if (!updateResult.Succeeded)
            {
                var errors = updateResult
                    .Errors.Select(e => new Error(400, e.Code, e.Description))
                    .ToList();
                return BaseResult<AuthResponse>.Failure(errors);
            }

            var authResponse = new AuthResponse
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };

            return BaseResult<AuthResponse>.Ok(authResponse);
        }
        catch (Exception)
        {
            throw;
        }
    }
}
