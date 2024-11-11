using FluentValidation;
using EventManager.Domain.Enums;
using EventManager.Application.Common.Interfaces;
using EventManager.Domain.Repositories;

namespace EventManager.Application.Events.Commands.RegisterToEvent;

/// <summary>
/// Validator for the RegisterToEventCommand.
/// </summary>
public class RegisterToEventCommandValidator : AbstractValidator<RegisterToEventCommand>
{
    private readonly IEventRepository _eventRepository;
    private readonly IEventRegistrationRepository _registrationRepository;
    private readonly ICurrentUserService _currentUserService;

    /// <summary>
    /// Initializes a new instance of the <see cref="RegisterToEventCommandValidator"/> class.
    /// </summary>
    /// <param name="eventRepository">The event repository.</param>
    /// <param name="registrationRepository">The registration repository.</param>
    /// <param name="currentUserService">The current user service.</param>
    public RegisterToEventCommandValidator(
        IEventRepository eventRepository,
        IEventRegistrationRepository registrationRepository,
        ICurrentUserService currentUserService)
    {
        _eventRepository = eventRepository;
        _registrationRepository = registrationRepository;
        _currentUserService = currentUserService;

        RuleFor(x => x.EventId)
            .GreaterThan(0)
            .WithMessage("Invalid event ID");

        // Check if the event exists and is active  
        RuleFor(x => x.EventId)
            .MustAsync(async (eventId, cancellation) =>
            {
                var @event = await _eventRepository.GetByIdAsync(eventId);
                return @event != null && @event.Status == EventStatus.Active;
            })
            .WithMessage("Event must exist and be active");

        // Check if the user is already registered
        RuleFor(x => x.EventId)
            .MustAsync(async (eventId, cancellation) =>
            {
                return !await _registrationRepository.IsRegisteredAsync(
                    eventId,
                    _currentUserService.UserId
                );
            })
            .WithMessage("You are already registered for this event");

        // Check if the event has reached its maximum capacity
        RuleFor(x => x.EventId)
            .MustAsync(async (eventId, cancellation) =>
            {
                var @event = await _eventRepository.GetByIdAsync(eventId);
                if (@event == null) return false;

                var currentRegistrations = await _registrationRepository
                    .GetEventRegistrationsCountAsync(eventId);

                return currentRegistrations < @event.MaxCapacity;
            })
            .WithMessage("Event has reached its maximum capacity");
    }
}
