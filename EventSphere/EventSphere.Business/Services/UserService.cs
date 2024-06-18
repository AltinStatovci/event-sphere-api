using EventSphere.Business.Helper;
using EventSphere.Business.Services.Interfaces;
using EventSphere.Domain.DTOs.User;
using EventSphere.Domain.Entities;
using EventSphere.Infrastructure.Repositories.UserRepository;
using MapsterMapper;

namespace EventSphere.Business.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _userRepository.GetAllAsync();
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            return await _userRepository.GetByIdAsync(id);
        }

        public async Task UpdateUserAsync(UpdateUserDTO updateUserDto)
        {
            var existingUser = await _userRepository.GetByIdAsync(updateUserDto.ID);
            if (existingUser == null)
            {
                throw new Exception("User does not exist!");
            }

            _mapper.Map(updateUserDto, existingUser);

            if (!string.IsNullOrWhiteSpace(updateUserDto.Password))
            {
                var passwordSalt = PasswordGenerator.GenerateSalt();
                var passwordHash = PasswordGenerator.GenerateHash(updateUserDto.Password, passwordSalt);
                existingUser.Salt = passwordSalt;
                existingUser.Password = passwordHash;
            }

            existingUser.DateCreated = existingUser.DateCreated;
            await _userRepository.UpdateAsync(existingUser);
        }


        public async Task DeleteUserAsync(int id)
        {
            await _userRepository.DeleteAsync(id);
        }
        public async Task<int> GetUserCountAsync()
        {
            return await _userRepository.CountAsync();
        }
    }
}