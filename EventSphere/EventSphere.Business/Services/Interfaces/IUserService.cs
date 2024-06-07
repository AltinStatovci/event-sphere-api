using EventSphere.Domain.Entities;

namespace EventSphere.Business.Services.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User> GetUserByIdAsync(int id);
        Task UpdateUserAsync(User user);
        Task DeleteUserAsync(int id);
        Task<int> GetUserCountAsync();
    }
}