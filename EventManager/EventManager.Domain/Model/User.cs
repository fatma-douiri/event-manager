using Microsoft.AspNetCore.Identity;

namespace EventManager.Domain.Model;

public class ApplicationUser : IdentityUser
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    
    public ICollection<EventRegistration> EventRegistrations { get; set; } = new List<EventRegistration>();
    
}