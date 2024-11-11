using EventManager.Application.Events.DTOs;
using MediatR;

namespace EventManager.Application.Events.Queries.GetEvent;

/// <summary>
/// Représente une requête pour obtenir un événement par son identifiant.
/// </summary>
public record GetEventQuery : IRequest<EventDto>
{
    /// <summary>
    /// Obtient ou définit l'identifiant de l'événement.
    /// </summary>
    public required int Id { get; init; }
}
