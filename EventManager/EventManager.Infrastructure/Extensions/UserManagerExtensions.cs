using EventManager.Domain.Model;
using Microsoft.AspNetCore.Identity;

namespace EventManager.Infrastructure.Extensions;
public static class UserManagerExtensions
{
    public static async Task<List<string>> GetUserRolesAsync(this UserManager<ApplicationUser> userManager, string userId)
    {
        var user = await userManager.FindByIdAsync(userId);
        return user != null ? (await userManager.GetRolesAsync(user)).ToList() : new List<string>();
    }
}