using MediatR;
using EventManager.Domain.Enums;

namespace EventManager.Application.Events.Commands.UpdateEvent;

/// <summary>
/// Command to update an event.
/// </summary>
public record UpdateEventCommand : IRequest<Unit>, IBaseRequest, IEquatable<UpdateEventCommand>
{
    /// <summary>
    /// Gets or sets the event ID.
    /// </summary>
    public required int Id { get; init; }

    /// <summary>
    /// Gets or sets the event title.
    /// </summary>
    public required string Title { get; init; }

    /// <summary>
    /// Gets or sets the event description.
    /// </summary>
    public required string Description { get; init; }

    /// <summary>
    /// Gets or sets the event location.
    /// </summary>
    public required string Location { get; init; }

    /// <summary>
    /// Gets or sets the event start date.
    /// </summary>
    public required string StartDate { get; init; }

    /// <summary>
    /// Gets or sets the event start time.
    /// </summary>
    public required string StartTime { get; init; }

    /// <summary>
    /// Gets or sets the event end date.
    /// </summary>
    public required string EndDate { get; init; }

    /// <summary>
    /// Gets or sets the event end time.
    /// </summary>
    public required string EndTime { get; init; }

    /// <summary>
    /// Gets or sets the maximum capacity of the event.
    /// </summary>
    public required int MaxCapacity { get; init; }

    /// <summary>
    /// Gets or sets the event status.
    /// </summary>
    public EventStatus Status { get; init; }
}
