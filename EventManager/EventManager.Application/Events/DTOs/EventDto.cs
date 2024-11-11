using EventManager.Domain.Enums;
using EventManager.Application.Users.DTOs;


namespace EventManager.Application.Events.DTOs;

/// <summary>
/// Data Transfer Object for Event.
/// </summary>
public class EventDto
{
    /// <summary>
    /// Gets or sets the unique identifier for the event.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the title of the event.
    /// </summary>
    public required string Title { get; set; }

    /// <summary>
    /// Gets or sets the description of the event.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets the location of the event.
    /// </summary>
    public required string Location { get; set; }

    /// <summary>
    /// Gets or sets the start date of the event.
    /// </summary>
    public required DateTime StartDate { get; set; }

    /// <summary>
    /// Gets or sets the start time of the event.
    /// </summary>
    public required TimeSpan StartTime { get; set; }

    /// <summary>
    /// Gets or sets the end date of the event.
    /// </summary>
    public required DateTime EndDate { get; set; }

    /// <summary>
    /// Gets or sets the end time of the event.
    /// </summary>
    public required TimeSpan EndTime { get; set; }

    /// <summary>
    /// Gets or sets the maximum capacity of the event.
    /// </summary>
    public required int MaxCapacity { get; set; }

    /// <summary>
    /// Gets or sets the current number of participants in the event.
    /// </summary>
    public int CurrentParticipants { get; set; }

    /// <summary>
    /// Gets or sets the name of the organizer of the event.
    /// </summary>
    public required string OrganizerName { get; set; }

    /// <summary>
    /// Gets or sets the status of the event.
    /// </summary>
    public required EventStatus Status { get; set; }

    /// <summary>
    /// Gets or sets the list of participants in the event.
    /// </summary>
    public List<UserDto> Participants { get; set; } = new List<UserDto>();
}
