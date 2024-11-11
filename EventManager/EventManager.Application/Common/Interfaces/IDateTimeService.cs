namespace EventManager.Application.Common.Interfaces;

/// <summary>
/// Service to provide the current date and time.
/// </summary>
public interface IDateTimeService
{
    /// <summary>
    /// Gets the current UTC date and time.
    /// </summary>
    /// <value>A DateTime representing the current UTC time.</value>
    DateTime Now { get; }
}