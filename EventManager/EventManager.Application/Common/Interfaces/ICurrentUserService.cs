namespace EventManager.Application.Common.Interfaces;

/// <summary>
/// Provides information about the current user.
/// </summary>
public interface ICurrentUserService
{
    /// <summary>
    /// Gets the user ID of the current user.
    /// </summary>
    /// <value>The user ID string.</value>
    string UserId { get; }

    /// <summary>
    /// Gets a value indicating whether the current user is authenticated.
    /// </summary>
    /// <value>
    /// <c>true</c> if the user is authenticated; otherwise, <c>false</c>.
    /// </value>
    bool IsAuthenticated { get; }
}