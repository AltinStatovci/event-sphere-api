using EventSphere.Business.Helper;
using EventSphere.Business.Services;
using EventSphere.Business.Services.Interfaces;
using EventSphere.Domain.DTOs;
using EventSphere.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> GetAllPayments()
        {
            var ticket = await _paymentService.GetAllPaymentsAsync();
            return Ok(ticket);
        }
        [HttpGet("count")]
        public async Task<ActionResult<int>> GetPaymentCount()
        {
            var count = await _paymentService.GetPaymentCountAsync();
            return Ok(count);
        }
        [HttpGet("{id}")]
        [Authorize(Policy = "All")]
        public async Task<IActionResult> GetPaymentId(int id)
        {
            var ticket = await _paymentService.GetPaymentByIdAsync(id);
            return Ok(ticket);
        }
        [HttpPost]
        [Authorize(Policy = "All")]
        public async Task<IActionResult> Create(PaymentDTO paymentDTO)
        {
            try
            {
                var paymentResponse = await _paymentService.AddPaymentAsync(paymentDTO);

                var mailRequest = new MailRequest
                {
                    ToEmail = paymentResponse.User.Email,
                    Subject = "Payment Confirmation",
                    Body = $@"
            <p>Thank you <strong>{paymentResponse.User.Name}</strong> for buying a ticket.</p>
            <p>The price of the ticket is <strong>{paymentResponse.Ticket.Price * paymentDTO.Amount:C}</strong>.</p>"
                };

                await _emailService.SendEmailAsync(mailRequest);
                return CreatedAtAction(nameof(GetPaymentId), new { id = paymentDTO.ID }, paymentDTO);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "All")]
        public async Task<IActionResult> Update(int id, PaymentDTO PaymentDTO)
        {
            await _paymentService.UpdatePaymentAsync(id, PaymentDTO);
            return NoContent();
        }
        [HttpDelete("{id}")]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            await _paymentService.DeletePaymentAsync(id);
            return NoContent();
        }
    }
}