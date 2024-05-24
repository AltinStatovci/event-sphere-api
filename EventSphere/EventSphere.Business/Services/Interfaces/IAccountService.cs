using EventSphere.Domain.DTOs.User;
using EventSphere.Domain.Entities;

namespace EventSphere.Business.Services.Interfaces
{
    public interface IAccountService
    {
        Task<UserDTO> AddUserAsync(CreateUserDTO createUserDto);
        Task<string> AuthenticateAsync(LoginDTO loginDto);
    }
}
