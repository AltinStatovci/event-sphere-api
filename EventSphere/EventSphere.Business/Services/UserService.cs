using EventSphere.Business.Helper;
using EventSphere.Business.Services.Interfaces;
using EventSphere.Business.Validator.password;
using EventSphere.Domain.DTOs.User;
using EventSphere.Domain.Entities;
using EventSphere.Infrastructure.Repositories.UserRepository;
using MapsterMapper;
using Microsoft.AspNetCore.Identity;

namespace EventSphere.Business.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IPasswordGenerator _passwordGenerator;
        private readonly IPasswordValidator _passwordValidator;

        public UserService(IUserRepository userRepository, IMapper mapper, IPasswordGenerator passwordGenerator, IPasswordValidator passwordValidator)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _passwordGenerator = passwordGenerator;
            _passwordValidator = passwordValidator;
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
            
            existingUser.DateCreated = existingUser.DateCreated;
            await _userRepository.UpdateAsync(existingUser);
        }

        public async Task UpdateUserPasswordAsync(int id, UpdatePasswordDto updatePasswordDto)
        {
            var existingUser = await _userRepository.GetByIdAsync(id);
            if (existingUser == null)
            {
                throw new Exception("User does not exist!");
            }
           
            
            if (!_passwordGenerator.VerifyPassword(updatePasswordDto.CurrentPassword, existingUser.Password, existingUser.Salt))
            {
                throw new UnauthorizedAccessException("Current password is incorrect");
            }

            _passwordValidator.ValidatePassword(updatePasswordDto.NewPassword);
            var newSalt = _passwordGenerator.GenerateSalt();
            var newHashedPassword = _passwordGenerator.GenerateHash(updatePasswordDto.NewPassword, newSalt);
            
            existingUser.Password = newHashedPassword;
            existingUser.Salt = newSalt;

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
        
        public async Task<IEnumerable<User>> GetUsersByRoleAsync(string role)
        {
            return await _userRepository.GetUsersByRoleAsync(role);
        }
    }
}