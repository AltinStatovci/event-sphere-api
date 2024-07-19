using EventSphere.Business.Services.Interfaces;
using EventSphere.Domain.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace EventSphere.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class NotificationController : ControllerBase
{
    private readonly INotificationService _notificationService;

    public NotificationController(INotificationService notificationService)
    {
        _notificationService = notificationService;
    }
    
    
    

    [HttpPost("send")]
    public async Task<IActionResult> SendNotification([FromBody] SendNotificationRequest request)
    {
        try
        {
            await _notificationService.SendNotificationAsync(request.UserId, request.Message);
            return Ok(new { Message = "Notification sent successfully." });
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, new { Message = "An error occurred while sending the notification." });
        }
    }
    
    [HttpPost("markAsRead/{notificationId}")]
    public async Task<IActionResult> MarkAsRead(int notificationId)
    {
        try
        {
            await _notificationService.MarkAsReadAsync(notificationId);
            return Ok(new { Message = "Notification marked as read successfully." });
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, new { Message = "An error occurred while marking the notification as read." });
        }
    }

    [HttpGet("unread/{userId}")]
    public async Task<IActionResult> GetUnreadNotifications(int userId)
    {
        try
        {
            var notifications = await _notificationService.GetUnreadNotificationsAsync(userId);
            return Ok(notifications);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, new { Message = "An error occurred while retrieving unread notifications." });
        }
    }
    
    [HttpPost("markAllAsRead/{userId}")]
    public async Task<IActionResult> MarkAllAsRead(int userId)
    {
        try
        {
            await _notificationService.MarkAllAsReadAsync(userId);
            return Ok(new { Message = "All notifications marked as read successfully." });
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, new { Message = "An error occurred while marking all notifications as read." });
        }
    }
}

