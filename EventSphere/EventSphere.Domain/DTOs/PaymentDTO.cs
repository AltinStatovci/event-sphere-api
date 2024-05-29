using EventSphere.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSphere.Domain.DTOs
{
    public class PaymentDTO
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public int TicketID { get; set; }
        public int Amount { get; set; }
        public string PaymentMethod { get; set; }
        public bool PaymentStatus { get; set; }
        public DateTime PaymentDate { get; set; }
    }
}
