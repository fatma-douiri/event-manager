namespace EventManager.Application.Auth.Models;

/// <summary>
/// Response containing authentication details.
/// </summary>
public class AuthResponse
{
    /// <summary>
    /// Gets or sets the authentication token.
    /// </summary>
    public required string Token { get; set; }

    /// <summary>
    /// Gets or sets the email of the authenticated user.
    /// </summary>
    public required string Email { get; set; }

    /// <summary>
    /// Gets or sets the full name of the authenticated user.
    /// </summary>
    public required string FullName { get; set; }

    /// <summary>
    /// Gets or sets the role of the authenticated user.
    /// </summary>
    public required string Role { get; set; }
}
