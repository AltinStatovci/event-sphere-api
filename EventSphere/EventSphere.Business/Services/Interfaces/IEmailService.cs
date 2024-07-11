using EventSphere.Business.Helper;
using EventSphere.Domain.Entities;

namespace EventSphere.Business.Services.Interfaces;

public interface IEmailService
{
    Task SendEmailAsync(MailRequest mailRequest );
}