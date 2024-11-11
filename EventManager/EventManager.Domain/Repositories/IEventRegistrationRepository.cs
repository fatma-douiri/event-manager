using EventManager.Domain.Model;

namespace EventManager.Domain.Repositories;

public interface IEventRegistrationRepository
{
    Task<EventRegistration> RegisterAsync(int eventId, string userId);
    Task UnregisterAsync(int eventId, string userId);
    Task<bool> IsRegisteredAsync(int eventId, string userId);
    Task<int> GetEventRegistrationsCountAsync(int eventId);
    Task<IEnumerable<EventRegistration>> GetEventRegistrationsAsync(int eventId);
    Task<IEnumerable<EventRegistration>> GetUserRegistrationsAsync(string userId);
}