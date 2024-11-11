using EventManager.Domain.Constants;
using Microsoft.AspNetCore.Identity;

namespace EventManager.Infrastructure.Persistence.Seeds;

public static class DefaultRoles
{
    public static async Task SeedAsync(RoleManager<IdentityRole> roleManager)
    {
        await CreateRoleIfNotExists(roleManager, UserRoles.Organizer);
        await CreateRoleIfNotExists(roleManager, UserRoles.Participant);
    }

    private static async Task CreateRoleIfNotExists(RoleManager<IdentityRole> roleManager, string roleName)
    {
        if (!await roleManager.RoleExistsAsync(roleName))
        {
            await roleManager.CreateAsync(new IdentityRole(roleName));
        }
    }
}
