using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using EventSphere.Business.Services.Interfaces;
using EventSphere.Domain.DTOs;
using Serilog;
using System.Security.Claims;

namespace EventSphere.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RCEventController : ControllerBase
    {
        private readonly IRCEventService _rcEventService;

        public RCEventController(IRCEventService rcEventService)
        {
            _rcEventService = rcEventService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllRCEvents()
        {
            try
            {
                var rcEvents = await _rcEventService.GetAllRCEventsAsync();
                return Ok(rcEvents);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while getting all RCEvents.");
                return StatusCode(500, new { Error = ex.Message });
            }
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetRCEventById(int id)
        {
            try
            {
                var rcEvent = await _rcEventService.GetRCEventByIdAsync(id);
                if (rcEvent == null)
                {
                    Log.Warning("RCEvent not found: {Id}", id);
                    return NotFound();
                }
                return Ok(rcEvent);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while getting RCEvent by ID: {Id}", id);
                return StatusCode(500, new { Error = "An error occurred while processing your request." });
            }
        }

        [HttpGet("GetRCEventsByUserId/{userId}")]
        public async Task<IActionResult> GetRCEventsByUserId(int userId)
        {
            try
            {
                var rcEvents = await _rcEventService.GetRCEventsByUserIdAsync(userId);
                return Ok(rcEvents);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while getting RCEvents by user ID: {UserId}", userId);
                return StatusCode(500, new { Error = "An error occurred while processing your request." });
            }
        }

        [HttpGet("user/{userId}/event/{eventId}")]
        public async Task<IActionResult> GetRCEventByUserIdAndEventId(int userId, int eventId)
        {
            var rcEvent = await _rcEventService.GetRCEventByUserIdAndEventIdAsync(userId, eventId);
            if (rcEvent == null) return NotFound();

            return Ok(rcEvent);
        }


        [HttpPost]
        public async Task<IActionResult> Create(RCEventDTO rcEventDTO)
        {
            var userEmail = HttpContext.User.FindFirst(ClaimTypes.Email)?.Value;
            if (rcEventDTO == null)
            {
                Log.Error("Received null RCEventDTO");
                return BadRequest(new { Error = "Invalid data. RCEventDTO cannot be null." });
            }

            if (!ModelState.IsValid)
            {
                Log.Error("Model state is invalid: {Errors}", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                return BadRequest(ModelState);
            }

            try
            {
                var rcEvent = await _rcEventService.CreateAsync(rcEventDTO);
                Log.Information("RCEvent created successfully: {@RCEvent} by {userEmail}", rcEventDTO, userEmail);
                return CreatedAtAction(nameof(GetRCEventById), new { id = rcEvent.Id }, rcEvent);
            }
            catch (ArgumentException ex)
            {
                Log.Error("Argument exception occurred while creating RCEvent: {Message}", ex.Message);
                return BadRequest(new { Error = ex.Message });
            }
            catch (Exception ex)
            {
                Log.Fatal("An error occurred while creating the RCEvent: {Message}", ex.Message);
                return StatusCode(500, new { Error = "An error occurred while processing your request." });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, RCEventDTO rcEventDTO)
        {
            var userEmail = HttpContext.User.FindFirst(ClaimTypes.Email)?.Value;
            if (id != rcEventDTO.Id)
            {
                Log.Error("Invalid ID or RCEvent data.");
                return BadRequest(new { Error = "Invalid ID or RCEvent data." });
            }

            try
            {
                await _rcEventService.UpdateAsync(id, rcEventDTO);
                Log.Information("RCEvent updated successfully: {@RCEvent} by {userEmail}", rcEventDTO, userEmail);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                Log.Warning("RCEvent not found while updating: {Message}", ex.Message);
                return NotFound(new { Error = ex.Message });
            }
            catch (Exception ex)
            {
                Log.Fatal("An error occurred while updating the RCEvent: {Message}", ex.Message);
                return StatusCode(500, new { Error = "An error occurred while processing your request." });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var userEmail = HttpContext.User.FindFirst(ClaimTypes.Email)?.Value;
            try
            {
                await _rcEventService.DeleteAsync(id);
                Log.Information("RCEvent deleted successfully: {Id} by {userEmail}", id, userEmail);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                Log.Warning("RCEvent not found while deleting: {Message}", ex.Message);
                return NotFound(new { Error = ex.Message });
            }
            catch (Exception ex)
            {
                Log.Fatal("An error occurred while deleting the RCEvent: {Message}", ex.Message);
                return StatusCode(500, new { Error = "An error occurred while processing your request." });
            }
        }
        [HttpGet("exists/user/{userId}/event/{eventId}")]
        public async Task<IActionResult> CheckRCEventExists(int userId, int eventId)
        {
            try
            {
                var rcEvent = await _rcEventService.GetRCEventByUserIdAndEventIdAsync(userId, eventId);
                return Ok(rcEvent != null);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while checking if RCEvent exists.");
                return StatusCode(500, new { Error = "An error occurred while processing your request." });
            }
        }


    }
}
