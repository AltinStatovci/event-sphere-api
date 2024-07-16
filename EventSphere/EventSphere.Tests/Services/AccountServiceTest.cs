using EventSphere.Business.Helper;
using EventSphere.Business.Services;
using EventSphere.Business.Services.Interfaces;
using EventSphere.Domain.DTOs.User;
using EventSphere.Domain.Entities;
using EventSphere.Infrastructure.Repositories;
using EventSphere.Infrastructure.Repositories.UserRepository;
using MapsterMapper;
using Microsoft.Extensions.Configuration;
using Moq;
using Xunit;

namespace EventSphere.Tests.Services;

public class AccountServiceTest
{
    private readonly IAccountService _accountService;
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IConfiguration> _configMock;
    private readonly Mock<IGenericRepository<Role>> _roleRepositoryMock;
    private readonly Mock<IPasswordGenerator> _passwordGeneratorMock;

    public AccountServiceTest()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _mapperMock = new Mock<IMapper>();
        _configMock = new Mock<IConfiguration>();
        _roleRepositoryMock = new Mock<IGenericRepository<Role>>();
        _passwordGeneratorMock = new Mock<IPasswordGenerator>();

        _accountService = new AccountService(
            _userRepositoryMock.Object,
            _configMock.Object,
            _mapperMock.Object,
            _roleRepositoryMock.Object,
            _passwordGeneratorMock.Object
        );
    }

    [Fact]
    public async Task AddUserAsync_OnSuccess_ShouldAddUser()
    {
        // Arrange
        var createUserDto = new CreateUserDTO { Email = "test@example.com", Password = "password123", RoleID = 1 };
        var user = new User();
        var role = new Role { RoleName = "User" };

        var salt = new byte[128 / 8];
        var hash = new byte[256 / 8];

        _userRepositoryMock.Setup(repo => repo.GetUserByEmailAsync(It.IsAny<string>()))!.ReturnsAsync((User)null);
        _mapperMock.Setup(mapper => mapper.Map<User>(It.IsAny<CreateUserDTO>())).Returns(user);
        _passwordGeneratorMock.Setup(pg => pg.GenerateSalt()).Returns(salt);
        _passwordGeneratorMock.Setup(pg => pg.GenerateHash(It.IsAny<string>(), It.IsAny<byte[]>())).Returns(hash);
        _roleRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(role);
        _userRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<User>())).ReturnsAsync(user);
        _mapperMock.Setup(mapper => mapper.Map<UserDTO>(It.IsAny<User>())).Returns(new UserDTO());

        // Act
        var result = await _accountService.AddUserAsync(createUserDto);

        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public async Task AddUserAsync_OnEmailIsInUser_ShouldThrowArgumentException()
    {
        // Arrange
        var createUserDto = new CreateUserDTO { Email = "test@example.com", Password = "password123", RoleID = 1 };
        var existingUser = new User();
        var exceptionMsg = "Email is already in use. " + createUserDto.Email; 

        _userRepositoryMock.Setup(repo => repo.GetUserByEmailAsync(It.IsAny<string>())).ReturnsAsync(existingUser);

        // Act 
        var exception = await Assert.ThrowsAsync<ArgumentException>(() => _accountService.AddUserAsync(createUserDto));
        
        // Assert
        Assert.Equal(exceptionMsg, exception.Message);
       
    }

    [Fact]
    public async Task AuthenticateAsync_onSuccess_ShouldReturnToken()
    {
        // Arrange
        var loginDto = new LoginDTO { Email = "test@example.com", Password = "password123" };
        var salt = new byte[128 / 8];
        var hash = new byte[256 / 8];
        var user = new User
        {
            Email = loginDto.Email,
            Password = hash,
            Salt = salt,
            ID = 1,
            RoleID = 1, 
            Name = "TestUser" 
        };

        _userRepositoryMock.Setup(repo => repo.GetUserByEmailAsync(It.IsAny<string>())).ReturnsAsync(user);
        _passwordGeneratorMock.Setup(pg => pg.VerifyPassword(It.IsAny<string>(), It.IsAny<byte[]>(), It.IsAny<byte[]>())).Returns(true);
        _configMock.Setup(config => config["Jwt:Key"]).Returns("TestKey123456789012345678901234567890"); 
        _configMock.Setup(config => config["Jwt:Issuer"]).Returns("TestIssuer");

        // Act
        var result = await _accountService.AuthenticateAsync(loginDto);

        // Assert
        Assert.NotNull(result);
     
        
    }

    [Fact]
    public async Task AuthenticateAsync_OnCredentialsAreInvalid_ShouldReturnNull()
    {
        // Arrange
        var loginDto = new LoginDTO { Email = "test@example.com", Password = "password123" };

        _userRepositoryMock.Setup(repo => repo.GetUserByEmailAsync(It.IsAny<string>()))!.ReturnsAsync((User)null);

        // Act
        var result = await _accountService.AuthenticateAsync(loginDto);

        // Assert
        Assert.Null(result);
       
    }
}