using MediatR;
using Microsoft.AspNetCore.Identity;
using EventManager.Application.Auth.Common;
using EventManager.Application.Common.Interfaces;
using EventManager.Domain.Model;
using EventManager.Application.Auth.Models;

namespace EventManager.Application.Auth.Commands.Login;
/// <summary>
/// Handles the login command.
/// </summary>
/// <param name="identityService"></param>
/// <param name="userManager"></param>
/// <param name="jwtHandler"></param>
public class LoginCommandHandler(
    IIdentityService identityService,
    UserManager<ApplicationUser> userManager,
    IJwtHandler jwtHandler) : IRequestHandler<LoginCommand, AuthenticationResult>
{
    /// <summary>
    /// Handles the login command.
    /// </summary>
    /// <param name="request">The login command request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The authentication result.</returns>
    public async Task<AuthenticationResult> Handle(
        LoginCommand request,
        CancellationToken cancellationToken)
    {
        var result = await identityService.ValidateUserAsync(request.Email, request.Password);
        if (!result)
            return AuthenticationResult.Failure(AuthenticationResult.ErrorMessages.InvalidCredentials);

        var user = await userManager.FindByEmailAsync(request.Email);
        if (user == null)
            return AuthenticationResult.Failure(AuthenticationResult.ErrorMessages.UserNotFound);

        var roles = await userManager.GetRolesAsync(user);
        var token = jwtHandler.GenerateToken(user, roles);

        return AuthenticationResult.Success(new AuthResponse
        {
            Token = token,
            Email = user.Email ?? string.Empty,
            FullName = $"{user.FirstName} {user.LastName}",
            Role = roles.FirstOrDefault() ?? string.Empty
        });
    }
}