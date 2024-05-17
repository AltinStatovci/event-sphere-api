using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSphere.Domain.Entities
{
    public class Event
    {
        public int Id {  get; set; }
        public string EventName { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public EventCategory Category { get; set; }
        public User OrganizerId { get; set; }
        public string? PhotoData { get; set; }
        public int MaxAttandance { get; set; }
        public int AvailableTickets { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
