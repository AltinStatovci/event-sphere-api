namespace EventSphere.Domain.Entities
{
    public class Ticket
    {
        public int ID { get; set; }
        public int EventID { get; set; }
        public Event Event { get; set; }
        public string? EventName { get; set; }
        public string TicketType { get; set; }
        public double Price { get; set; }
        public int TicketAmount { get; set; }
        public string BookingReference { get; set; }
        public ICollection<Payment> Payments { get; set; }
    }
}
