using EventSphere.Business.Helper;
using EventSphere.Business.Services.Interfaces;
using EventSphere.Domain.DTOs;
using EventSphere.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace EventSphere.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        private readonly IEmailService _emailService;

        public PaymentController(IPaymentService paymentService, IEmailService emailService)
        {
            _paymentService = paymentService;
            _emailService = emailService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Payment>>> GetPaymentsAsync()
        {
            await _paymentService.GetAllPaymentsAsync();
            return Ok();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Payment>> GetPaymentAsync(int id)
        {
            var payment = await _paymentService.GetPaymentByIdAsync(id);
            if (payment == null)
            {
                return NotFound();
            }

            return Ok(payment);
        }

        [HttpPost]
        public async Task<ActionResult<PaymentResponseDto>> CreateAsync(PaymentDTO paymentDto)
        {
            try
            {
                // Create the payment and retrieve user and event details
                var paymentResponse = await _paymentService.AddPaymentAsync(paymentDto);

                // Prepare the email details
                var mailRequest = new MailRequest
                {
                    ToEmail = paymentResponse.User.Email, // Assume User object has an Email property
                    Subject = "Payment Confirmation",
                    Body = $@"
                <p>Thank You <strong>{paymentResponse.User.Name}</strong> for buying a ticket to <strong>{paymentResponse.Event.EventName}</strong>.</p>
                <p>The price of the ticket is <strong>{paymentResponse.Payment.Amount:C}</strong>.</p>"
                };

                // Send the email
                await _emailService.SendEmailAsync(mailRequest);

                // Return the response
                return CreatedAtAction(nameof(GetPaymentAsync), new { id = paymentResponse.Payment.ID },
                    paymentResponse);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    

    [HttpPut("{id}")]
        public async Task<ActionResult<Payment>> EditAsync(int id, [FromBody] PaymentDTO paymentDto)
        {
            if (id == 0 || paymentDto == null)
            {
                return BadRequest();
            }
            await _paymentService.UdpatePaymentAsync(id, paymentDto);
            return NoContent();
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAsync(int id)
        {
            await _paymentService.DeletePaymentAsync(id);
            return NoContent();
        }
    }
}
