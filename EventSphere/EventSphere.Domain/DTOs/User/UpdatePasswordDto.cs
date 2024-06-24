namespace EventSphere.Domain.DTOs.User
{

    public class UpdatePasswordDto
    {
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }

    }

}