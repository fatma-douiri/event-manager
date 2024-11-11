using MediatR;
using EventManager.Application.Auth.Common;

namespace EventManager.Application.Auth.Commands.Login;

/// <summary>
/// Command to login.
/// </summary>
/// <param name="Email"></param>
/// <param name="Password"></param>
public record LoginCommand(string Email, string Password)
    : IRequest<AuthenticationResult>;