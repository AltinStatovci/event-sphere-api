using EventSphere.Business.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using EventSphere.Domain.Entities;
using Microsoft.AspNetCore.Authorization;

namespace EventSphere.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PromoCodeController : ControllerBase
    {
        private readonly IPromoCodeService _promoCodeService;

        public PromoCodeController(IPromoCodeService promoCodeService)
        {
            _promoCodeService = promoCodeService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPromoCodes()
        {
            try
            {
                var promoCodes = await _promoCodeService.GetAllPromoCodesAsync();
                return Ok(promoCodes);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = "An error occurred while processing your request." });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPromoCodeById(int id)
        {
            try
            {
                var promoCode = await _promoCodeService.GetPromoCodeByIdAsync(id);
                if (promoCode == null)
                {
                    return NotFound();
                }
                return Ok(promoCode);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = "An error occurred while processing your request." });
            }
        }

        [HttpPost]
        [Authorize(Policy = "AdminOrOrganizer")]
        public async Task<IActionResult> CreatePromoCode(PromoCode promoCode)
        {
            try
            {
                var createdPromoCode = await _promoCodeService.CreatePromoCodeAsync(promoCode);
                return CreatedAtAction(nameof(GetPromoCodeById), new { id = createdPromoCode.ID }, createdPromoCode);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = "An error occurred while processing your request." });
            }
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "AdminOrOrganizer")]
        public async Task<IActionResult> UpdatePromoCode(int id, PromoCode promoCode)
        {
            if (id != promoCode.ID)
            {
                return BadRequest(new { Error = "Invalid ID or promo code data." });
            }

            try
            {
                await _promoCodeService.UpdatePromoCodeAsync(promoCode);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = "An error occurred while processing your request." });
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "AdminOrOrganizer")]
        public async Task<IActionResult> DeletePromoCode(int id)
        {
            try
            {
                await _promoCodeService.DeletePromoCodeAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = "An error occurred while processing your request." });
            }
        }

        [HttpPost("validate")]
        [Authorize(Policy = "AdminOrOrganizer")]
        public async Task<IActionResult> ValidatePromoCode([FromBody] string code)
        {
            var isValid = await _promoCodeService.IsPromoCodeValidAsync(code);
            if (isValid)
            {
                return Ok(new { Valid = true });
            }
            return BadRequest(new { Valid = false });
        }
    }
}
