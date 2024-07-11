using EventSphere.Business.Services;
using EventSphere.Business.Services.Interfaces;
using EventSphere.Domain.DTOs.User;
using EventSphere.Domain.Entities;
using EventSphere.Infrastructure.Repositories.UserRepository;
using MapsterMapper;
using Moq;
using Xunit;

namespace EventSphere.Tests.Services;

public class UserServiceTest
{
    private readonly IUserService _userService;
    private readonly Mock<IUserRepository> _mockUserRepository;
    private readonly Mock<IMapper> _mockMapper;

    public UserServiceTest()
    {

        _mockUserRepository = new Mock<IUserRepository>();
        _mockMapper = new Mock<IMapper>();
        _userService = new UserService(_mockUserRepository.Object, _mockMapper.Object);
    }

    [Fact]
    public async Task GetAllUsersAsync_OnSuccess_ShouldReturnAllUsers()
    {
        // Arrange
        var expectedUsers = new List<User>
        {
            new User { ID = 1, Name = "User 1", LastName = "User 1", Email = "user1@gmail.com", RoleID = 1 },
            new User { ID = 2, Name = "User 2", LastName = "User 2", Email = "user2@gmail.com", RoleID = 2 }
        };

        _mockUserRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(expectedUsers);

        // Act
        var result = await _userService.GetAllUsersAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedUsers, result);
    }


    [Fact]
    public async Task GetUserByIdAsync_OnSuccess_ShouldReturnUser()
    {
        // Arrange
        var expectedUser = new User
        {
            ID = 1,
            Name = "User 1",
            LastName = "User 1",
            Email = "user1@gmail.com",
            RoleID = 1
        };

        var id = 1;
        _mockUserRepository.Setup(repo => repo.GetByIdAsync(id)).ReturnsAsync(expectedUser);

        // Act
        var result = await _userService.GetUserByIdAsync(id);

        // Assert 
        Assert.NotNull(result);
        Assert.Equal(expectedUser, result);
    }


    [Fact]
    public async Task UpdateUserAsync_OnSuccess_ShouldReturnUpdatedUser()
    {
        // Arrange
        var existingUser = new User
        {
            ID = 1,
            Name = "User 1",
            LastName = "User 1",
            Email = "user1@gmail.com",
            RoleID = 1
        };

        var updateUserDto = new UpdateUserDTO
        {
            ID = 1,
            Name = " Updated Name",
            LastName = "Updated LastName",
            Email = "user1@gmail.com",
            RoleID = 1
        };

        _mockUserRepository.Setup(repo => repo.GetByIdAsync(updateUserDto.ID)).ReturnsAsync(existingUser);
        _mockMapper.Setup(mapper => mapper.Map(updateUserDto, existingUser)).Callback<UpdateUserDTO, User>(
            (dto, user) =>
            {
                user.Name = dto.Name;
                user.LastName = dto.LastName;
                user.Email = dto.Email;
                user.RoleID = dto.RoleID;
            });

        // Act
        await _userService.UpdateUserAsync(updateUserDto);

        // Assert
        Assert.NotNull(existingUser);
        Assert.Equal(updateUserDto.Name, existingUser.Name);
        Assert.Equal(updateUserDto.LastName, existingUser.LastName);
        Assert.Equal(updateUserDto.Email, existingUser.Email);
        Assert.Equal(updateUserDto.RoleID, existingUser.RoleID);

    }

    [Fact]
    public async Task UpdateUserAsync_OnNullExistingUser_ShouldThrowException()
    {
        // Arrange
        var exceptionMsg = "User does not exist!";

        var updateUserDto = new UpdateUserDTO
        {
            ID = 1,
            Name = " Updated Name",
            LastName = "Updated LastName",
            Email = "user1@gmail.com",
            RoleID = 1
        };

        _mockUserRepository.Setup(repo => repo.GetByIdAsync(updateUserDto.ID))!.ReturnsAsync((User)null);

        // Act
        var exception = await Assert.ThrowsAsync<Exception>(() => _userService.UpdateUserAsync(updateUserDto));

        // Assert
        Assert.Equal(exceptionMsg, exception.Message);
    }

    [Fact]
    public async Task UpdateUserPasswordAsync_OnNullExistingUser_ShouldThrowException()
    {
        // Arrange
        var exceptionMsg = "User does not exist!";
        var userId = 1;
        var updatePasswordDto = new UpdatePasswordDto 
        {
            CurrentPassword = "currentPassword",
            NewPassword = "newPassword"
        };
        
        _mockUserRepository.Setup(repo => repo.GetByIdAsync(userId))!.ReturnsAsync((User)null);
        
        //Act
        var exception = await Assert.ThrowsAsync<Exception>(() => _userService.UpdateUserPasswordAsync(userId, updatePasswordDto));
      
        //Assert
        Assert.Equal(exceptionMsg, exception.Message);
       
        
        }
    
    
    [Fact]
    public async Task UpdateUserPasswordAsync_OnSuccess_ShouldReturnUpdatedUser()
    {
        // Arrange
        var userId = 1;

        var updatePasswordDto = new UpdatePasswordDto
        {
            CurrentPassword = "currentPassword",
            NewPassword = "newPassword"
        };
        _mockUserRepository.Setup(repo => repo.GetByIdAsync(userId)).ReturnsAsync(It.IsAny<User>());

        // Act
        await _userService.UpdateUserPasswordAsync(userId, updatePasswordDto);
    }
        

}