using EventSphere.Domain.DTOs;
using EventSphere.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSphere.Infrastructure.Repositories
{
    public interface IEventCategoryRepository
    {
        Task<IEnumerable<EventCategory>> GetAllAsync();
        Task<EventCategory> GetByIdAsync(int id);
        Task AddAsync(EventCategory eventCategory);
        Task UpdateAsync(EventCategory eventCategory);
        Task DeleteAsync(int id);
    }
}
