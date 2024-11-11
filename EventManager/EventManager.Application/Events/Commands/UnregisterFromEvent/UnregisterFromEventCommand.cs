using MediatR;

namespace EventManager.Application.Events.Commands.UnregisterFromEvent;

public record UnregisterFromEventCommand : IRequest<Unit>
{
    public required int EventId { get; init; }
}