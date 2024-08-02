using EventSphere.Business.Services.Interfaces;
using EventSphere.Domain.DTOs;
using EventSphere.Domain.Entities;
using EventSphere.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using EventSphere.Infrastructure.Repositories;

namespace EventSphere.Business.Services
{
    public class ReviewService : IReviewService
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IGenericRepository<User> _userRepository;

        public ReviewService(IReviewRepository reviewRepository, IGenericRepository<User> userRepository)
        {
            _reviewRepository = reviewRepository;
            _userRepository = userRepository;
        }

        public async Task<ReviewDTO> CreateAsync(ReviewDTO reviewDTO)
        {
            if (reviewDTO == null || !Enum.IsDefined(typeof(Rating), reviewDTO.Rating))
            {
                throw new ArgumentException("Invalid rating. Rating should be between 1 and 5.");
            }

            var reviewer = await _userRepository.GetByIdAsync(reviewDTO.ReviewerId);
            var organizer = await _userRepository.GetByIdAsync(reviewDTO.OrganizerId);

            if (reviewer == null || organizer == null)
            {
                throw new ArgumentException("Invalid reviewer or organizer.");
            }

            var review = new Review
            {
                ReviewerId = reviewDTO.ReviewerId,
                OrganizerId = reviewDTO.OrganizerId,
                Rating = reviewDTO.Rating,
                ReviewText = reviewDTO.ReviewText,
                CreatedAt = DateTime.UtcNow
            };

            await _reviewRepository.AddAsync(review);
            reviewDTO.Id = review.Id; // Assuming that Id is assigned by the database
            reviewDTO.CreatedAt = review.CreatedAt;
            return reviewDTO;
        }

        public async Task DeleteAsync(int id)
        {
            await _reviewRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<ReviewDTO>> GetAllReviewsAsync()
        {
            var reviews = await _reviewRepository.GetAllReviewsWithUserDetailsAsync();
            return reviews.Select(review => new ReviewDTO
            {
                Id = review.Id,
                ReviewerId = review.ReviewerId,
                ReviewerName = review.Reviewer.Name,
                OrganizerId = review.OrganizerId,
                OrganizerName = review.Organizer.Name,
                Rating = review.Rating,
                ReviewText = review.ReviewText,
                CreatedAt = review.CreatedAt
            });
        }

        public async Task<ReviewDTO> GetReviewByIdAsync(int id)
        {
            var review = await _reviewRepository.GetAllReviewsWithUserDetailsAsync();
            var reviewDetails = review.FirstOrDefault(r => r.Id == id);
            if (reviewDetails == null)
            {
                throw new InvalidOperationException("Review not found.");
            }

            return new ReviewDTO
            {
                Id = reviewDetails.Id,
                ReviewerId = reviewDetails.ReviewerId,
                ReviewerName = reviewDetails.Reviewer.Name,
                OrganizerId = reviewDetails.OrganizerId,
                OrganizerName = reviewDetails.Organizer.Name,
                Rating = reviewDetails.Rating,
                ReviewText = reviewDetails.ReviewText,
                CreatedAt = reviewDetails.CreatedAt
            };
        }

        public async Task<IEnumerable<ReviewDTO>> GetReviewsByOrganizerIdAsync(int organizerId)
        {
            var reviews = await _reviewRepository.GetReviewsByOrganizerIdAsync(organizerId);
            return reviews.Select(review => new ReviewDTO
            {
                Id = review.Id,
                ReviewerId = review.ReviewerId,
                ReviewerName = review.Reviewer.Name,
                OrganizerId = review.OrganizerId,
                OrganizerName = review.Organizer.Name,
                Rating = review.Rating,
                ReviewText = review.ReviewText,
                CreatedAt = review.CreatedAt
            });
        }

        public async Task<IEnumerable<ReviewDTO>> GetReviewsByReviewerIdAsync(int reviewerId)
        {
            var reviews = await _reviewRepository.GetReviewsByReviewerIdAsync(reviewerId);
            return reviews.Select(review => new ReviewDTO
            {
                Id = review.Id,
                ReviewerId = review.ReviewerId,
                ReviewerName = review.Reviewer.Name,
                OrganizerId = review.OrganizerId,
                OrganizerName = review.Organizer.Name,
                Rating = review.Rating,
                ReviewText = review.ReviewText,
                CreatedAt = review.CreatedAt
            });
        }

        public async Task UpdateAsync(int id, ReviewDTO reviewDTO)
        {
            var review = await _reviewRepository.GetByIdAsync(id);
            if (review == null)
            {
                throw new InvalidOperationException("Review not found.");
            }

            review.Rating = reviewDTO.Rating;
            review.ReviewText = reviewDTO.ReviewText;

            await _reviewRepository.UpdateAsync(review);
        }
    }
}
