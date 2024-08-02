using System;

namespace EventSphere.Domain.Entities
{
    public class Review
    {
        public int Id { get; set; }
        public int ReviewerId { get; set; }
        public int OrganizerId { get; set; }
        public Rating Rating { get; set; }
        public string ReviewText { get; set; }
        public DateTime CreatedAt { get; set; }

        public User Reviewer { get; set; }
        public User Organizer { get; set; }
    }
}
