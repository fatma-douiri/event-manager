using MediatR;
using EventManager.Domain.Exceptions;
using EventManager.Domain.Model;
using EventManager.Application.Common.Interfaces;
using EventManager.Domain.Enums;
using EventManager.Domain.Repositories;

namespace EventManager.Application.Events.Commands.UnregisterFromEvent;

public class UnregisterFromEventCommandHandler (
                    IEventRepository eventRepository,
                    IEventRegistrationRepository registrationRepository,
                    ICurrentUserService currentUserService,
                    IDateTimeService dateTimeService) : IRequestHandler<UnregisterFromEventCommand, Unit>
{
   
    public async Task<Unit> Handle(UnregisterFromEventCommand request, CancellationToken cancellationToken)
    {
        var @event = await eventRepository.GetByIdAsync(request.EventId)
            ?? throw new NotFoundException(nameof(Event), request.EventId);

        if (@event.Status != EventStatus.Active)
            throw new BusinessRuleException("Impossible de se désinscrire d'un événement non actif");

        if (!await registrationRepository.IsRegisteredAsync(request.EventId, currentUserService.UserId))
            throw new BusinessRuleException("Vous n'êtes pas inscrit à cet événement");

        var eventDateTime = @event.StartDate.Add(@event.StartTime);
        if (eventDateTime <= dateTimeService.Now)
            throw new BusinessRuleException("Impossible de se désinscrire d'un événement déjà commencé");

        await registrationRepository.UnregisterAsync(request.EventId,currentUserService.UserId);
        return Unit.Value;
    }
}