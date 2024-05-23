

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
        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
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
        public async Task<ActionResult<Payment>> CreateAsync(PaymentDTO paymentDto)
        {
            var payment = await _paymentService.AddPaymentAsync(paymentDto);
            return CreatedAtAction(nameof(GetPaymentAsync), new { id = payment.ID }, payment);
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
