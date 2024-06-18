namespace EventSphere.Domain.DTOs.User
{
    public class UserDTO
    {
        public int ID { get; set; }
        public string? Name { get; set; }
        public string? LastName { get; set; }
        public string Email { get; set; }
        public int RoleID { get; set; }
        public string? RoleName { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
