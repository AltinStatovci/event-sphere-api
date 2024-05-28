using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSphere.Domain.DTOs
{
    public class TicketDTO
    {
        public int ID {  get; set; }
        public int EventID { get; set; }
        public string TicketType { get; set; }
        public double Price { get; set; }
        public string BookingReference { get; set; }
        public DateTime DatePurchased { get; set; }
    }
}
