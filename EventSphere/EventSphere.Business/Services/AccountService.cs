using EventSphere.Business.Helper;
using EventSphere.Business.Services.Interfaces;
using EventSphere.Domain.DTOs.User;
using EventSphere.Domain.Entities;
using EventSphere.Infrastructure.Repositories.UserRepository;
using MapsterMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using EventSphere.Business.Validator.password;
using EventSphere.Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;


namespace EventSphere.Business.Services
{
    public class AccountService : IAccountService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;
        private readonly IGenericRepository<Role> _roleRepository;
        private readonly IPasswordGenerator _passwordGenexrator;
        private readonly IPasswordValidator _passwordValidator;
     

        public AccountService(IUserRepository userRepository, IConfiguration config, IMapper mapper, IGenericRepository<Role> roleRepository, IPasswordGenerator passwordGenexrator, IPasswordValidator passwordValidator)
        {
            _userRepository = userRepository;
            _config = config;
            _mapper = mapper;
            _roleRepository = roleRepository;
            _passwordGenexrator = passwordGenexrator;
            _passwordValidator = passwordValidator;
        }

        public async Task<UserDTO> AddUserAsync(CreateUserDTO createUserDto)
        {
            var existingUser = await _userRepository.GetUserByEmailAsync(createUserDto.Email.Trim().ToLower());
            if (existingUser != null)
            {
                throw new ArgumentException($"Email is already in use. {createUserDto.Email}");
               
            }
            
            _passwordValidator.ValidatePassword(createUserDto.Password);
            var user = _mapper.Map<User>(createUserDto);
            var passwordSalt = _passwordGenexrator.GenerateSalt();
            var passwordHash = _passwordGenexrator.GenerateHash(createUserDto.Password, passwordSalt);
            user.Salt = passwordSalt;
            user.Password = passwordHash;

            var role = await _roleRepository.GetByIdAsync(createUserDto.RoleID);
            var roleName = role.RoleName;

            user.RoleName = roleName;

            var createdUser = await _userRepository.AddAsync(user);
            return _mapper.Map<UserDTO>(createdUser);
        }

        public async Task<string> AuthenticateAsync(LoginDTO loginDto)
        {
            try
            {
                var user = await _userRepository.GetUserByEmailAsync(loginDto.Email);
                if (user == null || !_passwordGenexrator.VerifyPassword(loginDto.Password, user.Password, user.Salt))
                {
                    return null;
                }

                return GenerateJwtToken(user);
            }
            catch (Exception ex)
            {
                // Log the exception for debugging purposes
                Console.WriteLine($"An error occurred during authentication: {ex.Message}");
                throw; // Rethrow the exception to propagate it to the caller
            }
        }

        private string GenerateJwtToken(User user)
        {
            SecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            SigningCredentials credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            var expiration = DateTime.UtcNow.AddMinutes(120);

            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("ID", user.ID.ToString()),
                new Claim("Role", user.RoleID.ToString()),
                new Claim("Username", user.Name),
                new Claim(JwtRegisteredClaimNames.Exp, new DateTimeOffset(expiration).ToUnixTimeSeconds().ToString())
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
