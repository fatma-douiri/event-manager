namespace EventManager.Application.Common.Interfaces;

/// <summary>
/// Provides methods for managing user identities and roles.
/// </summary>
public interface IIdentityService
{
    /// <summary>
    /// Creates a new user asynchronously.
    /// </summary>
    /// <param name="firstName">The first name of the user.</param>
    /// <param name="lastName">The last name of the user.</param>
    /// <param name="email">The email of the user.</param>
    /// <param name="password">The password of the user.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a tuple with a success flag and the user ID.</returns>
    Task<(bool Success, string UserId)> CreateUserAsync(string firstName, string lastName, string email, string password);

    /// <summary>
    /// Validates a user asynchronously.
    /// </summary>
    /// <param name="userName">The username of the user.</param>
    /// <param name="password">The password of the user.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a boolean indicating whether the user is valid.</returns>
    Task<bool> ValidateUserAsync(string userName, string password);

    /// <summary>
    /// Checks if a user is in a specific role asynchronously.
    /// </summary>
    /// <param name="userId">The ID of the user.</param>
    /// <param name="role">The role to check.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a boolean indicating whether the user is in the role.</returns>
    Task<bool> IsInRoleAsync(string userId, string role);

    /// <summary>
    /// Assigns a role to a user asynchronously.
    /// </summary>
    /// <param name="userId">The ID of the user.</param>
    /// <param name="role">The role to assign.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a boolean indicating whether the role was successfully assigned.</returns>
    Task<bool> AssignRoleToUserAsync(string userId, string role);

    /// <summary>
    /// Authorizes a user based on a policy asynchronously.
    /// </summary>
    /// <param name="userId">The ID of the user.</param>
    /// <param name="policyName">The name of the policy.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a boolean indicating whether the user is authorized.</returns>
    Task<bool> AuthorizeAsync(string userId, string policyName);

    /// <summary>
    /// Gets the username of a user asynchronously.
    /// </summary>
    /// <param name="userId">The ID of the user.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the username of the user.</returns>
    Task<string?> GetUserNameAsync(string userId);
}
