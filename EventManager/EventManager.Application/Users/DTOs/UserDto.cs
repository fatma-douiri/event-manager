namespace EventManager.Application.Users.DTOs;

/// <summary>
/// Data Transfer Object for Usert.
/// </summary>
public class UserDto
{
    /// <summary>
    /// Gets or sets the user ID.
    /// </summary>
    public required string Id { get; set; }

    /// <summary>
    /// Gets or sets the full name of the user.
    /// </summary>
    public required string FullName { get; set; }

    /// <summary>
    /// Gets or sets the email of the user.
    /// </summary>
    public required string Email { get; set; }
}
