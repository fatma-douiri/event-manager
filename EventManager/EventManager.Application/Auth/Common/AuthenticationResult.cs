using EventManager.Application.Auth.Models;

namespace EventManager.Application.Auth.Common;

/// <summary>
/// Represents the result of an authentication attempt.
/// </summary>
public record AuthenticationResult
{
    /// <summary>
    /// Gets a value indicating whether the authentication attempt succeeded.
    /// </summary>
    public bool Succeeded { get; init; }

    /// <summary>
    /// Gets the error message if the authentication attempt failed.
    /// </summary>
    public string? Error { get; init; }

    /// <summary>
    /// Gets the response if the authentication attempt succeeded.
    /// </summary>
    public AuthResponse? Response { get; init; }

    private AuthenticationResult() { }

    /// <summary>
    /// Creates a successful authentication result.
    /// </summary>
    /// <param name="response">The response of the successful authentication attempt.</param>
    /// <returns>An instance of <see cref="AuthenticationResult"/> indicating success.</returns>
    public static AuthenticationResult Success(AuthResponse response) =>
        new() { Succeeded = true, Response = response };

    /// <summary>
    /// Creates a failed authentication result.
    /// </summary>
    /// <param name="error">The error message of the failed authentication attempt.</param>
    /// <returns>An instance of <see cref="AuthenticationResult"/> indicating failure.</returns>
    public static AuthenticationResult Failure(string error) =>
        new() { Succeeded = false, Error = error };

    /// <summary>
    /// Contains error messages for authentication failures.
    /// </summary>
    public static class ErrorMessages
    {
        /// <summary>
        /// Error message for invalid credentials.
        /// </summary>
        public const string InvalidCredentials = "Email ou mot de passe invalide";

        /// <summary>
        /// Error message for user not found.
        /// </summary>
        public const string UserNotFound = "Utilisateur non trouvé";

        /// <summary>
        /// Error message for email already exists.
        /// </summary>
        public const string EmailAlreadyExists = "Cet email existe déjà";

        /// <summary>
        /// Error message for registration failed.
        /// </summary>
        public const string RegistrationFailed = "L'inscription a échoué";
       
    }
}
