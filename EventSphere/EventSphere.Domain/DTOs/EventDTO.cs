using EventSphere.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace EventSphere.Domain.DTOs
{
    public class EventDTO 
    {
        public string EventName { get; set; }

        public string Description { get; set; }
      
        public string Address { get; set; }
        public int LocationId { get; set; }
        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public int CategoryID { get; set; }

        public int OrganizerID { get; set; }

        public int MaxAttendance { get; set; }

        public int AvailableTickets { get; set; }

        public DateTime DateCreated { get; set; }
        
    }
}
