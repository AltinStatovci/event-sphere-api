namespace EventSphere.Domain.Entities
{
    public class Event
    {
        public int ID { get; set; }
        public string EventName { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public int LocationId { get; set; }
        public Location Location { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int CategoryID { get; set; }
        public EventCategory Category { get; set; }
        public string? CategoryName { get; set; }
        public int OrganizerID { get; set; }
        public User Organizer { get; set; }
        public string? OrganizerName {  get; set; }
        public string PhotoData { get; set; }
        public int MaxAttendance { get; set; }
        public int AvailableTickets { get; set; }
        public DateTime DateCreated { get; set; }
        public bool IsApproved {  get; set; }
        public ICollection<Ticket> Tickets { get; set; }
    }
}
