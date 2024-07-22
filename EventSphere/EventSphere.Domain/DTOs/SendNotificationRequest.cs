namespace EventSphere.Domain.DTOs;

public class SendNotificationRequest
{
    public int UserId { get; set; }
    public string Message { get; set; }
}