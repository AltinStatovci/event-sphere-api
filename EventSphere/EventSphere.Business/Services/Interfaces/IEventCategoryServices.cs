using EventSphere.Domain.DTOs.EventSphere.API.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSphere.Business.Services.Interfaces
{
    public interface IEventCategoryServices
    {
        Task<IEnumerable<EventCategoryDto>> GetAllCategoriesAsync();
        Task<EventCategoryDto> GetCategoryByIdAsync(int id);
        Task<EventCategoryDto> AddCategoryAsync(EventCategoryDto categoryDTO);
        Task UpdateCategoryAsync(EventCategoryDto categoryDTO);
        Task DeleteCategoryAsync(int id);
    }
}
