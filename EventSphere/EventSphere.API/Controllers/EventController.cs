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
using System.Threading.Tasks;

namespace EventSphere.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventController : ControllerBase
    {
        private readonly IEventService _eventService;
        private readonly EventValidator _validator;
        private readonly ILogger<EventController> _logger;

        public EventController(IEventService eventService, ILogger<EventController> logger)
        {
            _eventService = eventService;
            _validator = new EventValidator();
            _logger = logger;
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
            if (eventDto == null || image == null || image.Length == 0)
            {
                Log.Error("Invalid event data or image.");
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
                Log.Information("Event created successfully: {@Event}", createdEvent);
                return CreatedAtAction(nameof(GetEventName), new { id = createdEvent.ID }, createdEvent);
            }
            catch (Exception ex)
            {
                Log.Fatal("An error occurred while creating the event: {@Error}", ex);
                return StatusCode(StatusCodes.Status500InternalServerError, new { Error = "Error occurred while creating the event." });
            }
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "AdminOrOrganizer")]
        public async Task<ActionResult> UpdateEvent(int id, [FromForm] EventDTO eventDto, IFormFile newImage)
        {
            if (id == 0 || eventDto == null)
            {
                Log.Error("Invalid ID or event data.");
                return BadRequest(new { Error = "Invalid ID or event data." });
            }

            try
            {
                await _eventService.UpdateEventsAsync(id, eventDto, newImage);
                Log.Information("Event updated successfully: {@Event}", eventDto);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                Log.Error("Event not found: {Id}", id);
                return NotFound(new { Error = ex.Message });
            }
            catch (Exception ex)
            {
                Log.Fatal("An error occurred while updating the event: {@Error}", ex);
                return StatusCode(500, new { Error = "An error occurred while updating the event. Please try again later." });
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "AdminOrOrganizer")]
        public async Task<ActionResult> DeleteCategory(int id)
        {
            try
            {
                await _eventService.DeleteEventsAsync(id);
                Log.Information("Event deleted successfully: {Id}", id);
                return NoContent();
            }
            catch (Exception ex)
            {
                Log.Fatal("An error occurred while deleting the event: {@Error}", ex);
                return StatusCode(500, new { Error = "An error occurred while processing your request." });
            }
        }

        [HttpGet("{id}/eventCategory")]
        public async Task<ActionResult<IEnumerable<Event>>> GetEventByCategoryIdAsync(int id)
        {
            try
            {
                var events = await _eventService.GetEventByCategoryId(id);
               
                return Ok(events);
            }
            catch (Exception ex)
            {
              
                return StatusCode(500, new { Error = "An error occurred while processing your request." });
            }
        }

        [HttpGet("{id}/organizer")]
        [Authorize(Policy = "AdminOrOrganizer")]
        public async Task<ActionResult<IEnumerable<Event>>> GetEventByOrganizerIdAsync(int id)
        {
            try
            {
                var events = await _eventService.GetEventByOrganizerId(id);
            
                return Ok(events);
            }
            catch (Exception ex)
            {
           
                return StatusCode(500, new { Error = "An error occurred while processing your request." });
            }
        }

        [HttpGet("{city}/city")]
        public async Task<ActionResult<IEnumerable<Event>>> GetEventsByCityAsync(string city)
        {
            try
            {
                var events = await _eventService.GetEventsByCity(city);
           
                return Ok(events);
            }
            catch (Exception ex)
            {
              
                return StatusCode(500, new { Error = "An error occurred while processing your request." });
            }
        }

        [HttpGet("{country}/country")]
        public async Task<ActionResult<IEnumerable<Event>>> GetEventsByCountryAsync(string country)
        {
            try
            {
                var events = await _eventService.GetEventsByCountry(country);
             
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
                var approvedEvent = await _eventService.UpdateEventStatus(id);
                return Ok(approvedEvent);
        }
    }
}
