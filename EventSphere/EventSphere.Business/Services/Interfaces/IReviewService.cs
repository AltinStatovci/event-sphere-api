using EventSphere.Domain.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EventSphere.Business.Services.Interfaces
{
    public interface IReviewService
    {
        Task<IEnumerable<ReviewDTO>> GetAllReviewsAsync();
        Task<ReviewDTO> GetReviewByIdAsync(int id);
        Task<IEnumerable<ReviewDTO>> GetReviewsByOrganizerIdAsync(int organizerId);
        Task<IEnumerable<ReviewDTO>> GetReviewsByReviewerIdAsync(int reviewerId);
        Task<ReviewDTO> CreateAsync(ReviewDTO reviewDTO);
        Task UpdateAsync(int id, ReviewDTO reviewDTO);
        Task DeleteAsync(int id);
    }
}
