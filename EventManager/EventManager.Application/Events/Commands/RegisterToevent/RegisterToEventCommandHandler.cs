using EventManager.Application.Common.Interfaces;
using EventManager.Domain.Enums;
using EventManager.Domain.Exceptions;
using EventManager.Domain.Model;
using EventManager.Domain.Repositories;
using MediatR;

namespace EventManager.Application.Events.Commands.RegisterToEvent;

/// <summary>
/// Handles the registration to an event.
/// </summary>
public class RegisterToEventCommandHandler (IEventRepository eventRepository, 
                    IEventRegistrationRepository registrationRepository,
                    ICurrentUserService currentUserService,
                    IDateTimeService dateTimeService) : IRequestHandler<RegisterToEventCommand, Unit>
{ 

    /// <summary>
    /// Handles the registration to an event.
    /// </summary>
    /// <param name="request">The registration request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task<Unit> Handle(RegisterToEventCommand request, CancellationToken cancellationToken)
    {
        var @event = await eventRepository.GetByIdAsync(request.EventId)
            ?? throw new NotFoundException(nameof(Event), request.EventId);

        // Vérification du statut
        if (@event.Status != EventStatus.Active)
            throw new BusinessRuleException("Impossible de s'inscrire à un événement non actif");

        // Vérification de la date
        var startDateTime = @event.StartDate.Add(@event.StartTime);
        if (startDateTime <= dateTimeService.Now)
            throw new BusinessRuleException("Impossible de s'inscrire à un événement déjà commencé");

        // Vérification inscription existante
        if (await registrationRepository.IsRegisteredAsync(request.EventId, currentUserService.UserId))
            throw new BusinessRuleException("Vous êtes déjà inscrit à cet événement");

        // Vérification capacité
        var currentRegistrations = await registrationRepository.GetEventRegistrationsCountAsync(request.EventId);
        if (currentRegistrations >= @event.MaxCapacity)
            throw new BusinessRuleException("L'événement a atteint sa capacité maximale");

        // Inscription
        await registrationRepository.RegisterAsync(request.EventId, currentUserService.UserId);
        return Unit.Value;
    }
}
