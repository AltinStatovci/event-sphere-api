using EventSphere.Business.Services.Interfaces;
using EventSphere.Domain.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System;
using System.Threading.Tasks;
using EventSphere.Business.Helper;

namespace EventSphere.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        private readonly IEmailService _emailService;
        private readonly ILogger<PaymentController> _logger;

        public PaymentController(IPaymentService paymentService, IEmailService emailService, ILogger<PaymentController> logger)
        {
            _paymentService = paymentService;
            _emailService = emailService;
            _logger = logger;
        }

        [HttpGet]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> GetAllPayments()
        {
            try
            {
                var payments = await _paymentService.GetAllPaymentsAsync();
              
                return Ok(payments);
            }
            catch (Exception ex)
            {
          
                return StatusCode(500, new { Error = "An error occurred while processing your request." });
            }
        }

        [HttpGet("count")]
        public async Task<ActionResult<int>> GetPaymentCount()
        {
            try
            {
                var count = await _paymentService.GetPaymentCountAsync();
              
                return Ok(count);
            }
            catch (Exception ex)
            {
              
                return StatusCode(500, new { Error = "An error occurred while processing your request." });
            }
        }

        [HttpGet("{id}")]
        [Authorize(Policy = "All")]
        public async Task<IActionResult> GetPaymentId(int id)
        {
            try
            {
                var payment = await _paymentService.GetPaymentByIdAsync(id);
                if (payment == null)
                {
                    Log.Error("Payment not found: {Id}", id);
                    return NotFound();
                }
             
                return Ok(payment);
            }
            catch (Exception ex)
            {
              
                return StatusCode(500, new { Error = "An error occurred while processing your request." });
            }
        }

        [HttpGet("user/{userId}")]
        [Authorize(Policy = "User")]
        public async Task<IActionResult> GetPaymentsByUserId(int userId)
        {
            try
            {
                var payments = await _paymentService.GetPaymentsByUserIdAsync(userId);
              
                return Ok(payments);
            }
            catch (Exception ex)
            {
              
                return StatusCode(500, new { Error = "An error occurred while processing your request." });
            }
        }

        [HttpGet("event/{eventId}")]
        [Authorize(Policy = "AdminOrOrganizer")]
        public async Task<IActionResult> GetPaymentsByEventId(int eventId)
        {
            try
            {
                var payments = await _paymentService.GetPaymentsByEventIdAsync(eventId);
           
                return Ok(payments);
            }
            catch (Exception ex)
            {
               
                return StatusCode(500, new { Error = "An error occurred while processing your request." });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(PaymentDTO paymentDTO)
        {
            if (paymentDTO == null)
            {
                Log.Error("Invalid payment data.");
                return BadRequest(new { Error = "Invalid payment data." });
            }

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

                Log.Information("Payment created successfully: {@Payment}", paymentResponse);
                return CreatedAtAction(nameof(GetPaymentId), new { id = paymentResponse.Payment.ID }, paymentResponse);
            }
            catch (Exception ex)
            {
                Log.Fatal("An error occurred while creating the payment: {@Error}", ex);
                return StatusCode(500, new { Error = "An error occurred while processing your request." });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, PaymentDTO paymentDTO)
        {
            if (id != paymentDTO.ID)
            {
                Log.Error("Invalid ID or payment data.");
                return BadRequest(new { Error = "Invalid ID or payment data." });
            }

            try
            {
                await _paymentService.UpdatePaymentAsync(id, paymentDTO);
                Log.Information("Payment updated successfully: {@Payment}", paymentDTO);
                return NoContent();
            }
            catch (Exception ex)
            {
                Log.Fatal("An error occurred while updating the payment: {@Error}", ex);
                return StatusCode(500, new { Error = "An error occurred while processing your request." });
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _paymentService.DeletePaymentAsync(id);
                Log.Information("Payment deleted successfully: {Id}", id);
                return NoContent();
            }
            catch (Exception ex)
            {
                Log.Fatal("An error occurred while deleting the payment: {@Error}", ex);
                return StatusCode(500, new { Error = "An error occurred while processing your request." });
            }
        }
    }
}
