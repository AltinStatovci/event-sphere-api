using EventSphere.Business.Helper;
using EventSphere.Business.Services.Interfaces;
using EventSphere.Domain.DTOs.User;
﻿using EventSphere.Business.Services.Interfaces;
using EventSphere.Domain.Entities;
using EventSphere.Infrastructure.Repositories.UserRepository;
using MapsterMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EventSphere.Business.Services
{
    public class AccountService : IAccountService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;

        public AccountService(IUserRepository userRepository, IConfiguration config, IMapper mapper)
        {
            _userRepository = userRepository;
            _config = config;
            _mapper = mapper;
        }

        public async Task<UserDTO> AddUserAsync(CreateUserDTO createUserDto)
        {    
            var user = _mapper.Map<User>(createUserDto);
            var passwordSalt = PasswordGenerator.GenerateSalt();
            var passwordHash = PasswordGenerator.GenerateHash(createUserDto.Password, passwordSalt);
            user.Salt = passwordSalt;
            user.Password = passwordHash;
            var createdUser = await _userRepository.AddAsync(user);
            return _mapper.Map<UserDTO>(createdUser);
        }

        public async Task<string> AuthenticateAsync(LoginDTO loginDto)
        {
            var user = await _userRepository.GetUserByEmailAsync(loginDto.Email);
            if (user == null || !PasswordGenerator.VerifyPassword(loginDto.Password, user.Password, user.Salt))
            {
                return null;
            }

            return GenerateJwtToken(user);
        }

        private string GenerateJwtToken(User user)
        {
            SecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            SigningCredentials credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("ID", user.ID.ToString()),
                new Claim("Role", user.RoleID.ToString())
            };

            var token = new JwtSecurityToken(
                _config["Jwt:Issuer"],
                _config["Jwt:Issuer"],
                claims,
                expires: DateTime.Now.AddMinutes(120),
                signingCredentials: credentials
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
