using EventSphere.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSphere.Domain.DTOs
{
    public class TicketDTO
    {
        [Required(ErrorMessage = "ID is required.")]
        public int ID { get; set; }

        [Required(ErrorMessage = "EventID is required.")]
        public int EventID { get; set; }

        [Required(ErrorMessage = "Ticket type is required.")]
        public string TicketType { get; set; }

        [Required(ErrorMessage = "Price is required.")]
        [Range(0, double.MaxValue, ErrorMessage = "Price cannot be negative.")]
        public double Price { get; set; }
        [Required(ErrorMessage = "Ticket Amount is required.")]
        public int TicketAmount { get; set; }

        [Required(ErrorMessage = "Booking reference is required.")]
        public string BookingReference { get; set; }
    }
}
