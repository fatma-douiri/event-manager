using MediatR;
using Microsoft.AspNetCore.Identity;
using EventManager.Application.Auth.Common;
using EventManager.Application.Common.Interfaces;
using EventManager.Domain.Model;
using EventManager.Domain.Constants;
using EventManager.Application.Auth.Models;
using Microsoft.Extensions.Logging;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace EventManager.Application.Auth.Commands.Register;
/// <summary>
/// Handles the registration command.
/// </summary>
/// <param name="identityService"></param>
/// <param name="userManager"></param>
/// <param name="jwtHandler"></param>
public class RegisterCommandHandler(
    IIdentityService identityService,
    UserManager<ApplicationUser> userManager,
    IJwtHandler jwtHandler) : IRequestHandler<RegisterCommand, AuthenticationResult>

{
    /// <summary>
    /// Handles the registration command.
    /// </summary>
    /// <param name="request">The registration command request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The authentication result.</returns>
    public async Task<AuthenticationResult> Handle(
        RegisterCommand request,
        CancellationToken cancellationToken)
    {
        var existingUser = await userManager.FindByEmailAsync(request.Email);
        if (existingUser != null)
            return AuthenticationResult.Failure(AuthenticationResult.ErrorMessages.EmailAlreadyExists);
       
        var (success, userId) = await identityService.CreateUserAsync(
            request.FirstName,
            request.LastName,
            request.Email,
            request.Password);

        if (!success)
        {  

            return AuthenticationResult.Failure(AuthenticationResult.ErrorMessages.InvalidCredentials);
        }
       

        var user = await userManager.FindByIdAsync(userId);
        if (user == null)
            return AuthenticationResult.Failure(AuthenticationResult.ErrorMessages.RegistrationFailed);
        var roles = await userManager.GetRolesAsync(user);
        var token = jwtHandler.GenerateToken(user, roles);

        return AuthenticationResult.Success(new AuthResponse
        {
            Token = token,
            Email = user.Email ?? string.Empty,
            FullName = $"{user.FirstName} {user.LastName}",
            Role = UserRoles.Participant
        });
    }
}