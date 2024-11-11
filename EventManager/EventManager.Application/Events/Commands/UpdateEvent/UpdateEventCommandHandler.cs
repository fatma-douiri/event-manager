using MediatR;
using EventManager.Domain.Enums;
using EventManager.Domain.Exceptions;
using EventManager.Application.Common.Interfaces;
using EventManager.Domain.Model;
using EventManager.Domain.Repositories;
using System.Globalization;

namespace EventManager.Application.Events.Commands.UpdateEvent;

/// <summary>
/// Handles the update event command.
/// </summary>
public class UpdateEventCommandHandler(IEventRepository eventRepository,
        IEventRegistrationRepository registrationRepository,
        ICurrentUserService currentUserService,
        IDateTimeService dateTimeService) : IRequestHandler<UpdateEventCommand, Unit>
{
  
    /// <summary>
    /// Handles the update event command.
    /// </summary>
    /// <param name="request">The update event command request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A unit value.</returns>
    public async Task<Unit> Handle(UpdateEventCommand request, CancellationToken cancellationToken)
    {
        var @event = await eventRepository.GetByIdAsync(request.Id)
            ?? throw new NotFoundException(nameof(Event), request.Id);

        if (@event.OrganizerId != currentUserService.UserId)
            throw new UnauthorizedAccessException("Only the organizer can update this event");

        // Parse Dates and Hours
        DateTime startDate = DateTime.ParseExact(request.StartDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
        TimeSpan startTime = TimeSpan.Parse(request.StartTime);
        DateTime endDate = DateTime.ParseExact(request.EndDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
        TimeSpan endTime = TimeSpan.Parse(request.EndTime);

        var startDateTime = startDate.Add(startTime);
        var endDateTime = endDate.Add(endTime);

        // Si l'événement est déjà terminé
        if (endDateTime < dateTimeService.Now)
        {
            @event.Status = EventStatus.Completed;
            await eventRepository.UpdateAsync(@event);
            throw new BusinessRuleException("L'événement est déjà terminé et ne peut plus être modifié");
        }

        // Vérifications métier
        if (startDateTime <= dateTimeService.Now)
            throw new BusinessRuleException("La date de début doit être dans le futur");

        if (endDateTime <= startDateTime)
            throw new BusinessRuleException("La date de fin doit être après la date de début");

        if (request.MaxCapacity <= 0)
            throw new BusinessRuleException("La capacité maximale doit être supérieure à 0");

        var currentRegistrations = await registrationRepository.GetEventRegistrationsCountAsync(request.Id);
        if (request.MaxCapacity < currentRegistrations)
            throw new BusinessRuleException($"La capacité ne peut pas être inférieure au nombre d'inscrits actuels ({currentRegistrations})");

        // Mise à jour des propriétés
        @event.Title = request.Title;
        @event.Description = request.Description;
        @event.Location = request.Location;
        @event.StartDate = startDate;
        @event.StartTime = startTime;
        @event.EndDate = endDate;
        @event.EndTime = endTime;
        @event.MaxCapacity = request.MaxCapacity;

        if (request.Status == EventStatus.Cancelled)
        {
            @event.Status = EventStatus.Cancelled;
        }

        await eventRepository.UpdateAsync(@event);
        return Unit.Value;
    }
}
