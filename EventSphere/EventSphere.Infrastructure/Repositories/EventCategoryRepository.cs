using EventSphere.Domain.DTOs;
using EventSphere.Domain.Entities;
using EventSphere.Infrastructure;
using EventSphere.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSphere.Domain.Repositories
{
    public class EventCategoryRepository : IEventCategoryRepository
    {
        private readonly EventSphereDbContext _context;

        public EventCategoryRepository(EventSphereDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<EventCategory>> GetAllAsync()
        {
            return await _context.EventCategories.ToListAsync();
        }

        public async Task<EventCategory> GetByIdAsync(int id)
        {
            return await _context.EventCategories.FindAsync(id);
        }

        public async Task AddAsync(EventCategory eventCategory)
        {
            await _context.EventCategories.AddAsync(eventCategory);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(EventCategory eventCategory)
        {
            _context.EventCategories.Update(eventCategory);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var eventCategory = await _context.EventCategories.FindAsync(id);
            if (eventCategory != null)
            {
                _context.EventCategories.Remove(eventCategory);
                await _context.SaveChangesAsync();
            }
        }
    }
}
