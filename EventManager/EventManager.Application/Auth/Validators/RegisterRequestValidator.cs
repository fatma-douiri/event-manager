
using FluentValidation;
using EventManager.Application.Auth.Models;
using static EventManager.Application.Auth.Common.AuthenticationResult;

namespace EventManager.Application.Auth.Validators;

/// <summary>
/// Validator for the RegisterRequest model.
/// </summary>
public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
{
    /// <summary>
    /// Validates the RegisterRequest object.
    /// </summary>
    public RegisterRequestValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.LastName)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress()
            .WithMessage("le format de l'email est invalid");

        RuleFor(x => x.Password)
            .NotEmpty()
            .MinimumLength(6)
            .WithMessage("le password doit contenir 6 caractères au minimum");
    }
}