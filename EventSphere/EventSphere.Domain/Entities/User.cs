namespace EventSphere.Domain.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public byte[] Password { get; set; }
        public byte[] Salt { get; set; }
        public int RoleId { get; set; }
        public Role Role { get; set; }
        public string? RoleName { get; set; }
        public DateTime DateCreated { get; set; }
       // public ICollection<Ticket> Tickets { get; set; }
        public ICollection<Payment> Payments { get; set; }
    }
}
