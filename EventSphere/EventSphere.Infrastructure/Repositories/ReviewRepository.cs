using EventSphere.Domain.Entities;
using EventSphere.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventSphere.Infrastructure.Repositories
{
    public class ReviewRepository : GenericRepository<Review>, IReviewRepository
    {
        private readonly EventSphereDbContext _context;

        public ReviewRepository(EventSphereDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Review>> GetReviewsByOrganizerIdAsync(int organizerId)
        {
            return await _context.Reviews
                                 .Where(r => r.OrganizerId == organizerId)
                                 .Include(r => r.Reviewer) // Include reviewer details
                                 .Include(r => r.Organizer) // Include organizer details
                                 .ToListAsync();
        }

        public async Task<IEnumerable<Review>> GetReviewsByReviewerIdAsync(int reviewerId)
        {
            return await _context.Reviews
                                 .Where(r => r.ReviewerId == reviewerId)
                                 .Include(r => r.Reviewer) // Include reviewer details
                                 .Include(r => r.Organizer) // Include organizer details
                                 .ToListAsync();
        }

        public async Task<IEnumerable<Review>> GetAllReviewsWithUserDetailsAsync()
        {
            return await _context.Reviews
                                 .Include(r => r.Reviewer)
                                 .Include(r => r.Organizer)
                                 .ToListAsync();
        }
    }
}
