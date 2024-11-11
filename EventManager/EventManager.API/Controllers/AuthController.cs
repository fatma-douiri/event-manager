using EventManager.Application.Auth.Commands.Login;
using EventManager.Application.Auth.Commands.Register;
using EventManager.Application.Auth.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using FluentValidation;


namespace EventManager.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IMediator mediator, ILogger<AuthController> logger) : ControllerBase
{

    [HttpPost("login")]
    public async Task<ActionResult<AuthResponse>> Login(LoginRequest request)
    {
        var command = new LoginCommand(request.Email, request.Password);
        var result = await mediator.Send(command);

        if (!result.Succeeded)
            return Unauthorized(new { message = result.Error });

        return Ok(result.Response);
    }


    [HttpPost("register")]
    public async Task<ActionResult<AuthResponse>> Register(RegisterRequest request)
    {
        try
        {
            var command = new RegisterCommand(
                request.FirstName,
                request.LastName,
                request.Email,
                request.Password);

            var result = await mediator.Send(command);

            if (!result.Succeeded)
                return BadRequest(new { message = result.Error });

            return Ok(result.Response);
        }
        catch (ValidationException ex)
        {
            logger.LogError($"Exception during registration: {ex}");

            return BadRequest(new
            {
                message = string.Join(", ", ex.Errors.Select(e => e.ErrorMessage)),
                errors = ex.Errors.Select(e => e.ErrorMessage)
            });
         
        }
    }
}