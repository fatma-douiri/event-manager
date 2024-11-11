using EventManager.Domain.Model;

namespace EventManager.Application.Common.Interfaces;

/// <summary>
/// Provides methods for handling JWT tokens.
/// </summary>
public interface IJwtHandler
{
    /// <summary>
    /// Generates a JWT token for the specified user and roles.
    /// </summary>
    /// <param name="user">The application user.</param>
    /// <param name="roles">The roles of the user.</param>
    /// <returns>A JWT token as a string.</returns>
    string GenerateToken(ApplicationUser user, IList<string> roles);
}
