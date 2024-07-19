using EventSphere.Business.Helper;
using EventSphere.Business.Services;
using EventSphere.Business.Services.Interfaces;
using EventSphere.Business.Validator;
using EventSphere.Domain.DTOs;
using EventSphere.Domain.DTOs.EventSphere.API.DTOs;
using EventSphere.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EventSphere.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventController : ControllerBase
    {
        private readonly IEventService _eventService;
        private readonly EventValidator _validator;
        private readonly IEmailService _emailService;
        
        public EventController(IEventService eventService, IEmailService emailService)
        {
            _eventService = eventService;
            _validator = new EventValidator();
            _emailService = emailService;

        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Event>>> GetEventName()
        {
            try
            {
                var eventName = await _eventService.GetAllEventsAsync();
              
                return Ok(eventName);
            }
            catch (Exception ex)
            {
      
                return StatusCode(500, new { Error = "An error occurred while processing your request." });
            }
        }

        [HttpGet("count")]
        public async Task<ActionResult<int>> GetEventCount()
        {
            try
            {
                var count = await _eventService.GetEventCountAsync();
              
                return Ok(count);
            }
            catch (Exception ex)
            {
             
                return StatusCode(500, new { Error = "An error occurred while processing your request." });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Event>> GetEventName(int id)
        {
            try
            {
                var eventName = await _eventService.GetEventsByIdAsync(id);
                if (eventName == null)
                {
                    Log.Error("Event not found: {Id}", id);
                    return NotFound();
                }
         
                return Ok(eventName);
            }
            catch (Exception ex)
            {
              
                return StatusCode(500, new { Error = "An error occurred while processing your request." });
            }
        }

        [HttpPost]
        [Authorize(Policy = "AdminOrOrganizer")]
        public async Task<ActionResult<Event>> CreateEvent([FromForm] EventDTO eventDto, IFormFile image)
        {
            
            var userEmail = HttpContext.User.FindFirst(ClaimTypes.Email)?.Value; 
            
            if (eventDto == null || image == null || image.Length == 0)
            {
                Log.Error("Invalid event data or image:.");
                return BadRequest(new { Error = "Invalid event data or image." });
            }

            var validationResult = _validator.Validate(eventDto);
            if (!validationResult.IsValid)
            {

                var errorMessages = validationResult.Errors.Select(error => error.ErrorMessage).ToList();
                Log.Error("Validation failed for event: {@Errors}", errorMessages);
                return BadRequest(new { Errors = errorMessages });

            }

            try
            {
                var createdEvent = await _eventService.CreateEventsAsync(eventDto, image);
                Log.Information("Event created successfully: {@Event} by {userEmail}", createdEvent, userEmail);
                return CreatedAtAction(nameof(GetEventName), new { id = createdEvent.ID }, createdEvent);
            }
            catch (Exception ex)
            {
                Log.Fatal("An error occurred while creating the event:  by {userEmail} ", userEmail);
                return StatusCode(StatusCodes.Status500InternalServerError, new { Error = "Error occurred while creating the event." });
            }
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "AdminOrOrganizer")]
        public async Task<ActionResult> UpdateEvent(int id, [FromForm] EventDTO eventDto, IFormFile newImage)
        {
            var userEmail = HttpContext.User.FindFirst(ClaimTypes.Email)?.Value; 
            if (id == 0 || eventDto == null)
            {
                Log.Error("Invalid ID or event data:");
                return BadRequest(new { Error = "Invalid ID or event data." });
            }

            try
            {
                await _eventService.UpdateEventsAsync(id, eventDto, newImage);
                Log.Information("Event updated successfully: {@Event} by {userEmail}", eventDto , userEmail);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                Log.Error("Event not found: {Id}", id);
                return NotFound(new { Error = ex.Message });
            }
            catch (Exception ex)
            {
                Log.Fatal("An error occurred while updating the event:  by {userEmail} ", userEmail);
                return StatusCode(500, new { Error = "An error occurred while updating the event. Please try again later." });
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "AdminOrOrganizer")]
        public async Task<ActionResult> DeleteCategory(int id)
        {
            var userEmail = HttpContext.User.FindFirst(ClaimTypes.Email)?.Value; 
            try
            {
                await _eventService.DeleteEventsAsync(id);
                Log.Information("Event deleted successfully: {Id}  by {userEmail}", id , userEmail);
                return NoContent();
            }
            catch (Exception ex)
            {
                Log.Fatal("An error occurred while deleting the event: by {userEmail} ", userEmail);
                return StatusCode(500, new { Error = "An error occurred while processing your request." });
            }
        }

        [HttpGet("{id}/eventCategory")]
        public async Task<ActionResult<IEnumerable<Event>>> GetEventByCategory(int id)
        {
            try
            {
                var events = await _eventService.GetEventByCategoryIdAsync(id);
               
                return Ok(events);
            }
            catch (Exception ex)
            {
              
                return StatusCode(500, new { Error = "An error occurred while processing your request." });
            }
        }

        [HttpGet("{id}/organizer")]
        [Authorize(Policy = "AdminOrOrganizer")]
        public async Task<ActionResult<IEnumerable<Event>>> GetEventByOrganizer(int id)
        {
            try
            {
                var events = await _eventService.GetEventByOrganizerIdAsync(id);
            
                return Ok(events);
            }
            catch (Exception ex)
            {
           
                return StatusCode(500, new { Error = "An error occurred while processing your request." });
            }
        }

        [HttpGet("{city}/city")]
        public async Task<ActionResult<IEnumerable<Event>>> GetEventsByCity(string city)
        {
            try
            {
                var events = await _eventService.GetEventsByCityAsync(city);
           
                return Ok(events);
            }
            catch (Exception ex)
            {
              
                return StatusCode(500, new { Error = "An error occurred while processing your request." });
            }
        }

        [HttpGet("{country}/country")]
        public async Task<ActionResult<IEnumerable<Event>>> GetEventsByCountry(string country)
        {
            try
            {
                var events = await _eventService.GetEventsByCountryAsync(country);
             
                return Ok(events);
            }
            catch (Exception ex)
            {
             
                return StatusCode(500, new { Error = "An error occurred while processing your request." });
            }
        }

        [AllowAnonymous]
        [HttpGet("getEventsByName")]
        public async Task<ActionResult<IEnumerable<Event>>> GetEventsByName([FromQuery] string name)
        {
            try
            {
                var events = await _eventService.GetEventsByNameAsync(name);
               
                return Ok(events);
            }
            catch (Exception ex)
            {
            
                return StatusCode(500, new { Error = "An error occurred while processing your request." });
            }
        }
        [HttpPost("approve/{id}")]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> ApproveEvent(int id)
        {
            var userEmail = HttpContext.User.FindFirst(ClaimTypes.Email)?.Value; 
                var approvedEvent = await _eventService.UpdateEventStatus(id);
                var email = await _eventService.GetOrganizerEmailAsync(id);
                var eventById = await _eventService.GetEventsByIdAsync(id); 
              

                var mailRequest = new MailRequest
            {
                ToEmail = email,
                Subject = "Event Approval Update",
                Body = $"<p>Dear {eventById.OrganizerName},</p><p> Your event submission for {eventById.EventName} was approved.</p><p>Best regards, EventSphere Team</p>",
            };
            await _emailService.SendEmailAsync(mailRequest);
            Log.Information("Event Approved successfully: {eventById}  by {userEmail}", eventById.EventName , userEmail);
            return Ok(approvedEvent);
        }

        [HttpGet("date")]
        public async Task<IActionResult> GetEventsByDate()
        {
            var data = DateTime.Now;
            var eventi = await _eventService.GetEventsByDateAsync(data);
            return Ok(eventi);

        }
        [HttpGet("datetime")]
        public async Task<IActionResult> GetEventsByDateTime()
        {
            var data = DateTime.Now;
            var eventi = await _eventService.GetEventsByDateTimeAsync(data);
            return Ok(eventi);

        }

        [HttpPost("reject")]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> RejectEvent([FromForm] int id, [FromForm] string message)
        {
            var userEmail = HttpContext.User.FindFirst(ClaimTypes.Email)?.Value; 
            var email = await _eventService.GetOrganizerEmailAsync(id);
            var eventById = await _eventService.GetEventsByIdAsync(id);

            await _eventService.UpdateMessage(id, message);

            var mailRequest = new MailRequest
            {
                ToEmail = email,
                Subject = "Event Approval Update",
                Body = $"<p>Dear {eventById.OrganizerName},</p><p>Unfortunately, your event submission for {eventById.EventName} was not approved for the following reason:</p><p>{message}</p><p>Best regards,</p><p>EventSphere Team</p>",
            };
            await _emailService.SendEmailAsync(mailRequest);
            Log.Information("Event Rejected : {event}  by {userEmail}", eventById.EventName , userEmail);

            return Ok();
        }
        [HttpGet("nearby")]
        public async Task<IActionResult> GetEventsNearby(double latitude, double longitude)
        {
            var events = await _eventService.GetEventsNearbyAsync(latitude, longitude);
            return Ok(events);
        }
    }
}