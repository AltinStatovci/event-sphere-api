namespace EventSphere.Domain.Entities
{
    public class Payment
    {
        public int Id { get; set; }
        public User User { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public Ticket Ticket { get; set; }
        public int TicketId { get; set; }
        public string? TicketName { get; set; }
        public int Amount { get; set; }
        public string PaymentMethod { get; set; }
        public bool PaymentStatus { get; set; }
        public DateTime PaymentDate { get; set; }
    }
}
