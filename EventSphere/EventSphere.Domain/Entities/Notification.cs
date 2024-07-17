using System.ComponentModel.DataAnnotations;

namespace EventSphere.Domain.Entities;

public class Notification
{
    public int Id { get; set; }
    [Required]
    public int UserId { get; set; }
    [Required]
    public string Message { get; set; }
    public bool IsRead { get; set; }
    public DateTime CreatedAt { get; set; }
    
}