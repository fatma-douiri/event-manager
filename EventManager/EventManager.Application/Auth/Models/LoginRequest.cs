namespace EventManager.Application.Auth.Models;

/// <summary>
/// Request containing login details.
/// </summary>
public class LoginRequest
{
    /// <summary>
    /// Gets or sets the email of the user.
    /// </summary>
    public required string Email { get; set; }

    /// <summary>
    /// Gets or sets the password of the user.
    /// </summary>
    public required string Password { get; set; }
}
