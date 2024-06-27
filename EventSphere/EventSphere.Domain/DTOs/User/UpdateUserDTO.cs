using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSphere.Domain.DTOs.User
{
    public class UpdateUserDTO
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
        public string? RoleName { get; set; }
        public int RoleID { get; set; }
        
    }

}
