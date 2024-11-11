using MediatR;

namespace EventManager.Application.Events.Commands.CreateEvent;

/// <summary>
/// Command to create a new event.
/// </summary>
public record CreateEventCommand : IRequest<int>, IBaseRequest, IEquatable<CreateEventCommand>
{
    /// <summary>
    /// Gets or sets the title of the event.
    /// </summary>
    public required string Title { get; init; }

    /// <summary>
    /// Gets or sets the description of the event.
    /// </summary>
    public required string Description { get; init; }

    /// <summary>
    /// Gets or sets the location of the event.
    /// </summary>
    public required string Location { get; init; }

    /// <summary>
    /// Gets or sets the start date of the event in dd/MM/yyyy format.
    /// </summary>
    public required string StartDate { get; init; }

    /// <summary>
    /// Gets or sets the start time of the event in HH:mm format.
    /// </summary>
    public required string StartTime { get; init; }

    /// <summary>
    /// Gets or sets the end date of the event in dd/MM/yyyy format.
    /// </summary>
    public required string EndDate { get; init; }

    /// <summary>
    /// Gets or sets the end time of the event in HH:mm format.
    /// </summary>
    public required string EndTime { get; init; }

    /// <summary>
    /// Gets or sets the maximum capacity of the event.
    /// </summary>
    public required int MaxCapacity { get; init; }
}
