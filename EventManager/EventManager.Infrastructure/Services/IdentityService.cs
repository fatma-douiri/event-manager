using Microsoft.AspNetCore.Identity;
using EventManager.Application.Common.Interfaces;
using EventManager.Domain.Model;
using EventManager.Domain.Constants;
using Microsoft.Extensions.Logging;

namespace EventManager.Infrastructure.Services;

public class IdentityService(
   UserManager<ApplicationUser> userManager,
   SignInManager<ApplicationUser> signInManager,
   ILogger<IdentityService> logger) : IIdentityService
{
    public async Task<(bool Success, string UserId)> CreateUserAsync(
        string firstName,
        string lastName,
        string email,
        string password)
    {
        
        var user = new ApplicationUser
        {
            FirstName = firstName,
            LastName = lastName,
            Email = email,
            UserName = email,
            EmailConfirmed = true
        };

        var result = await userManager.CreateAsync(user, password);

        if (!result.Succeeded)
        {
            var error = string.Join(", ", result.Errors.Select(e => e.Description));
            logger.LogError("User creation failed: {Error}", error);
            return (false, string.Empty);
        }

        var roleResult = await userManager.AddToRoleAsync(user, UserRoles.Participant);
        if (!roleResult.Succeeded)
        {
            var error = string.Join(", ", roleResult.Errors.Select(e => e.Description));
            logger.LogError("Role assignment failed: {Error}", error);
            return (false, string.Empty);
        }
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(user, UserRoles.Participant);
        }
        return (result.Succeeded, user.Id);
    }


    public async Task<bool> AssignRoleToUserAsync(string userId, string role)
    {
        var user = await userManager.FindByIdAsync(userId);
        if (user is null)
            return false;

        var addToRoleResult = await userManager.AddToRoleAsync(user, role);
        return addToRoleResult.Succeeded;
    }

    public async Task<bool> ValidateUserAsync(string userName, string password)
    {
        var user = await userManager.FindByEmailAsync(userName);
        if (user is null)
            return false;

        var result = await signInManager.CheckPasswordSignInAsync(user, password, false);
        return result.Succeeded;
    }

    public async Task<bool> IsInRoleAsync(string userId, string role)
    {
        var user = await userManager.FindByIdAsync(userId);
        if (user is null)
            return false;

        return await userManager.IsInRoleAsync(user, role);
    }

    public async Task<bool> AuthorizeAsync(string userId, string policyName)
    {
        var user = await userManager.FindByIdAsync(userId);
        return user is not null;
    }

    public async Task<string?> GetUserNameAsync(string userId)
    {
        var user = await userManager.FindByIdAsync(userId);
        return user?.UserName;
    }
}