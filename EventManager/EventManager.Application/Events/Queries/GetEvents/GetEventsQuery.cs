using MediatR;
using EventManager.Application.Common.Models;
using EventManager.Application.Events.DTOs;
using EventManager.Domain.Enums;

namespace EventManager.Application.Events.Queries.GetEvents;

/// <summary>
/// Query to get a paginated list of events.
/// </summary>
public record GetEventsQuery : IRequest<PaginatedList<EventDto>>
{
    /// <summary>
    /// The location to filter events by.
    /// </summary>
    public string? Location { get; init; }
    /// <summary>
    /// The status to filter events by.
    /// </summary>
    public EventStatus? Status { get; init; }
    /// <summary>
    /// Whether to filter only upcoming events.
    /// </summary>
    public bool UpcomingOnly { get; init; }

    /// <summary>
    /// Pagination parameters for the query.
    /// </summary>
    public required PaginationParams Pagination { get; init; }
}
