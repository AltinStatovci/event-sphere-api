namespace EventSphere.Domain.Entities
{
    public class Payment
    {
        public int ID { get; set; }
        public User User { get; set; }
        public int UserID { get; set; }
        public Ticket Ticket { get; set; }
        public int TicketID { get; set; }
        public int Amount { get; set; }
        public string PaymentMethod { get; set; }
        public bool PaymentStatus { get; set; }
        public DateTime PaymentDate { get; set; }
    }
}
