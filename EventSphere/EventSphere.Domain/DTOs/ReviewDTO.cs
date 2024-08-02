using EventSphere.Domain.Entities;
using System;

namespace EventSphere.Domain.DTOs
{
    public class ReviewDTO
    {
        public int Id { get; set; }
        public int ReviewerId { get; set; }
        public string? ReviewerName { get; set; }
        public int OrganizerId { get; set; }
        public string? OrganizerName { get; set; }
        public Rating Rating { get; set; }
        public string ReviewText { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
