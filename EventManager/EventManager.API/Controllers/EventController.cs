using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FluentValidation;
using EventManager.Domain.Constants;
using EventManager.Application.Events.Commands.CreateEvent;
using EventManager.Application.Events.Commands.UpdateEvent;
using EventManager.Application.Events.Commands.RegisterToEvent;
using EventManager.Application.Events.Queries.GetEvent;
using EventManager.Application.Events.Queries.GetEvents;
using EventManager.Application.Common.Models;
using EventManager.Application.Events.DTOs;
using EventManager.Domain.Exceptions;
using EventManager.Application.Events.Commands.UnregisterFromEvent;
using EventManager.Domain.Enums;
using EventManager.Application.Events.Queries.GetParticipantsOfEvent;
using EventManager.Application.Users.DTOs;

namespace EventManager.API.Controllers;

/// <summary>
/// Events management
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class EventController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Get all events with optional filtering
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(PaginatedList<EventDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<PaginatedList<EventDto>>> GetEvents(
        [FromQuery] string? location,
        [FromQuery] EventStatus? status,
        [FromQuery] bool upcomingOnly = false,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        try
        {
            var query = new GetEventsQuery
            {
                Location = location,
                Status = status,
                UpcomingOnly = upcomingOnly,
                Pagination = new PaginationParams
                {
                    PageNumber = pageNumber,
                    PageSize = pageSize
                }
            };

            var result = await mediator.Send(query);
            return Ok(new { message = "Liste des événements récupérée avec succès", data = result });
        }
        catch (ValidationException ex)
        {
            return BadRequest(new { errors = ex.Errors.Select(e => e.ErrorMessage) });
        }
        catch (BusinessRuleException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Get event by ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(EventDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<EventDto>> GetEvent(int id)
    {
        try
        {
            var query = new GetEventQuery { Id = id };
            var result = await mediator.Send(query);

            if (result == null)
                return NotFound(new { message = "Événement non trouvé" });

            return Ok(new { message = "Événement récupéré avec succès", data = result });
        }
        catch (ValidationException ex)
        {
            return BadRequest(new { errors = ex.Errors.Select(e => e.ErrorMessage) });
        }
        catch (BusinessRuleException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Create a new event
    /// </summary>
    [HttpPost]
    [Authorize(Roles = UserRoles.Organizer)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<int>> CreateEvent(CreateEventCommand command)
    {
        try
        {
            var result = await mediator.Send(command);
            return CreatedAtAction(nameof(GetEvent),
                new { id = result },
                new { message = "Événement créé avec succès", data = result });
        }
        catch (ValidationException ex)
        {
            return BadRequest(new { errors = ex.Errors.Select(e => e.ErrorMessage) });
        }
        catch (BusinessRuleException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
    }

    /// <summary>
    /// Update an existing event
    /// </summary>
    [HttpPut("{id}")]
    [Authorize(Roles = UserRoles.Organizer)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> UpdateEvent(int id, UpdateEventCommand command)
    {
        if (id != command.Id)
            return BadRequest(new { message = "L'identifiant de l'événement ne correspond pas" });

        try
        {
            await mediator.Send(command);
            return Ok(new { message = "Événement mis à jour avec succès" });
        }
        catch (ValidationException ex)
        {
            return BadRequest(new { errors = ex.Errors.Select(e => e.ErrorMessage) });
        }
        catch (BusinessRuleException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (NotFoundException)
        {
            return NotFound(new { message = "Événement non trouvé" });
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
    }

    /// <summary>
    /// Register current user to an event
    /// </summary>
    [HttpPost("{id}/register")]
    [Authorize(Roles = UserRoles.Participant)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> RegisterToEvent(int id)
    {
        try
        {
            await mediator.Send(new RegisterToEventCommand { EventId = id });
            return Ok(new { message = "Inscription à l'événement réussie" });
        }
        catch (ValidationException ex)
        {
            return BadRequest(new { errors = ex.Errors.Select(e => e.ErrorMessage) });
        }
        catch (BusinessRuleException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (NotFoundException)
        {
            return NotFound(new { message = "Événement non trouvé" });
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
    }

    /// <summary>
    /// Unregister current user from an event
    /// </summary>
    [HttpDelete("{id}/register")]
    [Authorize(Roles = UserRoles.Participant)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> UnregisterFromEvent(int id)
    {
        try
        {
            await mediator.Send(new UnregisterFromEventCommand { EventId = id });
            return Ok(new { message = "Désinscription de l'événement réussie" });
        }
        catch (ValidationException ex)
        {
            return BadRequest(new { errors = ex.Errors.Select(e => e.ErrorMessage) });
        }
        catch (BusinessRuleException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (NotFoundException)
        {
            return NotFound(new { message = "Événement non trouvé" });
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
    }


    /// <summary>
    /// Get all participants for a specific event
    /// </summary>
    /// <param name="eventId">The ID of the event</param>
    /// <returns>A list of participants for the specified event</returns>
    [HttpGet("events/{eventId}/participants")]
    [ProducesResponseType(typeof(List<UserDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> GetEventParticipants(int eventId)
    {
        try
        {
            var query = new GetParticipantsOfEventQuery { EventId = eventId };
            var participants = await mediator.Send(query);

            if (participants == null || !participants.Any())
            {
                return NotFound(new { message = "Aucun participant trouvé pour cet événement." });
            }

            return Ok(new { message = "Liste des participants récupérée avec succès", data = participants });
        }
        catch (ValidationException ex)
        {
            return BadRequest(new { errors = ex.Errors.Select(e => e.ErrorMessage) });
        }
        catch (NotFoundException)
        {
            return NotFound(new { message = "Événement non trouvé" });
        }
    }


}