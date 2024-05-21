using EventSphere.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSphere.Business.Services.Interfaces
{
    public interface IEventCategoryService
    {
        Task<IEnumerable<EventCategory>> GetAllEventCategoriesAsync();
        Task<EventCategory> GetEventCategoryByIdAsync(int id);
        Task<EventCategory> CreateEventCategoryAsync(EventCategory eventCategory);
        Task UpdateEventCategoryAsync(EventCategory eventCategory);
        Task DeleteEventCategoryAsync(int id);
    }
}
