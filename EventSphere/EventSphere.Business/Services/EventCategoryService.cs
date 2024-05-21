using EventSphere.Business.Services.Interfaces;
using EventSphere.Domain.DTOs.EventSphere.API.DTOs;
using EventSphere.Domain.Entities;
using EventSphere.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSphere.Business.Services
{
    public class EventCategoryService : IEventCategoryService
    {
        private readonly IGenericRepository<EventCategory> _eventCategoryRepository;

        public EventCategoryService(IGenericRepository<EventCategory> eventCategoryRepository)
        {
            _eventCategoryRepository = eventCategoryRepository;
        }

        private EventCategoryDto MapToDTO(EventCategory category)
        {
            return new EventCategoryDto
            {
                ID = category.ID,
                CategoryName = category.CategoryName
            };
        }

        private EventCategory MapToEntity(EventCategoryDto categoryDTO)
        {
            return new EventCategory
            {
                ID = categoryDTO.ID,
                CategoryName = categoryDTO.CategoryName
            };
        }

        public async Task<IEnumerable<EventCategoryDto>> GetAllCategoriesAsync()
        {
            var categories = await _eventCategoryRepository.GetAllAsync();
            return categories.Select(MapToDTO);
        }

        public async Task<EventCategoryDto> GetCategoryByIdAsync(int id)
        {
            var category = await _eventCategoryRepository.GetByIdAsync(id);
            return category != null ? MapToDTO(category) : null;
        }

        public async Task<EventCategoryDto> AddCategoryAsync(EventCategoryDto categoryDTO)
        {
            var category = MapToEntity(categoryDTO);
            var addedCategory = await _eventCategoryRepository.AddAsync(category);
            return MapToDTO(addedCategory);
        }

        public async Task UpdateCategoryAsync(EventCategoryDto categoryDTO)
        {
            var category = MapToEntity(categoryDTO);
            await _eventCategoryRepository.UpdateAsync(category);
        }

        public async Task DeleteCategoryAsync(int id)
        {
            await _eventCategoryRepository.DeleteAsync(id);
        }
    }
}
