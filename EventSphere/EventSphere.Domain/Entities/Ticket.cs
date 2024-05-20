namespace EventSphere.Domain.Entities
{
    public class Ticket
    {
        public int ID { get; set; }
        public int EventID { get; set; }
        public Event Event { get; set; }
        public int UserID { get; set; }
        public User User { get; set; }
        public string TicketType { get; set; }
        public double Price { get; set; }
        public DateTime DatePurchased { get; set; }
        public string BookingReference { get; set; }
        public ICollection<Payment> Payments { get; set; }
    }
}
