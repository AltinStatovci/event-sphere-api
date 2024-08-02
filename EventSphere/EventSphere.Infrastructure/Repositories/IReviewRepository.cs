using EventSphere.Domain.Entities;
using EventSphere.Infrastructure.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EventSphere.Domain.Repositories
{
    public interface IReviewRepository : IGenericRepository<Review>
    {
        Task<IEnumerable<Review>> GetReviewsByOrganizerIdAsync(int organizerId);
        Task<IEnumerable<Review>> GetReviewsByReviewerIdAsync(int reviewerId);
        Task<IEnumerable<Review>> GetAllReviewsWithUserDetailsAsync();
    }
}
