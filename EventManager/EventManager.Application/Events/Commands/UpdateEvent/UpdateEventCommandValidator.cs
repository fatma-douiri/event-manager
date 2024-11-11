using System.Globalization;
using FluentValidation;
using EventManager.Domain.Enums;
using EventManager.Application.Common.Interfaces;
using EventManager.Domain.Repositories;

namespace EventManager.Application.Events.Commands.UpdateEvent;
/// <summary>
/// Validator for the UpdateEventCommand.
/// </summary>
public class UpdateEventCommandValidator : AbstractValidator<UpdateEventCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateEventCommandValidator"/> class.
    /// </summary>
    /// <param name="eventRepository">The event repository.</param>
    /// <param name="dateTimeService">The date time service.</param>
    /// <param name="currentUserService">The current user service.</param>
    public UpdateEventCommandValidator(
        IEventRepository eventRepository,
        IDateTimeService dateTimeService,
        ICurrentUserService currentUserService)
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("Invalid event ID");

        RuleFor(x => x.Title)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.Description)
            .NotEmpty()
            .MaximumLength(2000);

        RuleFor(x => x.Location)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.MaxCapacity)
            .GreaterThan(0);

        //Check date format
        RuleFor(x => x.StartDate)
            .Matches(@"^(0[1-9]|[12][0-9]|3[01])/(0[1-9]|1[012])/\d{4}$")
            .WithMessage("Start date must be in format dd/MM/yyyy");

        RuleFor(x => x.EndDate)
            .Matches(@"^(0[1-9]|[12][0-9]|3[01])/(0[1-9]|1[012])/\d{4}$")
            .WithMessage("End date must be in format dd/MM/yyyy");

        //Check time format
        RuleFor(x => x.StartTime)
            .Matches(@"^([01]?[0-9]|2[0-3]):[0-5][0-9]$")
            .WithMessage("Start time must be in format HH:mm");

        RuleFor(x => x.EndTime)
            .Matches(@"^([01]?[0-9]|2[0-3]):[0-5][0-9]$")
            .WithMessage("End time must be in format HH:mm");

        // Check if event starts in the future
        RuleFor(x => x)
            .Must(x => {
                if (x.Status != EventStatus.Active) return true;

                if (!DateTime.TryParseExact(x.StartDate, "dd/MM/yyyy", CultureInfo.InvariantCulture,
                    DateTimeStyles.None, out DateTime startDate))
                    return false;

                if (!TimeSpan.TryParse(x.StartTime, out TimeSpan startTime))
                    return false;

                var startDateTime = startDate.Add(startTime);
                return startDateTime > dateTimeService.Now;
            })
            .When(x => x.Status == EventStatus.Active)
            .WithMessage("Event must start in the future if it's active");

        //Check if event end time is after start time
        RuleFor(x => x)
            .Must(x => {
                if (!DateTime.TryParseExact(x.StartDate, "dd/MM/yyyy", CultureInfo.InvariantCulture,
                    DateTimeStyles.None, out DateTime startDate))
                    return false;

                if (!DateTime.TryParseExact(x.EndDate, "dd/MM/yyyy", CultureInfo.InvariantCulture,
                    DateTimeStyles.None, out DateTime endDate))
                    return false;

                if (!TimeSpan.TryParse(x.StartTime, out TimeSpan startTime))
                    return false;

                if (!TimeSpan.TryParse(x.EndTime, out TimeSpan endTime))
                    return false;

                var startDateTime = startDate.Add(startTime);
                var endDateTime = endDate.Add(endTime);

                return endDateTime > startDateTime;
            })
            .WithMessage("Event end time must be after start time");

        // Check if the user the organizer 
        RuleFor(x => x)
            .MustAsync(async (command, _) => {
                var @event = await eventRepository.GetByIdAsync(command.Id);
                return @event?.OrganizerId == currentUserService.UserId;
            })
            .WithMessage("Only the organizer can update this event");
    }
}