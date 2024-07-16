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
namespace EventSphere.Tests.Services
{
    public class EventCategoryServiceTest
    {
        private readonly IEventCategoryService _eventCategoryService;
        private readonly Mock<IGenericRepository<EventCategory>> _mockEventCategoryRepository;

        public EventCategoryServiceTest()
        {
            _mockEventCategoryRepository = new Mock<IGenericRepository<EventCategory>>();
            _eventCategoryService = new EventCategoryService(_mockEventCategoryRepository.Object);
        }

        [Fact]
        public async Task GetAllCategoriesAsync_ShouldReturnAllCategories()
        {
            //Arrange
            var categories = new List<EventCategory>
            {
                new EventCategory{ ID = 1, CategoryName = "Category 1"},
                new EventCategory{ ID = 2, CategoryName = "Category 2"},
            };

            _mockEventCategoryRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(categories);
            //Act
            var result = await _eventCategoryService.GetAllCategoriesAsync();
            //Assert
            Assert.NotNull(result);
            Assert.Equal(categories.Count, result.Count());
        }

        [Fact]
        public async Task GetCategoryByIdAsync_ShouldReturnCategory_WhenCategoryExists()
        {
            //Arrange
            var categoryId = 1;
            var category = new EventCategory
            {
                ID = categoryId,
                CategoryName = "TestCategory"
            };

            _mockEventCategoryRepository.Setup(r => r.GetByIdAsync(categoryId)).ReturnsAsync(category);

            //Act
            var result = await _eventCategoryService.GetCategoryByIdAsync(categoryId);
            //Assert
            Assert.NotNull(result);
            Assert.Equal(categoryId, result.ID);
            Assert.Equal("TestCategory", result.CategoryName);
        }

        [Fact]
        public async Task GetCategoryByIdAsync_ShouldReturnNull_WhenCategoryDoesNotExist()
        {
            //Arrange
            var categoryId = 1;
            _mockEventCategoryRepository.Setup(r => r.GetByIdAsync(categoryId)).ReturnsAsync((EventCategory)null);

            //Act
            var result = await _eventCategoryService.GetCategoryByIdAsync(categoryId);

            //Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task AddCategoryAsync_ShouldAddCategory()
        {
            //Arrange
            var categoryDto = new EventCategoryDto
            {
                 ID = 1,
                 CategoryName = "NewCategory"
            };

             var category = new EventCategory
            {
                 ID = 1,
                 CategoryName = "NewCategory"
             };

            _mockEventCategoryRepository.Setup(r => r.AddAsync(It.IsAny<EventCategory>())).ReturnsAsync(category);
            
            //Act
            var result = await _eventCategoryService.AddCategoryAsync(categoryDto);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(categoryDto.ID, result.ID);
            Assert.Equal(categoryDto.CategoryName, result.CategoryName);
        }

        [Fact]
        public async Task UpdateCategoryAsync_ShouldCallRepositoryUpdateAsync()
        {
            //Arrange
            var categoryDto = new EventCategoryDto
            {
                ID = 1,
                CategoryName = "UpdatedCategory"
            };

            _mockEventCategoryRepository.Setup(r => r.UpdateAsync(It.IsAny<EventCategory>())).Returns(Task.CompletedTask).Verifiable();

            //Act
            await _eventCategoryService.UpdateCategoryAsync(categoryDto);

            //Assert
            _mockEventCategoryRepository.Verify(r => r.UpdateAsync(It.Is<EventCategory>(c =>
                c.ID == categoryDto.ID &&
                c.CategoryName == categoryDto.CategoryName)), Times.Once);
        }

        [Fact]
        public async Task DeleteCategoryAsync_ShouldCallRepositoryDeleteAsync()
        {
            // Arrange
            var categoryId = 1;

            // Set up the repository to accept any integer id.
            _mockEventCategoryRepository.Setup(r => r.DeleteAsync(categoryId)).Returns(Task.CompletedTask).Verifiable();

            // Act
            await _eventCategoryService.DeleteCategoryAsync(categoryId);

            // Assert
            _mockEventCategoryRepository.Verify(r => r.DeleteAsync(categoryId), Times.Once);
        }
    }


}
