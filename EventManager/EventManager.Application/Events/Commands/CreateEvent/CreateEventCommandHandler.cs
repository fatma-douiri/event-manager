using MediatR;
using EventManager.Domain.Model;
using EventManager.Domain.Repositories;
using EventManager.Application.Common.Interfaces;
using EventManager.Domain.Enums;
using EventManager.Domain.Exceptions;
using System.Globalization;

namespace EventManager.Application.Events.Commands.CreateEvent;

/// <summary>
/// Handles the creation of a new event.
/// </summary>
public class CreateEventCommandHandler(IEventRepository eventRepository,
        ICurrentUserService currentUserService,
        IUserRepository userRepository) : IRequestHandler<CreateEventCommand, int>
{
  
    /// <summary>
    /// Handles the creation of a new event.
    /// </summary>
    /// <param name="request">The create event command request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The ID of the created event.</returns>
    /// <exception cref="NotFoundException">Thrown when the organizer is not found.</exception>
    public async Task<int> Handle(CreateEventCommand request, CancellationToken cancellationToken)
    {
        // Récupérer l'organisateur
        var organizer = await userRepository.GetByIdAsync(currentUserService.UserId)
            ?? throw new NotFoundException(nameof(ApplicationUser), currentUserService.UserId);

        // Parse dates and hours
        DateTime startDate = DateTime.ParseExact(request.StartDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
        TimeSpan startTime = TimeSpan.Parse(request.StartTime);
        DateTime endDate = DateTime.ParseExact(request.EndDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
        TimeSpan endTime = TimeSpan.Parse(request.EndTime);

        
        var startDateTime = startDate.Add(startTime);
        var endDateTime = endDate.Add(endTime);

        if (startDateTime <= DateTime.Now)
        {
            throw new BusinessRuleException("La date de début doit être dans le futur");
        }

        if (endDateTime <= startDateTime)
        {
            throw new BusinessRuleException("La date de fin doit être après la date de début");
        }

        if (request.MaxCapacity <= 0)
        {
            throw new BusinessRuleException("La capacité maximale doit être supérieure à 0");
        }

        var @event = new Event
        {
            Title = request.Title,
            Description = request.Description,
            Location = request.Location,
            StartDate = startDate,
            StartTime = startTime,
            EndDate = endDate,
            EndTime = endTime,
            MaxCapacity = request.MaxCapacity,
            Status = EventStatus.Active,
            OrganizerId = currentUserService.UserId,
            Organizer = organizer
        };

        var createdEvent = await eventRepository.AddAsync(@event);
        return createdEvent.Id;
    }
}