namespace EventSphere.Domain.DTOs
{
    public class UserDTO
    {
        public int ID { get; set; }
        public string? Name { get; set; }
        public string? LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int RoleID { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
