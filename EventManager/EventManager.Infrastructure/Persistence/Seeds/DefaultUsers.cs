using EventManager.Domain.Constants;
using EventManager.Domain.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace EventManager.Infrastructure.Persistence.Seeds;

public static class DefaultUsers
{
    public static async Task SeedAsync(UserManager<ApplicationUser> userManager)
    {
         
        var organizer = new ApplicationUser
        {
            UserName = "organizer@example.com",
            Email = "organizer@example.com",
            FirstName = "Default",
            LastName = "Organizer",
            EmailConfirmed = true
        };

        var participant = new ApplicationUser
        {
            UserName = "participant@example.com",
            Email = "participant@example.com",
            FirstName = "Default",
            LastName = "Participant",
            EmailConfirmed = true
        };

        await CreateUserIfNotExists(userManager, organizer, "Pass123$", UserRoles.Organizer);
        await CreateUserIfNotExists(userManager, participant, "Pass123$", UserRoles.Participant);
    }

    private static async Task CreateUserIfNotExists(
        UserManager<ApplicationUser> userManager,
        ApplicationUser user,
        string password,
        string role)
    {
        if (user.Email != null && await userManager.FindByEmailAsync(user.Email) == null)
        {
            await userManager.CreateAsync(user, password);
            await userManager.AddToRoleAsync(user, role);
        }
    }
}