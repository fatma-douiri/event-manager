namespace EventManager.Domain.Model;

public class EventRegistration
{
    public int Id { get; set; }
    public int EventId { get; set; }
    public required string UserId { get; set; }
    public DateTime RegisteredAt { get; set; }

    public Event? Event { get; set; }
    public ApplicationUser? User { get; set; }
}
