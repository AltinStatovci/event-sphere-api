using EventSphere.Business.Services.Interfaces;
using EventSphere.Domain.DTOs;
using EventSphere.Domain.DTOs.EventSphere.API.DTOs;
using EventSphere.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EventSphere.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventController : ControllerBase
    {
        private readonly IEventService _eventService;

        public EventController(IEventService eventService)
        {
            _eventService = eventService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Event>>> GetEventName()
        {
            var eventName = await _eventService.GetAllEventsAsync();
            return Ok(eventName);
        }
        [HttpGet("count")]
        public async Task<ActionResult<int>> GetEventCount()
        {
            var count = await _eventService.GetEventCountAsync();
            return Ok(count);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Event>> GetEventName(int id)
        {
            var eventName = await _eventService.GetEventsByIdAsync(id);
            if (eventName == null)
            {
                return NotFound();
            }
            return Ok(eventName);
        }


        [HttpPost]
        public async Task<ActionResult<Event>> CreateEvent([FromForm] EventDTO eventDto, IFormFile image)
        {
            if (eventDto == null || image == null || image.Length == 0)
            {
                return BadRequest();
            }

            try
            {
                var createdEvent = await _eventService.CreateEventsAsync(eventDto, image);
                return CreatedAtAction(nameof(GetEventName), new { id = createdEvent.ID }, createdEvent);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error occurred while creating the event.");
            }
        }
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateEvent(int id, [FromForm] EventDTO eventDto, IFormFile newImage)
        {
            if (id == 0 || eventDto == null)
            {
                return BadRequest("Invalid ID or event data.");
            }

            try
            {
                await _eventService.UpdateEventsAsync(id, eventDto, newImage);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while updating the event. Please try again later.");
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCategory(int id)
        {
            await _eventService.DeleteEventsAsync(id);
            return NoContent();
        }
        [HttpGet("{id}/eventCategory")]
        public async Task<ActionResult<IEnumerable<Event>>> GetEventByCategoryIdAsync(int id)
        {
            var events = await _eventService.GetEventByCategoryId(id);
            return Ok(events);
        }
    }
}
