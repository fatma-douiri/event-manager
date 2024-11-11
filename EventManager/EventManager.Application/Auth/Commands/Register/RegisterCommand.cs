using MediatR;
using EventManager.Application.Auth.Common;

namespace EventManager.Application.Auth.Commands.Register;
/// <summary>
/// Command to register.
/// </summary>
/// <param name="FirstName"></param>
/// <param name="LastName"></param>
/// <param name="Email"></param>
/// <param name="Password"></param>
public record RegisterCommand(
    string FirstName,
    string LastName,
    string Email,
    string Password) : IRequest<AuthenticationResult>;