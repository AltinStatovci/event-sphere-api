using EventSphere.Domain.DTOs;
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
        Task<IEnumerable<EventCategory>> GetAllCategoriesAsync();
        Task<EventCategory> GetCategoryByIdAsync(int id);
        Task<EventCategory> CreateCategoryAsync(EventCategoryDto eventCategoryDto);
        Task UpdateCategoryAsync(int id, EventCategoryDto eventCategoryDto);
        Task DeleteCategoryAsync(int id);
    }
}
