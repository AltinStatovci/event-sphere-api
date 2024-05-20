using EventSphere.Business.Services.Interfaces;
using EventSphere.Domain.DTOs;
using EventSphere.Domain.Entities;
using EventSphere.Domain.Repositories;
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
        private readonly IEventCategoryRepository _eventCategoryRepository;

        public EventCategoryService(IEventCategoryRepository eventCategoryRepository)
        {
            _eventCategoryRepository = eventCategoryRepository;
        }

        public async Task<IEnumerable<EventCategory>> GetAllCategoriesAsync()
        {
            return await _eventCategoryRepository.GetAllAsync();
        }

        public async Task<EventCategory> GetCategoryByIdAsync(int id)
        {
            return await _eventCategoryRepository.GetByIdAsync(id);
        }

        public async Task<EventCategory> CreateCategoryAsync(EventCategoryDto eventCategoryDto)
        {
            var eventCategory = new EventCategory
            {
                CategoryName = eventCategoryDto.CategoryName
            };
            await _eventCategoryRepository.AddAsync(eventCategory);
            return eventCategory;
        }

        public async Task UpdateCategoryAsync(int id, EventCategoryDto eventCategoryDto)
        {
            var eventCategory = await _eventCategoryRepository.GetByIdAsync(id);
            if (eventCategory == null) return;

            eventCategory.CategoryName = eventCategoryDto.CategoryName;
            await _eventCategoryRepository.UpdateAsync(eventCategory);
        }

        public async Task DeleteCategoryAsync(int id)
        {
            await _eventCategoryRepository.DeleteAsync(id);
        }
    }
}
