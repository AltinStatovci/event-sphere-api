using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSphere.Domain.Entities
{
    public class Payment
    {
        public int Id { get; set; }
        public User UserId { get; set; }
        public Event EventId { get; set; }
        public Ticket TicketId { get; set; }
        public int Amount { get; set; } 
        public string PaymentMethod { get; set; }
        public bool PaymentStatus { get; set; }
        public DateTime PaymentDate { get; set; }
    }
}
