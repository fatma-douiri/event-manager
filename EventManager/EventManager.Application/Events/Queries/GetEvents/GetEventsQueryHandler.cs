using AutoMapper;
using MediatR;
using EventManager.Domain.Model;
using EventManager.Domain.Repositories;
using EventManager.Application.Common.Models;
using EventManager.Application.Events.DTOs;
using EventManager.Application.Common.Interfaces;

namespace EventManager.Application.Events.Queries.GetEvents;

/// <summary>
/// Handles the GetEventsQuery request and returns a paginated list of EventDto.
/// </summary>
/// <param name="dateTimeService"></param>
/// <param name="eventRepository"></param>
/// <param name="mapper"></param>
public class GetEventsQueryHandler(IEventRepository eventRepository,
        IMapper mapper,
        IDateTimeService dateTimeService) : IRequestHandler<GetEventsQuery, PaginatedList<EventDto>>
{
    private readonly IEventRepository _eventRepository = eventRepository;
    private readonly IMapper _mapper = mapper;
    private readonly IDateTimeService _dateTimeService = dateTimeService;



    /// <summary>
    /// Handles the GetEventsQuery request and returns a paginated list of EventDto.
    /// </summary>
    /// <param name="request">The GetEventsQuery request containing the filters and pagination parameters.</param>
    /// <param name="cancellationToken">The cancellation token to cancel the operation.</param>
    /// <returns>A paginated list of EventDto.</returns>
    public async Task<PaginatedList<EventDto>> Handle(GetEventsQuery request, CancellationToken cancellationToken)
    {
        IEnumerable<Event> events;

        // Recuparate the events based on the filters
        if (!string.IsNullOrEmpty(request.Location) && request.Status.HasValue)
        {
            events = await _eventRepository.GetEventsByLocationAndStatusAsync(request.Location, request.Status.Value);
        }
        if (!string.IsNullOrEmpty(request.Location))
        {
            events = await _eventRepository.GetEventsByLocationAsync(request.Location);
        }
        else if (request.Status.HasValue) 
        {
            events = await _eventRepository.GetEventsByStatusAsync(request.Status.Value);
        }
        else if (request.UpcomingOnly)
        {
            events = await _eventRepository.GetUpcomingEventsAsync();
        }
        else
        {
            events = await _eventRepository.GetAllAsync();
        }

        // Convert the events to DTOs
        var dtos = _mapper.Map<List<EventDto>>(events);

        // Add pagination to the list
        var count = dtos.Count;
        var items = dtos
            .Skip((request.Pagination.PageNumber - 1) * request.Pagination.PageSize)
            .Take(request.Pagination.PageSize)
            .ToList();

        return new PaginatedList<EventDto>(items, count, request.Pagination.PageNumber, request.Pagination.PageSize);
    }
}