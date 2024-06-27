using System.ComponentModel.DataAnnotations;

namespace EventSphere.Domain.DTOs.User
{
    public class UserDTO
    {
        [Required(ErrorMessage = "ID is required.")]
        public int ID { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Last Name is required.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid Email Address.")]
        public string Email { get; set; }
      
        [Required(ErrorMessage = "RoleID is required.")]
        public int RoleID { get; set; }

        public string? RoleName { get; set; }
        public DateTime DateCreated { get; set; }

    }
}
