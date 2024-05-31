using EventSphere.Business.Services.Interfaces;
using EventSphere.Domain.DTOs;
using EventSphere.Domain.DTOs.EventSphere.API.DTOs;
using EventSphere.Domain.Entities;
using EventSphere.Infrastructure.Repositories.EventRepository;
using Microsoft.AspNetCore.Mvc;

namespace EventSphere.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventController : ControllerBase
    {
        private readonly IEventService _eventService;
        private readonly IEventRepository _eventRepository;

        public EventController(IEventService eventService, IEventRepository eventRepository)
        {
            _eventService = eventService;
            _eventRepository = eventRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Event>>> GetEventName()
        {
            var eventName = await _eventService.GetAllEventsAsync();
            return Ok(eventName);
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
        public async Task<ActionResult> CreateEvents([FromBody] EventDTO eventDto)
        {
            if (eventDto == null)
            {
                return BadRequest();
            }

            var createdEvent = await _eventService.CreateEventsAsync(eventDto);
            return CreatedAtAction(nameof(GetEventName), new { id = createdEvent.ID }, createdEvent);
        }
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateEvent(int id, [FromBody] EventDTO eventDto)
        {
            if (id == 0 || eventDto == null)
            {
                return BadRequest();
            }

            await _eventService.UpdateEventsAsync(id, eventDto);
            return NoContent();
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
            var events = await _eventRepository.GetEventByCategoryId(id);
            return Ok(events);
        }
    }
}
