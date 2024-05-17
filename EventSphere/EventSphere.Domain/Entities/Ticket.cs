using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSphere.Domain.Entities
{
    public class Ticket
    {
        public int Id {  get; set; }
        public Event EventId { get; set; }
        public User UserId { get; set; }
        public string TicketType { get; set; }
        public double Price { get; set; }
        public DateTime DatePurchased { get; set; }
        public string BookingReference { get; set; }
    }
}
