using Microsoft.EntityFrameworkCore;
using EventManager.Application.Common.Interfaces;
using EventManager.Domain.Model;
using EventManager.Domain.Repositories;
using EventManager.Infrastructure.Persistence;


namespace EventManager.Infrastructure.Repositories;

public class EventRegistrationRepository : IEventRegistrationRepository
{
    private readonly ApplicationDbContext _context;
    private readonly IDateTimeService _dateTimeService;

    public EventRegistrationRepository(
        ApplicationDbContext context,
        IDateTimeService dateTimeService)
    {
        _context = context;
        _dateTimeService = dateTimeService;
    }

    public async Task<EventRegistration> RegisterAsync(int eventId, string userId)
    {
        var eventEntity = await _context.Events.FindAsync(eventId);
        var userEntity = await _context.Users.FindAsync(userId);

        if (eventEntity == null || userEntity == null)
        {
            throw new ArgumentException("Event or User not found.");
        }

        var registration = new EventRegistration
        {
            EventId = eventId,
            UserId = userId,
            RegisteredAt = _dateTimeService.Now
        };

        _context.EventRegistrations.Add(registration);
        await _context.SaveChangesAsync();
        return registration;
    }

    public async Task UnregisterAsync(int eventId, string userId)
    {
        var registration = await _context.EventRegistrations
            .FirstOrDefaultAsync(r => r.EventId == eventId && r.UserId == userId);

        if (registration != null)
        {
            _context.EventRegistrations.Remove(registration);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> IsRegisteredAsync(int eventId, string userId)
    {
        return await _context.EventRegistrations
            .AnyAsync(r => r.EventId == eventId && r.UserId == userId);
    }

    public async Task<int> GetEventRegistrationsCountAsync(int eventId)
    {
        return await _context.EventRegistrations
            .CountAsync(r => r.EventId == eventId);
    }

    public async Task<IEnumerable<EventRegistration>> GetEventRegistrationsAsync(int eventId)
    {
        return await _context.EventRegistrations
            .Include(r => r.User)
            .Where(r => r.EventId == eventId)
            .ToListAsync();
    }

    public async Task<IEnumerable<EventRegistration>> GetUserRegistrationsAsync(string userId)
    {
        return await _context.EventRegistrations
            .Include(r => r.Event)
            .Where(r => r.UserId == userId)
            .ToListAsync();
    }
}