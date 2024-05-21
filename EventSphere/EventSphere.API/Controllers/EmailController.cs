using EventSphere.Business.Helper;
using EventSphere.Business.Services.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace EventSphere.API.Controllers;
[ApiController]
[Route("api/[controller]")]
public class EmailController
{
    private readonly IEmailService _emailService;
    
    public EmailController(IEmailService emailService)
    {
        _emailService = emailService;
    }

    [HttpPost("email-send")]
    public async Task<IActionResult> SendEmail()
    {
        try
        {

            var mailRequest = new MailRequest
            {
                ToEmail = "altinstatovci56@hotmail.com",
                Subject = "Test Email",
                Body = @"
                         <p>Thank You <strong>UserName</strong> for buying a ticket to <strong>TestEvent</strong>.</p>
                         <p>The price of the ticket <strong>100$</strong>.</p>"
            };

            await _emailService.SendEmailAsync(mailRequest);
            return new OkResult();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
}