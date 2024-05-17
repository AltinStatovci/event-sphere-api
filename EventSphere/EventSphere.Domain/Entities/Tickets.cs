using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSphere.Domain.Entities
{
    public class Tickets
    {
        public int TicketId {  get; set; }
        public Events EventId { get; set; }
        public Users UserId { get; set; }
        public string TicketType { get; set; }
        public double Price { get; set; }
        public DateTime DatePurchased { get; set; }
        public string BookingReference { get; set; }
    }
}
