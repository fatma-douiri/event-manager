using EventManager.Application.Common.Interfaces;

namespace EventManager.Infrastructure.Services;

public class DateTimeService : IDateTimeService
{
    public DateTime Now => DateTime.UtcNow;
}
