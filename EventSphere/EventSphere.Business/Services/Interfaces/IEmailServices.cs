using EventSphere.Business.Helper;
using EventSphere.Domain.Entities;

namespace EventSphere.Business.Services.Interfaces;

public interface IEmailServices
{
    Task SendEmailAsync(MailRequest mailRequest );
}