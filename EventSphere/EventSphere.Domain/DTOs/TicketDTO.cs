using EventSphere.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSphere.Domain.DTOs
{
    public class TicketDTO
    {
        public int Id {  get; set; }
        public int EventId { get; set; }
        public string TicketType { get; set; }
        public double Price { get; set; }
        public string BookingReference { get; set; }
    }
}
