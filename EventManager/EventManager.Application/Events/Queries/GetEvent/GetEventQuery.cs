using EventManager.Application.Events.DTOs;
using MediatR;

namespace EventManager.Application.Events.Queries.GetEvent;

/// <summary>
/// Repr�sente une requ�te pour obtenir un �v�nement par son identifiant.
/// </summary>
public record GetEventQuery : IRequest<EventDto>
{
    /// <summary>
    /// Obtient ou d�finit l'identifiant de l'�v�nement.
    /// </summary>
    public required int Id { get; init; }
}
