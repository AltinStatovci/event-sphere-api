using EventSphere.Business.Services.Interfaces;
using EventSphere.Domain.Entities;
using EventSphere.Infrastructure.Repositories;
using EventSphere.Infrastructure.Repositories.Interfaces;
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


        public async Task<IEnumerable<EventCategory>> GetAllEventCategoriesAsync()
        {
            return await _eventCategoryRepository.GetAllAsync();
        }

        public async Task<EventCategory> GetEventCategoryByIdAsync(int id)
        {
            return await _eventCategoryRepository.GetByIdAsync(id);
        }

        public async Task<EventCategory> CreateEventCategoryAsync(EventCategory eventCategory)
        {
            return await _eventCategoryRepository.AddAsync(eventCategory);
        }

        public async Task UpdateEventCategoryAsync(EventCategory eventCategory)
        {
            await _eventCategoryRepository.UpdateAsync(eventCategory);
        }

        public async Task DeleteEventCategoryAsync(int id)
        {
            await _eventCategoryRepository.DeleteAsync(id);
        }
    }
}
