using MediatR;

namespace EventManager.Application.Events.Commands.RegisterToEvent;

/// <summary>
/// Command to register to an event.
/// </summary>
public record RegisterToEventCommand : IRequest<Unit>, IBaseRequest, IEquatable<RegisterToEventCommand>
{
    /// <summary>
    /// Gets or sets the event ID.
    /// </summary>
    public required int EventId { get; init; }
}
