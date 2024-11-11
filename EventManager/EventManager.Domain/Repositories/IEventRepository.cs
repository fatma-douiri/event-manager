using EventManager.Domain.Model;
using EventManager.Domain.Enums;

namespace EventManager.Domain.Repositories;

public interface IEventRepository
{
    Task<Event?> GetByIdAsync(int id);
    Task<IEnumerable<Event>> GetAllAsync();
    Task<Event> AddAsync(Event @event);
    Task UpdateAsync(Event @event);
    Task<bool> ExistsAsync(int id);
    Task<IEnumerable<Event>> GetUserEventsAsync(string userId);
    Task<IEnumerable<Event>> GetOrganizerEventsAsync(string organizerId);
    Task<IEnumerable<Event>> GetUpcomingEventsAsync();
    Task<IEnumerable<Event>> GetEventsByLocationAndStatusAsync(string location, EventStatus status);
    Task<IEnumerable<Event>> GetEventsByLocationAsync(string location);
    Task<IEnumerable<Event>> GetEventsByStatusAsync(EventStatus status);
    Task<IEnumerable<Event>> GetUserRegisteredEventsAsync(string userId);



}