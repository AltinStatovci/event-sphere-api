using EventSphere.Domain.DTOs.User;
using EventSphere.Domain.Entities;

namespace EventSphere.Business.Services.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User> GetUserByIdAsync(int id);
        Task UpdateUserAsync(UpdateUserDTO updateUserDto);
        Task DeleteUserAsync(int id);
        Task<int> GetUserCountAsync();
    }
}