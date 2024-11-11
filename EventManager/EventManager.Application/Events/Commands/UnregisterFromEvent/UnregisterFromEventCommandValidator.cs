using FluentValidation;
using EventManager.Domain.Enums;
using EventManager.Application.Common.Interfaces;
using EventManager.Domain.Repositories;

namespace EventManager.Application.Events.Commands.UnregisterFromEvent;

/// <summary>
/// Validator for the UnregisterFromEventCommand.
/// </summary>
public class UnregisterFromEventCommandValidator : AbstractValidator<UnregisterFromEventCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UnregisterFromEventCommandValidator"/> class.
    /// </summary>
    /// <param name="eventRepository">The event repository.</param>
    /// <param name="registrationRepository">The registration repository.</param>
    /// <param name="currentUserService">The current user service.</param>
    public UnregisterFromEventCommandValidator(
        IEventRepository eventRepository,
        IEventRegistrationRepository registrationRepository,
        ICurrentUserService currentUserService)
    {
        RuleFor(x => x.EventId)
            .GreaterThan(0)
            .WithMessage("Invalid event ID");

        // ChecK if event exists and is active
        RuleFor(x => x.EventId)
            .MustAsync(async (eventId, cancellation) =>
            {
                var @event = await eventRepository.GetByIdAsync(eventId);
                return @event != null && @event.Status == EventStatus.Active;
            })
            .WithMessage("Event must exist and be active");

        // ChecK if user is registered
        RuleFor(x => x.EventId)
            .MustAsync(async (eventId, cancellation) =>
            {
                return await registrationRepository.IsRegisteredAsync(
                    eventId,
                    currentUserService.UserId
                );
            })
            .WithMessage("You are not registered for this event");
    }
}
