using EventManager.Domain.Model;

namespace EventManager.Domain.Repositories;

public interface IUserRepository
{
    Task<ApplicationUser?> GetByIdAsync(string id);
    Task<ApplicationUser?> GetByEmailAsync(string email);
    Task<IEnumerable<ApplicationUser>> GetAllAsync();
    Task<bool> IsInRoleAsync(string userId, string role);
}