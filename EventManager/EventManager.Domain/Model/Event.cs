using EventManager.Domain.Enums;

namespace EventManager.Domain.Model;

public class Event( )
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public required string Description { get; set; }
    public required string Location { get; set; }
    public DateTime StartDate { get; set; }
    public TimeSpan StartTime { get; set; }
    public DateTime EndDate { get; set; }
    public TimeSpan EndTime { get; set; }
    public int MaxCapacity { get; set; }
    public EventStatus Status { get; set; }
    public required string OrganizerId { get; set; }
    public required ApplicationUser Organizer { get; set; } 
    public ICollection<EventRegistration> Registrations { get; set; } = new List<EventRegistration>();
    
}