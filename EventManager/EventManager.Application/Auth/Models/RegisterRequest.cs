namespace EventManager.Application.Auth.Models;

/// <summary>
/// Request containing registration details.
/// </summary>
public record RegisterRequest : IEquatable<RegisterRequest>
{
    /// <summary>
    /// Gets or sets the first name of the user.
    /// </summary>
    public required string FirstName { get; init; }

    /// <summary>
    /// Gets or sets the last name of the user.
    /// </summary>
    public required string LastName { get; init; }

    /// <summary>
    /// Gets or sets the email of the user.
    /// </summary>
    public required string Email { get; init; }

    /// <summary>
    /// Gets or sets the password of the user.
    /// </summary>
    public required string Password { get; init; }
}
