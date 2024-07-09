using EventSphere.Business.Services.Interfaces;
using EventSphere.Domain.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System;
using System.Security.Claims;
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
    

        public PaymentController(IPaymentService paymentService, IEmailService emailService)
        {
            _paymentService = paymentService;
            _emailService = emailService;
         
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
            var userEmail = HttpContext.User.FindFirst(ClaimTypes.Email)?.Value;
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
                Log.Information("Payment created successfully: by  {userEmail}", userEmail);
                return CreatedAtAction(nameof(GetPaymentId), new { id = paymentDTO.ID }, paymentDTO);
            }
            catch (Exception ex)
            {
                Log.Fatal("An error occurred while creating the payment: by  {userEmail}", userEmail);
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, PaymentDTO paymentDTO)
        {
            var userEmail = HttpContext.User.FindFirst(ClaimTypes.Email)?.Value;
            if (id != paymentDTO.ID)
            {
                Log.Error("Invalid ID or payment data.");
                return BadRequest(new { Error = "Invalid ID or payment data." });
            }

            try
            {
                await _paymentService.UpdatePaymentAsync(id, paymentDTO);
                Log.Information("Payment updated successfully: by {userEmail}", userEmail);
                return NoContent();
            }
            catch (Exception ex)
            {
                Log.Fatal("An error occurred while updating the payment: by  {userEmail}", userEmail);
                return StatusCode(500, new { Error = "An error occurred while processing your request." });
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var userEmail = HttpContext.User.FindFirst(ClaimTypes.Email)?.Value;
            try
            {
                await _paymentService.DeletePaymentAsync(id);
                Log.Information("Payment deleted successfully: {Id} by {userEmail}", id, userEmail); ;
                return NoContent();
            }
            catch (Exception ex)
            {
                Log.Fatal("An error occurred while deleting the payment: by  {userEmail}", userEmail);
                return StatusCode(500, new { Error = "An error occurred while processing your request." });
            }
        }
        [HttpPost("validatePromoCode")]
        public async Task<IActionResult> ValidatePromoCode([FromBody] PromoCodeDTO request)
        {
            var discount = await _paymentService.ValidatePromoCodeAsync(request.Code);
            if (discount != null)
            {
                return Ok(new { Discount = discount });
            }
            return BadRequest("Invalid or expired promo code.");
        }
    }
}
