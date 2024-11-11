using EventManager.Application.Common.Interfaces;
using EventManager.Application.Events.Commands.CreateEvent;
using FluentValidation;
using System.Globalization;
using EventManager.Domain.Constants;

namespace EventManager.Application.Events.Commands.CreateEvent;
/// <summary>
/// Validator for the CreateEventCommand.
/// </summary>
public class CreateEventCommandValidator : AbstractValidator<CreateEventCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CreateEventCommandValidator"/> class.
    /// </summary>
    /// <param name="dateTimeService">The date time service.</param>
    /// <param name="currentUserService">The current user service.</param>
    /// <param name="identityService">The identity service.</param>
    public CreateEventCommandValidator(
        IDateTimeService dateTimeService,
        ICurrentUserService currentUserService,
        IIdentityService identityService)
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .MaximumLength(200)
            .MaximumLength(200).WithMessage("Le titre ne peut pas dépasser 200 caractères");
        

        RuleFor(x => x.Description)
            .NotEmpty()
            .MaximumLength(2000)
            .MaximumLength(2000).WithMessage("La description ne peut pas dépasser 2000 caractères");
        

        RuleFor(x => x.Location)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.MaxCapacity)
        .GreaterThan(0).WithMessage("La capacité maximale doit être supérieure à 0");

        // Check date format
        RuleFor(x => x.StartDate)
            .Matches(@"^(0[1-9]|[12][0-9]|3[01])/(0[1-9]|1[012])/\d{4}$")
            .WithMessage("Start date must be in format dd/MM/yyyy");

        RuleFor(x => x.EndDate)
            .Matches(@"^(0[1-9]|[12][0-9]|3[01])/(0[1-9]|1[012])/\d{4}$")
            .WithMessage("End date must be in format dd/MM/yyyy");

        // Check time format
        RuleFor(x => x.StartTime)
            .Matches(@"^([01]?[0-9]|2[0-3]):[0-5][0-9]$")
            .WithMessage("Start time must be in format HH:mm");
        
        RuleFor(x => x.EndTime)
            .Matches(@"^([01]?[0-9]|2[0-3]):[0-5][0-9]$")
            .WithMessage("End time must be in format HH:mm");

        //Check if event starts in the future
        RuleFor(x => x)
            .Must(x =>
            {
                if (!DateTime.TryParseExact(x.StartDate, "dd/MM/yyyy", CultureInfo.InvariantCulture,
                    DateTimeStyles.None, out DateTime startDate))
                    return false;

                if (!TimeSpan.TryParse(x.StartTime, out TimeSpan startTime))
                    return false;

                var startDateTime = startDate.Add(startTime);
                return startDateTime > dateTimeService.Now;
            })
            .WithMessage("Event must start in the future");

        //Check if event end time is after start time
        RuleFor(x => x)
            .Custom((command, context) =>
            {
                if (DateTime.TryParseExact(command.StartDate, "dd/MM/yyyy", CultureInfo.InvariantCulture,
                    DateTimeStyles.None, out DateTime startDate) &&
                    DateTime.TryParseExact(command.EndDate, "dd/MM/yyyy", CultureInfo.InvariantCulture,
                    DateTimeStyles.None, out DateTime endDate))
                {
                    if (TimeSpan.TryParse(command.StartTime, out TimeSpan startTime) &&
                        TimeSpan.TryParse(command.EndTime, out TimeSpan endTime))
                    {
                        var startDateTime = startDate.Add(startTime);
                        var endDateTime = endDate.Add(endTime);

                        if (endDateTime <= startDateTime)
                        {
                            context.AddFailure("La date et l'heure de fin doivent être après la date et l'heure de début");
                        }
                    }
                }
            });

        // Check if the user is an organizer
        RuleFor(x => x)
            .MustAsync(async (_, _) => await identityService.IsInRoleAsync(
                currentUserService.UserId,
                UserRoles.Organizer))
            .WithMessage("Only organizers can create events");
    }
}
