using AutoMapper;
using EventManager.Application.Events.DTOs;
using EventManager.Application.Events.Queries.GetEvent;
using EventManager.Domain.Exceptions;
using EventManager.Domain.Model;
using EventManager.Domain.Repositories;
using MediatR;

namespace EventManager.Application.Events.Queries.GetEvent;

/// <summary>
/// Handles the request to get an event by its ID.
/// </summary>
/// <param name="eventRepository"></param>
/// <param name="mapper"></param>
public class GetEventQueryHandler(IEventRepository eventRepository, IMapper mapper) : IRequestHandler<GetEventQuery, EventDto>
{
    private readonly IEventRepository _eventRepository = eventRepository;
    private readonly IMapper _mapper = mapper;

    
    /// <summary>
    /// Handles the request to get an event by its ID.
    /// </summary>
    /// <param name="request">The request containing the event ID.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The event data transfer object.</returns>
    public async Task<EventDto> Handle(GetEventQuery request, CancellationToken cancellationToken)
    {
        var @event = await _eventRepository.GetByIdAsync(request.Id)
            ?? throw new NotFoundException(nameof(Event), request.Id);

        return _mapper.Map<EventDto>(@event);
    }

}
