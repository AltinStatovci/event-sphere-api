using EventSphere.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSphere.Domain.DTOs
{
    public class PaymentDTO
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "User ID is required.")]
        public int UserID { get; set; }

        [Required(ErrorMessage = "Ticket ID is required.")]
        public int TicketID { get; set; }

        [Required(ErrorMessage = "Amount is required.")]
        [Range(0, int.MaxValue, ErrorMessage = "Amount of tickets cannot be negative.")]
        public int Amount { get; set; }

        [Required(ErrorMessage = "Payment method is required.")]
        public string PaymentMethod { get; set; }

        [Required(ErrorMessage = "Payment status is required.")]
        public bool PaymentStatus { get; set; }

        [Required(ErrorMessage = "Payment date is required.")]
        public DateTime PaymentDate { get; set; }
        public string StripeToken { get; set; }
        public string PromoCode { get; set; }
    }

}
