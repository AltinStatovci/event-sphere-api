using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSphere.Domain.Entities
{
    public class Payments
    {
        public int PaymentId { get; set; }
        public Users UserId { get; set; }
        public Events EventId { get; set; }
        public Tickets TicketId { get; set; }
        public int Amount { get; set; } 
        public string PaymentMethod { get; set; }
        public bool PaymentStatus { get; set; }
        public DateTime PaymentDate { get; set; }
    }
}
