using EventSphere.Domain.Entities;

namespace EventSphere.Domain.DTOs;

public class PaymentResponseDto
{
    public Payment Payment { get; set; }
    public Entities.User  User { get; set; }
    
}