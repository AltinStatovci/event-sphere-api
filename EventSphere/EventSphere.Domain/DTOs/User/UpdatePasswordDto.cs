using System.ComponentModel.DataAnnotations;

namespace EventSphere.Domain.DTOs.User
{

    public class UpdatePasswordDto
    {
        public string CurrentPassword { get; set; }
        
        [Required(ErrorMessage = "Password is required.")]
        public string NewPassword { get; set; }

    }

}