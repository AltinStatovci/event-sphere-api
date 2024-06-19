namespace EventSphere.Domain.DTOs.User
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? LastName { get; set; }
        public string Email { get; set; }
        public int RoleId { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
