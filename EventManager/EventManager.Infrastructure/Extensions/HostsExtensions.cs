using EventManager.Domain.Model;
using EventManager.Infrastructure.Persistence.Seeds;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;

namespace EventManager.Infrastructure.Extensions;
public static class HostExtensions
{
    public static async Task SeedDataAsync(this IHost host)
    {
        using var scope = host.Services.CreateScope();
        var services = scope.ServiceProvider;
        var logger = services.GetService<ILoggerFactory>()
           ?.CreateLogger(nameof(HostExtensions));

        try
        {
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();

            await DefaultRoles.SeedAsync(roleManager);
            await DefaultUsers.SeedAsync(userManager);
            
        }
        catch (Exception ex)
        {
            logger?.LogError(ex, "An error occurred while seeding the database.");

        }
    }
}
