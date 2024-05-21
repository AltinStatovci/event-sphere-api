using EventSphere.Domain.Entities;

namespace EventSphere.Domain.DTOs
{
    public class EventDTO
    {
        public string EventName { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int CategoryID { get; set; }
        public int OrganizerID { get; set; }
        public string? PhotoData { get; set; }
        public int MaxAttendance { get; set; }
        public int AvailableTickets { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
