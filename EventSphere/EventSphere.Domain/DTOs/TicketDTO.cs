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
        [Required(ErrorMessage = "Id is required.")]
        public int Id {  get; set; }
        [Required(ErrorMessage = "EventId is required.")]
        public int EventId { get; set; }
        [Required(ErrorMessage = "Ticket type is required.")]
        public string TicketType { get; set; }
        [Required(ErrorMessage = "Price is required.")]
        [Range(0, double.MaxValue, ErrorMessage = "Price cannot be negative.")]
        public double Price { get; set; }
        [Required(ErrorMessage = "Booking reference is required.")]
        public string BookingReference { get; set; }
    }
}
