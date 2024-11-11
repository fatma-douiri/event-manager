using Microsoft.EntityFrameworkCore;
using EventManager.Domain.Model;
using EventManager.Domain.Repositories;
using EventManager.Infrastructure.Persistence;
using EventManager.Domain.Enums;

namespace EventManager.Infrastructure.Repositories;

public class EventRepository : IEventRepository
{
    private readonly ApplicationDbContext _context;

    public EventRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Event?> GetByIdAsync(int id)
    {
        return await _context.Events
            .Include(e => e.Organizer)
            .Include(e => e.Registrations)
            .ThenInclude(r => r.User)
            .FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task<IEnumerable<Event>> GetAllAsync()
    {
        return await _context.Events
            .Include(e => e.Organizer)
            .Include(e => e.Registrations)
            .OrderByDescending(e => e.StartDate)
            .ToListAsync();
    }

    public async Task<Event> AddAsync(Event @event)
    {
        _context.Events.Add(@event);
        await _context.SaveChangesAsync();
        return @event;
    }

    public async Task UpdateAsync(Event @event)
    {
        _context.Entry(@event).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.Events.AnyAsync(e => e.Id == id);
    }

    public async Task<IEnumerable<Event>> GetUserEventsAsync(string userId)
    {
        return await _context.Events
            .Include(e => e.Organizer)
            .Include(e => e.Registrations)
            .Where(e => e.Registrations.Any(r => r.UserId == userId))
            .OrderByDescending(e => e.StartDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<Event>> GetOrganizerEventsAsync(string organizerId)
    {
        return await _context.Events
            .Include(e => e.Registrations)
            .ThenInclude(r => r.User)
            .Where(e => e.OrganizerId == organizerId)
            .OrderByDescending(e => e.StartDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<Event>> GetUpcomingEventsAsync()
    {
        return await _context.Events
            .Where(e => e.StartDate > DateTime.UtcNow)
            .OrderBy(e => e.StartDate)
            .ToListAsync();
    }
  public async Task<IEnumerable<Event>> GetUserRegisteredEventsAsync(string userId)
    {
        return await _context.Events
            .Include(e => e.Registrations)
            .Where(e => e.Registrations.Any(r => r.UserId == userId))
            .OrderByDescending(e => e.StartDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<Event>> GetEventsByLocationAsync(string location)
    {
        return await _context.Events
            .Where(e => e.Location.Contains(location))
            .OrderByDescending(e => e.StartDate)
            .ToListAsync();
    }

  
    public async Task<IEnumerable<Event>> GetEventsByStatusAsync(EventStatus status)
    {
        return await _context.Events
            .Where(e => e.Status == status)
            .OrderByDescending(e => e.StartDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<Event>> GetEventsByLocationAndStatusAsync(string location, EventStatus status)
    {
        return await _context.Events
            .Where(e => e.Location.Contains(location) && e.Status == (EventStatus)status)
            .OrderByDescending(e => e.StartDate)
            .ToListAsync();
    }

}