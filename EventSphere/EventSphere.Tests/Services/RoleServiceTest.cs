using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventSphere.Infrastructure.Repositories;
using EventSphere.Business.Services.Interfaces;
using Moq;
using EventSphere.Domain.Entities;
using Xunit;
using EventSphere.Business.Services;
using EventSphere.Domain.DTOs.EventSphere.API.DTOs;
using EventSphere.Tests.Services;
using System.Collections.Immutable;
namespace EventSphere.Tests.Services
{

    public class RoleServiceTest
    {
        private readonly IRoleService _roleService;
        private readonly Mock<IGenericRepository<Role>> _mockRoleRepository;
        public RoleServiceTest()
        {
            _mockRoleRepository = new Mock<IGenericRepository<Role>>();
            _roleService = new RoleService(_mockRoleRepository.Object);

        }

        [Fact]
        public async Task AddRole_ShouldAddRole()
        {
            //Arrange
            var roleToAdd = new Role
            {
                ID = 1,
                RoleName = "NewRole"
            };

            _mockRoleRepository.Setup(r => r.AddAsync(It.IsAny<Role>())).ReturnsAsync(roleToAdd);

            //Act
            var result = await _roleService.AddRoleAsync(roleToAdd);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(roleToAdd.ID, result.ID);
            Assert.Equal(roleToAdd.RoleName, result.RoleName);
        }

        [Fact]
        public async Task DeleteRoleAsync_ShouldDeleteRole()
        {
            //Arrange
            var roleId = 1;

            _mockRoleRepository.Setup(r => r.DeleteAsync(roleId)).Returns(Task.CompletedTask).Verifiable();
            //Act
            await _roleService.DeleteRoleAsync(roleId);
            //Assert
            _mockRoleRepository.Verify(r => r.DeleteAsync(roleId), Times.Once());

        }

        [Fact]
        public async Task GetAllRolesAsync_ShouldReturnAllRoles()
        {
            //Arrange
            var roles = new List<Role>
            {
                new Role {ID = 1, RoleName = "Role 1"},
                new Role {ID = 2, RoleName = "Role 2"}
            };
            _mockRoleRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(roles);
            //Act
            var result = await _roleService.GetAllRolesAsync();
            //Assert
            Assert.NotNull(result);
            Assert.Equal(roles.Count, result.Count());
            Assert.Equal(roles[0].ID, result.ElementAt(0).ID);
            Assert.Equal(roles[0].RoleName, result.ElementAt(0).RoleName);
            Assert.Equal(roles[1].ID, result.ElementAt(1).ID);
            Assert.Equal(roles[1].RoleName, result.ElementAt(1).RoleName);
        }

        [Fact]
        public async Task GetRoleByIdAsync_ShouldReturnRole_WhenRoleExists()
        {

            //Arange
            var roleId = 1;
            var role = new Role
            {
                ID = roleId,
                RoleName = "TestRole"
            };
            _mockRoleRepository.Setup(r => r.GetByIdAsync(roleId)).ReturnsAsync(role);
            //Act
            var result = await _roleService.GetRoleByIdAsync(roleId);
            //Assert
            Assert.NotNull(result);
            Assert.Equal(roleId, result.ID);
            Assert.Equal("TestRole", result.RoleName);
        }

        [Fact]
        public async Task GetRoleByIdAsync_ShouldReturnNull_WhenRoleDoesNotExist()
        {
            //Arrange
            var roleId = 1;
            _mockRoleRepository.Setup(r => r.GetByIdAsync(roleId)).ReturnsAsync((Role)null);

            //Act
            var result = await _roleService.GetRoleByIdAsync(roleId);

            //Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task UpdateRoleAsync_ShouldUpdateRole()
        {
            // Arrange
            var roleToUpdate = new Role
            {
                ID = 1,
                RoleName = "UpdatedRole"
            };

            _mockRoleRepository.Setup(r => r.UpdateAsync(It.IsAny<Role>())).Returns(Task.CompletedTask).Verifiable();

            // Act
            await _roleService.UpdateRoleAsync(roleToUpdate);

            // Assert
            _mockRoleRepository.Verify(r => r.UpdateAsync(It.Is<Role>(r =>
                r.ID == roleToUpdate.ID &&
                r.RoleName == roleToUpdate.RoleName)), Times.Once);
        }
    }
}