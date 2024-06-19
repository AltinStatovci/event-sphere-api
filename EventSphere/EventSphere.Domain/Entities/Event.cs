namespace EventSphere.Domain.Entities
{
    public class Event
    {
        public int Id { get; set; }
        public string EventName { get; set; }
        public string Description { get; set; }
        public int LocationId { get; set; }
        public Location Location { get; set; }
        public string? LocationAdress { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int CategoryId { get; set; }
        public EventCategory Category { get; set; }
        public string? CategoryName { get; set; }
        public int OrganizerId { get; set; }
        public User Organizer { get; set; }
        public string? OrganizerName {  get; set; }
        public string PhotoData { get; set; }
        public int MaxAttendance { get; set; }
        public int AvailableTickets { get; set; }
        public DateTime DateCreated { get; set; }
        public ICollection<Ticket> Tickets { get; set; }
    }
}
